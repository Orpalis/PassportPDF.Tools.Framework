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
            PassportManagerApi apiInstance = new PassportManagerApi(FrameworkGlobals.API_SERVER_URI);
            apiInstance.Configuration.AddDefaultHeader("X-PassportPDF-API-Key", passportId);

            Exception e = null;
            int pausems = 1000;

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
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 1000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static int GetSuggestedMaxClientThreads()
        {
            ConfigApi apiInstance = new ConfigApi(FrameworkGlobals.API_SERVER_URI);

            Exception e = null;
            int pausems = 1000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return apiInstance.GetSuggestedMaxClientThreads().Value.Value;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 1000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static List<string> GetSupportedFileExtensions()
        {
            PDFApi apiInstance = new PDFApi(FrameworkGlobals.API_SERVER_URI);

            Exception e = null;
            int pausems = 1000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
                    return apiInstance.GetPDFImportSupportedFileExtensions().Value;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 1000;
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
            ConfigApi apiInstance = new ConfigApi(FrameworkGlobals.API_SERVER_URI);
            Exception e = null;
            int pausems = 1000;

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
                        pausems += 1000;
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
            ConfigApi apiInstance = new ConfigApi(FrameworkGlobals.API_SERVER_URI);

            Exception e = null;
            int pausems = 1000;

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
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 1000;
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
            ConfigApi apiInstance = new ConfigApi(FrameworkGlobals.API_SERVER_URI);

            Exception e = null;
            int pausems = 1000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                try
                {
#if DEBUG

                    return new StringArrayResponse(null, new List<string>() { "fr", "eng" });
#else
                    return apiInstance.GetSupportedOCRLanguages();
#endif
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 1000;
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
            int pausems = 1000;

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
                        pausems += 1000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static PDFOCRResponse SendOCRRequest(PDFApi apiInstance, PDFOCRParameters ocrParameters, int workerNumber, string inputFilePath, OperationsManager.ProgressDelegate ocrOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 1000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                ocrOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
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
                        pausems += 1000;
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
            int pausems = 1000;

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
                        pausems += 1000;
                    }
                    else
                    {//last iteration
                        e = ex;
                    }
                }
            }

            throw (e);
        }


        public static PDFLoadDocumentResponse SendLoadDocumentMultipartRequest(PDFApi apiInstance, int workerNumber, string inputFilePath, string fileName, string conformance, Stream fileStream, string contentEncoding, OperationsManager.ProgressDelegate uploadOperationStartEventHandler)
        {
            Exception e = null;
            int pausems = 1000;

            for (int i = 0; i < FrameworkGlobals.MAX_RETRYING_REQUESTS; i++)
            {
                uploadOperationStartEventHandler.Invoke(workerNumber, inputFilePath, i);
                try
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    PDFLoadDocumentResponse response = apiInstance.LoadDocumentAsPDFMultipart(fileStream, contentEncoding: contentEncoding, conformance: conformance, fileName: fileName);

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < FrameworkGlobals.MAX_RETRYING_REQUESTS - 1)
                    {
                        Thread.Sleep(pausems); //marking a pause in case of cnx temporarily out and to avoid overhead.
                        pausems += 1000;
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