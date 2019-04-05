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
            pdfApiInstance = new PDFApi(FrameworkGlobals.PassportPdfApiUri);
            pdfApiInstance.Configuration.AddDefaultHeader("X-PassportPDF-API-Key", apiKey);
            pdfApiInstance.Configuration.Timeout = FrameworkGlobals.PassportPDFConfiguration.SuggestedClientTimeout;
            imageApiInstance = new ImageApi(FrameworkGlobals.PassportPdfApiUri);
            imageApiInstance.Configuration.AddDefaultHeader("X-PassportPDF-API-Key", apiKey);
            imageApiInstance.Configuration.Timeout = FrameworkGlobals.PassportPDFConfiguration.SuggestedClientTimeout;
        }


        private void Process(PDFApi pdfApiInstance, ImageApi imageApiInstance, int workerNumber, FileProductionRules fileProductionRules, OperationsWorkflow workflow, string destinationFolder, bool fileSizeReductionIsIntended)
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

                    WorkflowProcessingResult workFlowProcessingResult = ProcessWorkflow(pdfApiInstance, imageApiInstance, workflow, fileToProcess, workerNumber);

                    if (workFlowProcessingResult != null)
                    {
                        string outputFileAbsolutePath = destinationFolder + fileToProcess.FileRelativePath;

                        if (HandleOutputFileProduction(fileToProcess, fileProductionRules, workFlowProcessingResult, fileSizeReductionIsIntended, inputIsPDF, inputFileSize, outputFileAbsolutePath))
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
            byte[] producedFileData = null;
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
                        PDFReduceParameters.OutputVersionEnum outputVersion = (PDFReduceParameters.OutputVersionEnum)operation.Parameters;
                        PDFLoadDocumentResponse loadDocumentResponse = HandleLoadPDF(pdfApiInstance, outputVersion, fileToProcess, workerNumber);
                        if (loadDocumentResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "Load"));
                            return null;
                        }
                        remainingTokens = loadDocumentResponse.RemainingTokens.Value;
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
                        remainingTokens = imageLoadResponse.RemainingTokens.Value;
                        actionError = imageLoadResponse.Error;
                        fileID = imageLoadResponse.FileId;
                        break;

                    case Operation.OperationType.ReducePDF:
                        PDFReduceActionConfiguration reduceActionConfiguration = (PDFReduceActionConfiguration)operation.Parameters;
                        PDFReduceResponse reduceResponse = HandleReducePDF(pdfApiInstance, reduceActionConfiguration, fileToProcess, fileID, workerNumber, warningMessages);
                        if (reduceResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "Reduce"));
                            return null;
                        }
                        remainingTokens = reduceResponse.RemainingTokens.Value;
                        contentRemoved = (bool)reduceResponse.ContentRemoved;
                        versionChanged = (bool)reduceResponse.VersionChanged;
                        actionError = reduceResponse.Error;
                        reduceErrorInfo = reduceResponse.ErrorInfo;
                        linearized = reduceActionConfiguration.FastWebView;
                        break;

                    case Operation.OperationType.OCRPDF:
                        PDFOCRActionConfiguration ocrActionConfiguration = (PDFOCRActionConfiguration)operation.Parameters;
                        PDFOCRResponse ocrResponse = HandleOCRPDF(pdfApiInstance, ocrActionConfiguration, fileToProcess, fileID, workerNumber);
                        if (ocrResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "OCR"));
                            return null;
                        }
                        remainingTokens = ocrResponse.RemainingTokens.Value;
                        actionError = ocrResponse.Error;
                        break;

                    case Operation.OperationType.SavePDF:
                        PDFSaveDocumentResponse saveDocumentResponse = HandleSavePDF(pdfApiInstance, fileToProcess, fileID, workerNumber);
                        if (saveDocumentResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "Save"));
                            return null;
                        }
                        remainingTokens = saveDocumentResponse.RemainingTokens.Value;
                        actionError = saveDocumentResponse.Error;
                        producedFileData = saveDocumentResponse.Data;
                        break;

                    case Operation.OperationType.SaveImageAsPDFMRC:
                        ImageSaveAsPDFMRCActionConfiguration imageSaveAsPdfMrcActionConfiguration = (ImageSaveAsPDFMRCActionConfiguration)operation.Parameters;
                        ImageSaveAsPDFMRCResponse imageSaveAsPdfResponse = HandleSaveImageAsPDFMRC(imageApiInstance, imageSaveAsPdfMrcActionConfiguration, fileToProcess, fileID, workerNumber);
                        if (imageSaveAsPdfResponse == null)
                        {
                            OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_invalid_response_received", FrameworkGlobals.ApplicationLanguage), actionName: "PDF MRC"));
                            return null;
                        }
                        remainingTokens = imageSaveAsPdfResponse.RemainingTokens.Value;
                        actionError = imageSaveAsPdfResponse.Error;
                        producedFileData = imageSaveAsPdfResponse.PdfData;
                        break;
                }


                if (actionError != null)
                {
                    if (fileID != null)
                    {
                        TryCloseDocumentAsync(pdfApiInstance, fileID);
                    }
                    string errorMessage = reduceErrorInfo != null && reduceErrorInfo.ErrorCode != ReduceErrorInfo.ErrorCodeEnum.OK ? ErrorManager.GetMessageFromReduceActionError(reduceErrorInfo, fileToProcess.FileAbsolutePath) : ErrorManager.GetMessageFromPassportPDFError(actionError, operation.Type, fileToProcess.FileAbsolutePath);
                    OnError(errorMessage);
                    return null;
                }
                else
                {
                    RemainingTokensUpdateEventHandler.Invoke(remainingTokens);
                }
            }

            if (fileID != null)
            {
                TryCloseDocumentAsync(pdfApiInstance, fileID);
            }

            return producedFileData != null ? new WorkflowProcessingResult(contentRemoved, versionChanged, linearized, fileID, producedFileData, warningMessages) : null;
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
                    file = default(FileToProcess);
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


        private PDFLoadDocumentResponse HandleLoadPDF(PDFApi pdfApiInstance, PDFReduceParameters.OutputVersionEnum outputVersion, FileToProcess fileToProcess, int workerNumber)
        {
            FileStream inputFileStream = null;

            try
            {
                PassportPDFParametersUtilities.GetLoadDocumentMultipartParameters(fileToProcess.FileAbsolutePath, outputVersion, out inputFileStream, out string conformance, out string fileName);

                using (FileStream tmpFile = File.Create(Path.GetTempFileName(), 4096, FileOptions.DeleteOnClose))
                {
                    using (GZipStream dataStream = new GZipStream(tmpFile, CompressionLevel.Optimal, true))
                    {
                        inputFileStream.CopyTo(dataStream);
                        inputFileStream.Dispose();
                        inputFileStream = null;
                    }

                    tmpFile.Seek(0, SeekOrigin.Begin);
                    pdfApiInstance.Configuration.Timeout = FrameworkGlobals.PassportPDFConfiguration.SuggestedClientTimeout;

                    return PassportPDFRequestsUtilities.SendLoadPDFMultipartRequest(pdfApiInstance, workerNumber, fileToProcess.FileAbsolutePath, fileName, conformance, fileToProcess.Password, tmpFile, "Gzip", UploadOperationStartEventHandler);
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
                    imageApiInstance.Configuration.Timeout = FrameworkGlobals.PassportPDFConfiguration.SuggestedClientTimeout;

                    return PassportPDFRequestsUtilities.SendLoadImageMultipartRequest(imageApiInstance, workerNumber, fileToProcess.FileAbsolutePath, fileName, tmpFile, "Gzip", UploadOperationStartEventHandler);
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


        private PDFReduceResponse HandleReducePDF(PDFApi pdfApiInstance, PDFReduceActionConfiguration actionConfiguration, FileToProcess fileToProcess, string fileID, int workerNumber, List<string> warnings)
        {
            PDFReduceParameters reduceParameters = PassportPDFParametersUtilities.GetReduceParameters(actionConfiguration, fileID);
            PDFReduceResponse reduceResponse = PassportPDFRequestsUtilities.SendReduceRequest(pdfApiInstance, reduceParameters, workerNumber, fileToProcess.FileAbsolutePath, FileOperationStartEventHandler);

            if (reduceResponse.WarningsInfo != null)
            {
                foreach (ReduceWarningInfo warning in reduceResponse.WarningsInfo)
                {
                    warnings.Add(LogMessagesUtils.GetWarningStatustext(warning, fileToProcess.FileAbsolutePath));
                }
            }

            return reduceResponse;
        }


        private PDFOCRResponse HandleOCRPDF(PDFApi pdfApiInstance, PDFOCRActionConfiguration actionConfiguration, FileToProcess fileToProcess, string fileID, int workerNumber)
        {
            // First get the number of page of the PDF 
            PDFGetInfoResponse getInfoResponse = PassportPDFRequestsUtilities.SendGetInfoRequest(pdfApiInstance, new PDFGetInfoParameters(fileID), workerNumber, fileToProcess.FileAbsolutePath, FileOperationStartEventHandler);// todo: use appropriate event handler

            if (getInfoResponse.Error != null)
            {
                return null;
            }

            PDFOCRParameters ocrParameters = PassportPDFParametersUtilities.GetOCRParameters(actionConfiguration, fileID);

            int pageCount = getInfoResponse.PageCount.Value;
            int chunkLength = Math.Min(getInfoResponse.PageCount.Value, FrameworkGlobals.PAGE_CHUNK_LENGTH_FOR_OCR_ACTION);
            int chunkCount = getInfoResponse.PageCount.Value > FrameworkGlobals.PAGE_CHUNK_LENGTH_FOR_OCR_ACTION ? (int)Math.Ceiling(((double)getInfoResponse.PageCount.Value / FrameworkGlobals.PAGE_CHUNK_LENGTH_FOR_OCR_ACTION)) : 1;

            PDFOCRResponse ocrResponse = null;

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


        private PDFSaveDocumentResponse HandleSavePDF(PDFApi pdfApiInstance, FileToProcess fileToProcess, string fileID, int workerNumber)
        {
            PDFSaveDocumentParameters saveDocumentParameters = PassportPDFParametersUtilities.GetSaveDocumentParameters(fileID);

            return PassportPDFRequestsUtilities.SendSaveDocumentRequest(pdfApiInstance, saveDocumentParameters, workerNumber, fileToProcess.FileAbsolutePath, DownloadOperationStartEventHandler);
        }


        private ImageSaveAsPDFMRCResponse HandleSaveImageAsPDFMRC(ImageApi imageApiInstance, ImageSaveAsPDFMRCActionConfiguration actionConfiguration, FileToProcess fileToProcess, string fileID, int workerNumber)
        {
            ImageSaveAsPDFMRCParameters saveAsPdfMrcParameters = PassportPDFParametersUtilities.GetImageSaveAsPDFMRCParameters(actionConfiguration, fileID);

            return PassportPDFRequestsUtilities.SendSaveImageAsPDFMRCRequest(imageApiInstance, saveAsPdfMrcParameters, workerNumber, fileToProcess.FileAbsolutePath, DownloadOperationStartEventHandler);
        }


        private static async void TryCloseDocumentAsync(PDFApi pdfApiInstance, string fileID)
        {
            if (string.IsNullOrWhiteSpace(fileID))
            {
                throw new ArgumentNullException("FileID");
            }

            PDFCloseDocumentParameters closeDocumentParameters = new PDFCloseDocumentParameters(fileID);

            try
            {
                await pdfApiInstance.ClosePDFAsync(closeDocumentParameters); //we do not want to stop the process by waiting such response.
            }
            catch
            {
                return;
            }
        }


        private bool HandleOutputFileProduction(FileToProcess fileToProcess, FileProductionRules fileProductionRules, WorkflowProcessingResult workflowProcessingResult, bool fileSizeReductionIsIntended, bool inputIsPDF, long inputFileSize, string outputFileAbsolutePath)
        {
            bool outputIsInput = FileUtils.AreSamePath(fileToProcess.FileAbsolutePath, outputFileAbsolutePath);
            bool keepProducedFile = MustProducedFileBeKept(workflowProcessingResult, fileSizeReductionIsIntended, inputIsPDF, inputFileSize);

            // Save reduced document to output folder
            if (keepProducedFile)
            {
                FileUtils.SaveFile(workflowProcessingResult.ProducedFileData, fileToProcess.FileAbsolutePath, outputFileAbsolutePath, fileProductionRules.KeepWriteAndAccessTime);

                if (fileProductionRules.DeleteOriginalFileOnSuccess && !outputIsInput)
                {
                    try
                    {
                        FileUtils.DeleteFileEx(fileToProcess.FileAbsolutePath);
                    }
                    catch (Exception exception)
                    {
                        OnError(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_original_file_deletion_failure", FrameworkGlobals.ApplicationLanguage), fileName: fileToProcess.FileAbsolutePath, additionalMessage: exception.Message));
                        return false;
                    }
                }
            }
            else
            {
                if (!outputIsInput)
                {
                    FileUtils.EnsureDirectoryExists(Path.GetDirectoryName(outputFileAbsolutePath));
                    File.Copy(fileToProcess.FileAbsolutePath, outputFileAbsolutePath, true);

                    if (fileProductionRules.KeepWriteAndAccessTime)
                    {
                        FileUtils.SetOriginalLastAccessTime(fileToProcess.FileAbsolutePath, outputFileAbsolutePath);
                    }
                }

                if (fileSizeReductionIsIntended)
                {
                    // Inform file size reduction failure
                    workflowProcessingResult.WarningMessages.Add(LogMessagesUtils.GetWarningStatustext(new ReduceWarningInfo() { WarningCode = ReduceWarningInfo.WarningCodeEnum.FileSizeReductionFailure }, fileToProcess.FileAbsolutePath));
                }
            }

            return true;
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


        private static bool MustProducedFileBeKept(WorkflowProcessingResult workflowProcessingResult, bool fileSizeReductionIsIntended, bool inputIsPdf, float inputFileSize)
        {
            if (fileSizeReductionIsIntended)
            {
                return workflowProcessingResult.ProducedFileData.LongLength < inputFileSize || workflowProcessingResult.Linearized || !inputIsPdf || workflowProcessingResult.ContentRemoved || workflowProcessingResult.VersionChanged;
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
            public byte[] ProducedFileData { get; }
            public List<string> WarningMessages { get; }

            public WorkflowProcessingResult(bool contentRemoved, bool versionChanged, bool linearized, string fileID, byte[] producedFileData, List<string> warningMessages)
            {
                ContentRemoved = contentRemoved;
                VersionChanged = versionChanged;
                Linearized = linearized;
                FileID = fileID;
                ProducedFileData = producedFileData;
                WarningMessages = warningMessages;
            }
        }
    }
}