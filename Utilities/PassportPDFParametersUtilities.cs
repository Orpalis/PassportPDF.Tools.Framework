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
        public static void GetLoadDocumentMultipartParameters(string inputFileAbsolutePath, PdfVersion pdfVersion, out FileStream fileStream, out PdfConformance conformance, out string fileName)
        {
            switch (pdfVersion)
            {
                case PdfVersion.PdfVersion14:
                    conformance = PdfConformance.PDF14;
                    break;

                case PdfVersion.PdfVersion15:
                    conformance = PdfConformance.PDF15;
                    break;

                case PdfVersion.PdfVersion16:
                    conformance = PdfConformance.PDF16;
                    break;

                case PdfVersion.PdfVersion17:
                    conformance = PdfConformance.PDF17;
                    break;

                default:
                    conformance = PdfConformance.PDF15;
                    break;
            }

            fileStream = new FileStream(inputFileAbsolutePath, FileMode.Open, FileAccess.Read);
            fileName = Path.GetFileName(inputFileAbsolutePath);
        }


        public static void GetLoadImageMultipartParameters(string inputFileAbsolutePath, out FileStream fileStream, out string fileName)
        {
            fileStream = new FileStream(inputFileAbsolutePath, FileMode.Open, FileAccess.Read);
            fileName = Path.GetFileName(inputFileAbsolutePath);
        }


        public static PdfReduceParameters GetReduceParameters(PDFReduceActionConfiguration configuration, string fileId)
        {
            PdfReduceParameters reduceParameters = new PdfReduceParameters(fileId)
            {
                OutputVersion = configuration.OutputVersion,
                ImageQuality = configuration.ImageQuality,
                RecompressImages = configuration.RecompressImages,
                EnableColorDetection = configuration.EnableColorDetection,
                PackDocument = configuration.PackDocument,
                PackFonts = configuration.PackFonts,
                DownscaleImages = configuration.DownscaleImages,
                DownscaleResolution = configuration.DownscaleResolution,
                FastWebView = configuration.FastWebView,
                RemoveFormFields = configuration.RemoveFormFields,
                RemoveAnnotations = configuration.RemoveAnnotations,
                RemoveBookmarks = configuration.RemoveBookmarks,
                RemoveHyperlinks = configuration.RemoveHyperlinks,
                RemoveEmbeddedFiles = configuration.RemoveEmbeddedFiles,
                RemoveBlankPages = configuration.RemoveBlankPages,
                RemoveJavaScript = configuration.RemoveJavaScript,
                EnableJPEG2000 = configuration.EnableJPEG2000,
                EnableJBIG2 = configuration.EnableJBIG2,
                EnableCharRepair = configuration.EnableCharRepair,
                EnableMRC = configuration.EnableMRC,
                PreserveSmoothing = configuration.MRCPreserveSmoothing,
                DownscaleResolutionMRC = configuration.MRCDownscaleResolution,
                RemovePageThumbnails = configuration.RemovePageThumbnails,
                RemoveMetadata = configuration.RemoveMetadata
            };

            return reduceParameters;
        }


        public static PdfOCRParameters GetOCRParameters(PDFOCRActionConfiguration configuration, string fileId)
        {
            PdfOCRParameters ocrParameters = new PdfOCRParameters(fileId, configuration.PageRange)
            {
                Language = configuration.OCRLanguage,
                SkipPageWithText = configuration.SkipPagesWithText
            };

            return ocrParameters;
        }


        public static PdfSaveDocumentParameters GetSaveDocumentParameters(string fileId)
        {
            return new PdfSaveDocumentParameters(fileId);
        }


        public static ImageSaveAsPDFMRCParameters GetImageSaveAsPDFMRCParameters(ImageSaveAsPDFMRCActionConfiguration configuration, string fileId)
        {
            return new ImageSaveAsPDFMRCParameters(fileId)
            {
                Conformance = configuration.Conformance,
                ColorImageCompression = configuration.ColorImageCompression,
                BitonalImageCompression = configuration.BitonalImageCompression,
                ImageQuality = configuration.ImageQuality,
                DownscaleResolution = configuration.DownscaleImages ? configuration.DownscaleResolution : 0,
                PreserveSmoothing = configuration.PreserveSmoothing,
                FastWebView = configuration.FastWebView,
                JBIG2PMSThreshold = configuration.JBIG2PMSTreshold
            };
        }


        public static string GetChunkProcessingPageRange(int pageCount, int chunkLength, int chunkNumber, int chunkCount)
        {
            int startPage = chunkNumber == 1 ? 1 : chunkLength * (chunkNumber - 1) + 1;
            int endPage = startPage - 1 + (chunkNumber == chunkCount ? (pageCount % chunkLength == 0 ? chunkLength : pageCount % chunkLength) : chunkLength);
            return startPage != endPage ? string.Format("{0}-{1}", startPage, endPage) : startPage.ToString();
        }
    }
}