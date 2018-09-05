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
using PassportPDF.Api;
using PassportPDF.Model;
using PassportPDF.Tools.Framework.Business;


namespace PassportPDF.Tools.Framework.Utilities
{
    public static class PassportPDFRequestsUtilities
    {
        public static PassportPDFPassport GetPassportInfo(string passportId)
        {
            PassportManagerApi apiInstance = new PassportManagerApi(FrameworkGlobals.PassportPdfApiUri);
            apiInstance.Configuration.AddDefaultHeader("X-PassportPDF-API-Key", passportId);
            Exception e = null;
            int pauseMs = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return apiInstance.GetPassportInfo(passportId);
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pauseMs); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pauseMs += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static int GetMaxClientThreads(string appId)
        {
            PassportPDFApplicationManagerApi passportPDFApplicationManagerApi = new PassportPDFApplicationManagerApi(FrameworkGlobals.PassportPdfApiUri);

            Exception e = null;
            int pauseMs = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return Math.Max(passportPDFApplicationManagerApi.GetMaxClientThreads(appId).Value.Value, 1);
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pauseMs); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pauseMs += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static string[] GetImageApiSupportedFileExtensions()
        {
            ImageApi apiInstance = new ImageApi(FrameworkGlobals.PassportPdfApiUri);

            Exception e = null;
            int pauseMs = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return apiInstance.GetSupportedImageFileExtensions().Value.ToArray();
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pauseMs); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pauseMs += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static string[] GetPdfApiSupportedFileExtensions()
        {
            PDFApi apiInstance = new PDFApi(FrameworkGlobals.PassportPdfApiUri);

            Exception e = null;
            int pauseMs = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return apiInstance.GetPDFImportSupportedFileExtensions().Value.ToArray();
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pauseMs); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pauseMs += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static int GetSuggestedClientTimeout()
        {
            ConfigApi apiInstance = new ConfigApi(FrameworkGlobals.PassportPdfApiUri);

            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return apiInstance.GetSuggestedClientTimeout().Value.Value;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static long GetMaxAllowedContentLength()
        {
            ConfigApi apiInstance = new ConfigApi(FrameworkGlobals.PassportPdfApiUri);

            Exception e = null;
            int pauseMs = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return apiInstance.GetMaxAllowedContentLength().Value.Value;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pauseMs); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pauseMs += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static StringArrayResponse GetAvailableOCRLanguages()
        {
            ConfigApi apiInstance = new ConfigApi(FrameworkGlobals.PassportPdfApiUri);

            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return apiInstance.GetSupportedOCRLanguages();
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static PDFReduceResponse SendReduceRequest(PDFApi apiInstance, PDFReduceParameters reduceParameters, int workerNumber, string inputFilePath, OperationsManager.ProgressDelegate reduceOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                reduceOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
                try
                {
                    PDFReduceResponse response = apiInstance.Reduce(reduceParameters);

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static PDFOCRResponse SendOCRRequest(PDFApi apiInstance, PDFOCRParameters ocrParameters, int workerNumber, string inputFilePath, string pageRange, int pageCount, OperationsManager.ChunkProgressDelegate chunkProgressEventHandler)
        {
            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                chunkProgressEventHandler.Invoke(workerNumber, inputFilePath, pageRange, pageCount, i);
                try
                {
                    PDFOCRResponse response = apiInstance.OCR(ocrParameters);

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static PDFSaveDocumentResponse SendSaveDocumentRequest(PDFApi apiInstance, PDFSaveDocumentParameters saveDocumentParameters, int workerNumber, string inputFilePath, OperationsManager.ProgressDelegate downloadOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                downloadOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
                try
                {
                    PDFSaveDocumentResponse response = apiInstance.SaveDocument(saveDocumentParameters);

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }

        public static ImageSaveAsPDFMRCResponse SendSaveImageAsPDFMRCRequest(ImageApi apiInstance, ImageSaveAsPDFMRCParameters imageSaveAsPdfMrcParameters, int workerNumber, string inputFilePath, OperationsManager.ProgressDelegate downloadOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                downloadOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
                try
                {
                    ImageSaveAsPDFMRCResponse response = apiInstance.SaveAsPDFMRC(imageSaveAsPdfMrcParameters);

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static PDFLoadDocumentResponse SendLoadPDFMultipartRequest(PDFApi apiInstance, int workerNumber, string inputFilePath, string fileName, string conformance, string password, Stream fileStream, string contentEncoding, OperationsManager.ProgressDelegate uploadOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                uploadOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
                try
                {
                    fileStream.Seek(0, SeekOrigin.Begin);

                    PDFLoadDocumentResponse response = apiInstance.LoadDocumentAsPDFMultipart(fileStream, contentEncoding: contentEncoding, conformance: conformance, password: password, fileName: fileName);

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static LoadImageResponse SendLoadImageMultipartRequest(ImageApi apiInstance, int workerNumber, string inputFilePath, string fileName, Stream fileStream, string contentEncoding, OperationsManager.ProgressDelegate uploadOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                uploadOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
                try
                {
                    fileStream.Seek(0, SeekOrigin.Begin);

                    LoadImageResponse response = apiInstance.LoadImageMultipart(fileStream, contentEncoding: contentEncoding, fileName: fileName);

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static PDFLoadDocumentResponse SendLoadDocumentRequest(PDFApi apiInstance, int workerNumber, string inputFilePath, string fileName, string conformance, string password, Stream fileStream, string contentEncoding, OperationsManager.ProgressDelegate uploadOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 5000;

            if (fileStream.Length > int.MaxValue)
            {
                throw new OutOfMemoryException();
            }

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                uploadOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
                try
                {
                    fileStream.Seek(0, SeekOrigin.Begin);

                    byte[] data = new byte[fileStream.Length];

                    fileStream.Read(data, 0, (int)fileStream.Length);

                    PDFLoadDocumentFromByteArrayParameters pdfLoadDocumentFromByteArrayParameters = new PDFLoadDocumentFromByteArrayParameters(data, fileName, password, Enum.Parse(typeof(PDFLoadDocumentFromByteArrayParameters.ConformanceEnum), conformance) as PDFLoadDocumentFromByteArrayParameters.ConformanceEnum?, Enum.Parse(typeof(PDFLoadDocumentFromByteArrayParameters.ContentEncodingEnum), contentEncoding) as PDFLoadDocumentFromByteArrayParameters.ContentEncodingEnum?);
                    PDFLoadDocumentResponse response = apiInstance.LoadDocumentAsPDF(pdfLoadDocumentFromByteArrayParameters);

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }

        public static PDFGetInfoResponse SendGetInfoRequest(PDFApi apiInstance, PDFGetInfoParameters getInfoParameters, int workerNumber, string inputFilePath, OperationsManager.ProgressDelegate getInfoOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 5000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                getInfoOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
                try
                {
                    return apiInstance.GetInfo(getInfoParameters);
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 2000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }
    }
}