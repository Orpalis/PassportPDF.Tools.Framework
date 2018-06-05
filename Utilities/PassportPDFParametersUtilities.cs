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

using System.IO;
using PassportPDF.Model;
using PassportPDF.Tools.Framework.Configuration;

namespace PassportPDF.Tools.Framework.Utilities
{
    internal static class PassportPDFParametersUtilities
    {
        public static void GetLoadDocumentMultipartParameters(string inputFileAbsolutePath, PDFReduceParameters.OutputVersionEnum outputVersion, out FileStream fileStream, out string conformance, out string fileName)
        {
            switch (outputVersion)
            {
                case PDFReduceParameters.OutputVersionEnum.PdfVersion14:
                    conformance = "PDF14";
                    break;
                case PDFReduceParameters.OutputVersionEnum.PdfVersion15:
                    conformance = "PDF15";
                    break;
                case PDFReduceParameters.OutputVersionEnum.PdfVersion16:
                    conformance = "PDF16";
                    break;
                case PDFReduceParameters.OutputVersionEnum.PdfVersion17:
                    conformance = "PDF17";
                    break;
                default:
                    conformance = "PDF15";
                    break;
            }
            fileStream = new FileStream(inputFileAbsolutePath, FileMode.Open);
            fileName = Path.GetFileName(inputFileAbsolutePath);
        }


        public static void GetLoadImageMultipartParameters(string inputFileAbsolutePath, out FileStream fileStream, out string fileName)
        {
            fileStream = new FileStream(inputFileAbsolutePath, FileMode.Open);
            fileName = Path.GetFileName(inputFileAbsolutePath);
        }


        public static PDFReduceParameters GetReduceParameters(PDFReduceActionConfiguration configuration, string fileID)
        {
            PDFReduceParameters reduceParameters = new PDFReduceParameters(fileID, configuration.OutputVersion,
                configuration.ImageQuality, configuration.RecompressImages, configuration.EnableColorDetection,
                configuration.PackDocument, configuration.PackFonts, configuration.DownscaleImages, configuration.DownscaleResolution,
                configuration.FastWebView, configuration.RemoveFormFields, configuration.RemoveAnnotations,
                configuration.RemoveBookmarks, configuration.RemoveHyperlinks, configuration.RemoveEmbeddedFiles, configuration.RemoveBlankPages,
                configuration.RemoveJavaScript, configuration.EnableJPEG2000, configuration.EnableJBIG2, configuration.EnableCharRepair, configuration.EnableMRC);

            return reduceParameters;
        }


        public static PDFOCRParameters GetOCRParameters(PDFOCRActionConfiguration configuration, string fileID)
        {
            PDFOCRParameters ocrParameters = new PDFOCRParameters(fileID, configuration.PageRange, configuration.OCRLanguage, configuration.SkipPagesWithText);

            return ocrParameters;
        }


        public static PDFSaveDocumentParameters GetSaveDocumentParameters(string fileID)
        {
            return new PDFSaveDocumentParameters(fileID);
        }
    }
}