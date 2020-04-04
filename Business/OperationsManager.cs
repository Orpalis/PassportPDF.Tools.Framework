/**********************************************************************
 * Project:                 PassportPDF.Tools.Framework
 * Authors:                 - Evan Carrère.
 *                          - Loïc Carrère.
 *
 * (C) Copyright 2018, ORPALIS.
 ** Licensed under the Apache License, Version 2.0 (the "License");
 ** you may not use this file except in compliance with the License.
 ** You may obtain a copy of the License at
 ** http://www.apache.org/licenses/LICENSE-2.0
 ** Unless required by applicable law or agreed to in writing, software
 ** distributed under the License is distributed on an "AS IS" BASIS,
 ** WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 ** See the License for the specific language governing permissions and
 ** limitations under the License.
 *
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.IO.Compression;
using PassportPDF.Api;
using PassportPDF.Model;
using PassportPDF.Tools.Framework.Models;
using PassportPDF.Tools.Framework.Utilities;
using PassportPDF.Tools.Framework.Configuration;
using PassportPDF.Tools.Framework.Errors;

namespace PassportPDF.Tools.Framework.Business
{
    // Note: the event handlers calls made in this class are protected by a mutex, but this responsability should be left to the consuming application.
    // Also, we assume that all the exposed events are always registered to by the consuming application, so we never check null.
    // This will be fixed in the future as the PassportPDF libraries and applications evolve.
    public sealed class OperationsManager
    {
        private readonly List<FileToProcess> _filesToProcess = new List<FileToProcess>();
        private readonly object _locker = new object();
        private readonly ManualResetEvent _waitHandle = new ManualResetEvent(true);

        private int _busyWorkersCount;
        private bool _workPaused;
        private bool _cancellationPending;


        public delegate void ProgressDelegate(int workerNumber, string fileName, int retries);
        public delegate void ChunkProgressDelegate(int workerNumber, string fileName, string pageRange, int pageCount, int retries);
        public delegate void ErrorDelegate(string errorMessage);
        public delegate void WarningDelegate(string warningMessage);
        public delegate void UpdateRemainingTokensDelegate(long remainingTokens);
        public delegate void FileOperationsCompletionDelegate(FileOperationsResult fileOperationsResult);
        public delegate void WorkerPauseDelegate(int workerNumber);
        public delegate void WorkerWorkCompletionDelegate(int workerNumber);
        public delegate void OperationsCompletionDelegate();

        public ProgressDelegate UploadOperationStartEventHandler;
        public ProgressDelegate FileOperationStartEventHandler;
        public ProgressDelegate DownloadOperationStartEventHandler;
        public ChunkProgressDelegate FileChunkProcessingProgressEventHandler;
        public ErrorDelegate ErrorEventHandler;
        public WarningDelegate WarningEventHandler;
        public UpdateRemainingTokensDelegate RemainingTokensUpdateEventHandler;
        public FileOperationsCompletionDelegate FileOperationsSuccesfullyCompletedEventHandler;
        public WorkerPauseDelegate WorkerPauseEventHandler;
        public WorkerWorkCompletionDelegate WorkerWorkCompletionEventHandler;
        public OperationsCompletionDelegate OperationsCompletionEventHandler;


        public void Feed(List<FileToProcess> filesToProcess)
        {
            lock (_locker)
            {
                _filesToProcess.AddRange(filesToProcess);
            }
        }


        public void Start(int workerCount, string destinationFolder, FileProductionRules fileProductionRules, OperationsWorkflow workflow, string apiKey)
        {
            lock (_locker)
            {
                _cancellationPending = false;
            }

            InitializeApiInstances(out PDFApi pdfApiInstance, out ImageApi imageApiInstance, apiKey);

            destinationFolder = ParsingUtils.EnsureFolderPathEndsWithBackSlash(destinationFolder);

            bool fileSizeReductionIsIntended = OperationsWorkflowUtilities.IsFileSizeReductionIntended(workflow);

            for (int i = 1; i <= workerCount; i++)
            {
                int workerNumber = i;
                // Launch the workers.
                Thread thread = new Thread(() => Process(pdfApiInstance, imageApiInstance, workerNumber, fileProductionRules, workflow, destinationFolder, fileSizeReductionIsIntended));
                thread.Start();
                _busyWorkersCount++;
            }
        }


        public bool PauseWork()
        {
            lock (_locker)
            {
                if (!_cancellationPending && _filesToProcess.Count > 0)
                {
                    _workPaused = true;
                    _waitHandle.Reset();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public void ResumeWork()
        {
            lock (_locker)
            {
                _workPaused = false;
                _waitHandle.Set();
            }
        }


        public void AbortWork()
        {
            lock (_locker)
            {
                _cancellationPending = true;
                _filesToProcess.Clear();
                if (_workPaused)
                {
                    // Resume worker threads so they can exit
                    ResumeWork();
                }
            }
        }


        private void InitializeApiInstances(out PDFApi pdfApiInstance, out ImageApi imageApiInstance, string apiKey)
        {
            pdfApiInstance = new PDFApi(apiKey)
            {
                BasePath = FrameworkGlobals.PassportPdfApiUri
            };

            imageApiInstance = new ImageApi(apiKey)
            {
                BasePath = FrameworkGlobals.PassportPdfApiUri
            };

            PassportPDF.Client.GlobalConfiguration.Timeout = FrameworkGlobals.PassportPDFConfiguration.SuggestedClientTimeout;
        }


        private void Process(PDFApi pdfApi, ImageApi imageApi, int workerNumber, FileProductionRules fileProductionRules, OperationsWorkflow workflow, string destinationFolder, bool fileSizeReductionIsIntended)
        {
            while (PickFile(out FileToProcess fileToProcess))
            {
                if (_cancellationPending)
                {
                    break;
                }

                try
                {
                    long inputFileSize = FileUtils.GetFileSize(fileToProcess.FileAbsolutePath);
                    bool inputIsPDF = Path.GetExtension(fileToProcess.FileAbsolutePath).ToUpper() == ".PDF";

                    if (!CheckInputFileSizeValidity(inputFileSize, fileToProcess.FileAbsolutePath))
                    {
                        continue;
                    }

                    WorkflowProcessingResult workFlowProcessingResult = ProcessWorkflow(pdfApi, imageApi, workflow, fileToProcess, workerNumber);

                    if (workFlowProcessingResult != null)
                    {
                        string outputFileAbsolutePath = destinationFolder + fileToProcess.FileRelativePath;

                        bool fileSuccesfullyProcessed = HandleOutputFileProduction(fileToProcess, workFlowProcessingResult.FileID, workerNumber, workflow.SaveOperation, workflow.SaveOperationConfiguration, pdfApi, imageApi, fileProductionRules, workFlowProcessingResult, fileSizeReductionIsIntended, inputIsPDF, inputFileSize, outputFileAbsolutePath);

                        TryCloseDocumentAsync(pdfApi, workFlowProcessingResult.FileID);

                        if (fileSuccesfullyProcessed)
                        {
                            OnFileSuccesfullyProcessed(new FileOperationsResult(fileToProcess.FileAbsolutePath, inputFileSize, FileUtils.GetFileSize(outputFileAbsolutePath), !inputIsPDF), workFlowProcessingResult.WarningMessages);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                catch (Exception exception)
                {
                    OnError(ErrorManager.GetMessageFromException(exception, fileToProcess.FileAbsolutePath));
                }

                if (_workPaused && !_cancellationPending)
                {
                    // If pause has been requested, wait for resume signal
                    WorkerPauseEventHandler.Invoke(workerNumber);
                    _waitHandle.WaitOne();
                }
            }

            OnWorkerWorkCompletion(workerNumber);
        }


        private WorkflowProcessingResult ProcessWorkflow(PDFApi pdfApiInstance, ImageApi imageApiInstance, OperationsWorkflow workflow, FileToProcess fileToProcess, int workerNumber)
        {
            List<string> warningMessages = new List<string>();
            bool contentRemoved = false;
            bool versionChanged = false;
            bool linearized = false;
            string fileID = null;

            foreach (Operation operation in workflow.OperationsToBePerformed)
            {
                Error actionError = null;
                ReduceErrorInfo reduceErrorInfo = null;
                long remainingTokens = 0;

                if (_cancellationPending)
                {
                    return null;
                }

                switch (operation.Type)
                {
                    case Operation.OperationType.LoadPDF:
                        PdfVersion outputVersion = (PdfVersion)operation.Parameters;
                        PdfLoadDocumentResponse loadDocumentResponse = HandleLoadPDF(pdfApiInstance, outputVersion, fileToProcess, workerNumber);
                        if (loadDocumentResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "Load"));
                            return null;
                        }
                        remainingTokens = loadDocumentResponse.RemainingTokens;
                        actionError = loadDocumentResponse.Error;
                        fileID = loadDocumentResponse.FileId;
                        break;

                    case Operation.OperationType.LoadImage:
                        ImageLoadResponse imageLoadResponse = HandleLoadImage(imageApiInstance, fileToProcess, workerNumber);
                        if (imageLoadResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "Load"));
                            return null;
                        }
                        remainingTokens = imageLoadResponse.RemainingTokens;
                        actionError = imageLoadResponse.Error;
                        fileID = imageLoadResponse.FileId;
                        break;

                    case Operation.OperationType.ReducePDF:
                        PDFReduceActionConfiguration reduceActionConfiguration = (PDFReduceActionConfiguration)operation.Parameters;
                        PdfReduceResponse reduceResponse = HandleReducePDF(pdfApiInstance, reduceActionConfiguration, fileToProcess, fileID, workerNumber, warningMessages);
                        if (reduceResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "Reduce"));
                            return null;
                        }
                        remainingTokens = reduceResponse.RemainingTokens;
                        contentRemoved = reduceResponse.ContentRemoved;
                        versionChanged = reduceResponse.VersionChanged;
                        actionError = reduceResponse.Error;
                        reduceErrorInfo = reduceResponse.ErrorInfo;
                        linearized = reduceActionConfiguration.FastWebView;
                        break;

                    case Operation.OperationType.OCRPDF:
                        PDFOCRActionConfiguration ocrActionConfiguration = (PDFOCRActionConfiguration)operation.Parameters;
                        PdfOCRResponse ocrResponse = HandleOCRPDF(pdfApiInstance, ocrActionConfiguration, fileToProcess, fileID, workerNumber);
                        if (ocrResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "OCR"));
                            return null;
                        }
                        remainingTokens = ocrResponse.RemainingTokens;
                        actionError = ocrResponse.Error;
                        break;
                }

                if (actionError != null)
                {
                    string errorMessage = reduceErrorInfo != null && reduceErrorInfo.ErrorCode != ReduceErrorCode.OK ? ErrorManager.GetMessageFromReduceActionError(reduceErrorInfo, fileToProcess.FileAbsolutePath) : ErrorManager.GetMessageFromPassportPDFError(actionError, operation.Type, fileToProcess.FileAbsolutePath);
                    OnError(errorMessage);
                    return null;
                }
                else
                {
                    RemainingTokensUpdateEventHandler.Invoke(remainingTokens);
                }
            }


            return new WorkflowProcessingResult(contentRemoved, versionChanged, linearized, fileID, warningMessages);
        }


        private bool PickFile(out FileToProcess file)
        {
            lock (_locker)
            {
                if (_filesToProcess.Count > 0)
                {
                    file = _filesToProcess[0];
                    _filesToProcess.RemoveAt(0);
                    return true;
                }
                else
                {
                    file = default;
                    return false;
                }
            }
        }


        private bool CheckInputFileSizeValidity(float inputFileSize, string inputFileAbsolutePath)
        {
            if (inputFileSize == 0)
            {
                OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_empty_file", FrameworkGlobals.ApplicationLanguage), fileName: inputFileAbsolutePath));
                return false;
            }
            else if (inputFileSize > FrameworkGlobals.PassportPDFConfiguration.MaxAllowedContentLength)
            {
                OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_input_file_too_large", FrameworkGlobals.ApplicationLanguage), fileName: inputFileAbsolutePath, inputSize: FrameworkGlobals.PassportPDFConfiguration.MaxAllowedContentLength));
                return false;
            }
            else
            {
                return true;
            }
        }


        private PdfLoadDocumentResponse HandleLoadPDF(PDFApi pdfApiInstance, PdfVersion outputVersion, FileToProcess fileToProcess, int workerNumber)
        {
            FileStream inputFileStream = null;

            try
            {
                PassportPDFParametersUtilities.GetLoadDocumentMultipartParameters(fileToProcess.FileAbsolutePath, outputVersion, out inputFileStream, out PdfConformance conformance, out string fileName);

                using (FileStream tmpFile = File.Create(Path.GetTempFileName(), 4096, FileOptions.DeleteOnClose))
                {
                    using (GZipStream dataStream = new GZipStream(tmpFile, CompressionLevel.Optimal, true))
                    {
                        inputFileStream.CopyTo(dataStream);
                        inputFileStream.Dispose();
                        inputFileStream = null;
                    }

                    tmpFile.Seek(0, SeekOrigin.Begin);

                    return PassportPDFRequestsUtilities.SendLoadPDFMultipartRequest(pdfApiInstance, workerNumber, fileToProcess.FileAbsolutePath, fileName, conformance, fileToProcess.Password, tmpFile, ContentEncoding.Gzip, UploadOperationStartEventHandler);
                }
            }
            catch
            {
                if (inputFileStream != null)
                {
                    inputFileStream.Dispose();
                }
                throw;
            }
        }


        private ImageLoadResponse HandleLoadImage(ImageApi imageApiInstance, FileToProcess fileToProcess, int workerNumber)
        {
            FileStream inputFileStream = null;

            try
            {
                PassportPDFParametersUtilities.GetLoadImageMultipartParameters(fileToProcess.FileAbsolutePath, out inputFileStream, out string fileName);

                using (FileStream tmpFile = File.Create(Path.GetTempFileName(), 4096, FileOptions.DeleteOnClose))
                {
                    using (GZipStream dataStream = new GZipStream(tmpFile, CompressionLevel.Optimal, true))
                    {
                        inputFileStream.CopyTo(dataStream);
                        inputFileStream.Dispose();
                        inputFileStream = null;
                    }

                    tmpFile.Seek(0, SeekOrigin.Begin);

                    return PassportPDFRequestsUtilities.SendLoadImageMultipartRequest(imageApiInstance, workerNumber, fileToProcess.FileAbsolutePath, fileName, tmpFile, ContentEncoding.Gzip, UploadOperationStartEventHandler);
                }
            }
            catch
            {
                if (inputFileStream != null)
                {
                    inputFileStream.Dispose();
                }
                throw;
            }
        }


        private PdfReduceResponse HandleReducePDF(PDFApi pdfApiInstance, PDFReduceActionConfiguration actionConfiguration, FileToProcess fileToProcess, string fileID, int workerNumber, List<string> warnings)
        {
            PdfReduceParameters reduceParameters = PassportPDFParametersUtilities.GetReduceParameters(actionConfiguration, fileID);
            PdfReduceResponse reduceResponse = PassportPDFRequestsUtilities.SendReduceRequest(pdfApiInstance, reduceParameters, workerNumber, fileToProcess.FileAbsolutePath, FileOperationStartEventHandler);

            if (reduceResponse.WarningsInfo != null)
            {
                foreach (ReduceWarningInfo warning in reduceResponse.WarningsInfo)
                {
                    warnings.Add(LogMessagesUtils.GetWarningStatustext(warning, fileToProcess.FileAbsolutePath));
                }
            }

            return reduceResponse;
        }


        private PdfOCRResponse HandleOCRPDF(PDFApi pdfApiInstance, PDFOCRActionConfiguration actionConfiguration, FileToProcess fileToProcess, string fileID, int workerNumber)
        {
            // First get the number of page of the PDF 
            PdfGetInfoResponse getInfoResponse = PassportPDFRequestsUtilities.SendGetInfoRequest(pdfApiInstance, new PdfGetInfoParameters(fileID), workerNumber, fileToProcess.FileAbsolutePath, FileOperationStartEventHandler);// todo: use appropriate event handler

            if (getInfoResponse.Error != null)
            {
                return null;
            }

            PdfOCRParameters ocrParameters = PassportPDFParametersUtilities.GetOCRParameters(actionConfiguration, fileID);

            int pageCount = getInfoResponse.PageCount;
            int chunkLength = Math.Min(getInfoResponse.PageCount, FrameworkGlobals.PAGE_CHUNK_LENGTH_FOR_OCR_ACTION);
            int chunkCount = getInfoResponse.PageCount > FrameworkGlobals.PAGE_CHUNK_LENGTH_FOR_OCR_ACTION ? (int)Math.Ceiling((double)getInfoResponse.PageCount / FrameworkGlobals.PAGE_CHUNK_LENGTH_FOR_OCR_ACTION) : 1;

            PdfOCRResponse ocrResponse = null;

            for (int chunkNumber = 1; chunkNumber <= chunkCount; chunkNumber++)
            {
                ocrParameters.PageRange = PassportPDFParametersUtilities.GetChunkProcessingPageRange(pageCount, chunkLength, chunkNumber, chunkCount);

                ocrResponse = PassportPDFRequestsUtilities.SendOCRRequest(pdfApiInstance, ocrParameters, workerNumber, fileToProcess.FileAbsolutePath, ocrParameters.PageRange, pageCount, FileChunkProcessingProgressEventHandler);

                if (_cancellationPending || ocrResponse == null)
                {
                    return ocrResponse;
                }
            }

            return ocrResponse;
        }


        private static async void TryCloseDocumentAsync(PDFApi pdfApiInstance, string fileID)
        {
            if (string.IsNullOrWhiteSpace(fileID))
            {
                throw new ArgumentNullException("FileID");
            }

            PdfCloseDocumentParameters closeDocumentParameters = new PdfCloseDocumentParameters(fileID);

            try
            {
                await pdfApiInstance.ClosePDFAsync(closeDocumentParameters); //we do not want to stop the process by waiting such response.
            }
            catch
            {
                return;
            }
        }


        private bool HandleOutputFileProduction(FileToProcess fileToProcess, string fileId, int workerNumber, OperationsWorkflow.SaveOperationType saveOperationType, object saveOperationConfiguration, PDFApi pdfApi, ImageApi imageApi, FileProductionRules fileProductionRules, WorkflowProcessingResult workflowProcessingResult, bool fileSizeReductionIsIntended, bool inputIsPDF, long inputFileSize, string outputFileAbsolutePath)
        {
            if (!DownloadAndSaveDocument(pdfApi, imageApi, fileId, workerNumber, saveOperationType, saveOperationConfiguration, DownloadOperationStartEventHandler, fileToProcess.FileAbsolutePath, out string downloadedDocumentFileName))
            {
                return false;
            }

            bool keepProducedFile = MustProducedFileBeKept(workflowProcessingResult, fileSizeReductionIsIntended, inputIsPDF, inputFileSize, FileUtils.GetFileSize(downloadedDocumentFileName));
            bool outputIsInput = FileUtils.AreSamePath(fileToProcess.FileAbsolutePath, outputFileAbsolutePath);

            if (keepProducedFile)
            {
                if (fileProductionRules.DeleteOriginalFileOnSuccess && !outputIsInput)
                {
                    try
                    {
                        FileUtils.DeleteFile(fileToProcess.FileAbsolutePath);
                    }
                    catch (Exception exception)
                    {
                        OnWarning(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_original_file_deletion_failure", FrameworkGlobals.ApplicationLanguage), fileName: fileToProcess.FileAbsolutePath, additionalMessage: exception.Message));
                    }
                }

                FileUtils.MoveFile(downloadedDocumentFileName, outputFileAbsolutePath);
                File.SetCreationTime(outputFileAbsolutePath, File.GetCreationTime(fileToProcess.FileAbsolutePath));
            }
            else
            {
                if (!outputIsInput)
                {
                    FileUtils.CopyFile(fileToProcess.FileAbsolutePath, outputFileAbsolutePath);
                }

                if (fileSizeReductionIsIntended)
                {
                    // Inform file size reduction failure
                    workflowProcessingResult.WarningMessages.Add(LogMessagesUtils.GetWarningStatustext(new ReduceWarningInfo() { WarningCode = ReduceWarningCode.FileSizeReductionFailure }, fileToProcess.FileAbsolutePath));
                }

                FileUtils.DeleteFile(downloadedDocumentFileName);
            }


            if (fileProductionRules.KeepWriteAndAccessTime)
            {
                FileUtils.SetOriginalLastAccessTime(fileToProcess.FileAbsolutePath, outputFileAbsolutePath);
            }

            return true;
        }


        private bool DownloadAndSaveDocument(PDFApi pdfApi, ImageApi imageApi, string fileId, int workerNumber, OperationsWorkflow.SaveOperationType saveOperationType, object saveOperationConfiguration, ProgressDelegate downloadOperationStartEventHandler, string inputFileAbsolutePath, out string downloadedDocumentFileName)
        {
            downloadedDocumentFileName = Path.GetTempFileName();

            using (FileStream outputFileStream = new FileStream(downloadedDocumentFileName, FileMode.Open))
            {
                try
                {
                    if (saveOperationType == OperationsWorkflow.SaveOperationType.SavePDF)
                    {
                        PassportPDFRequestsUtilities.DownloadPDF(pdfApi, new PdfSaveDocumentParameters(fileId), workerNumber, inputFileAbsolutePath, outputFileStream, downloadOperationStartEventHandler);
                    }
                    else if (saveOperationType == OperationsWorkflow.SaveOperationType.SaveImageAsPDFMRC)
                    {
                        PassportPDFRequestsUtilities.DownloadImageAsPDFMRC(imageApi, PassportPDFParametersUtilities.GetImageSaveAsPDFMRCParameters((ImageSaveAsPDFMRCActionConfiguration)saveOperationConfiguration, fileId),
                        workerNumber, inputFileAbsolutePath, outputFileStream, downloadOperationStartEventHandler);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_output_file_download_failure", FrameworkGlobals.ApplicationLanguage), inputFileAbsolutePath, additionalMessage: ex.Message));
                    return false;
                }
            }
        }


        private void OnFileSuccesfullyProcessed(FileOperationsResult result, List<string> warningMessages)
        {
            lock (_locker)
            {
                FileOperationsSuccesfullyCompletedEventHandler.Invoke(result);
            }

            if (warningMessages.Count > 0)
            {
                HandleActionsWarningMessages(warningMessages);
            }
        }


        private void HandleActionsWarningMessages(List<string> warningMessages)
        {
            foreach (string warningMessage in warningMessages)
            {
                OnWarning(warningMessage);
            }
        }


        private void OnError(string errorMessage)
        {
            lock (_locker)
            {
                ErrorEventHandler.Invoke(errorMessage);
            }
        }


        private void OnWarning(string warningMessage)
        {
            lock (_locker)
            {
                WarningEventHandler.Invoke(warningMessage);
            }
        }


        private void OnWorkerWorkCompletion(int workerNumber)
        {
            lock (_locker)
            {
                WorkerWorkCompletionEventHandler.Invoke(workerNumber);

                _busyWorkersCount -= 1;

                if (_busyWorkersCount == 0)
                {
                    OperationsCompletionEventHandler.Invoke();
                }
            }
        }


        private static bool MustProducedFileBeKept(WorkflowProcessingResult workflowProcessingResult, bool fileSizeReductionIsIntended, bool inputIsPdf, float inputFileSize, float producedFileSize)
        {
            if (fileSizeReductionIsIntended)
            {
                return producedFileSize < inputFileSize || workflowProcessingResult.Linearized || !inputIsPdf || workflowProcessingResult.ContentRemoved || workflowProcessingResult.VersionChanged;
            }
            else
            {
                return true;
            }
        }


        private sealed class WorkflowProcessingResult
        {
            public bool Linearized { get; }
            public bool ContentRemoved { get; }
            public bool VersionChanged { get; }
            public string FileID { get; }
            public List<string> WarningMessages { get; }

            public WorkflowProcessingResult(bool contentRemoved, bool versionChanged, bool linearized, string fileID, List<string> warningMessages)
            {
                ContentRemoved = contentRemoved;
                VersionChanged = versionChanged;
                Linearized = linearized;
                FileID = fileID;
                WarningMessages = warningMessages;
            }
        }
    }
}