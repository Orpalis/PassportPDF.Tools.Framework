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
using Orpalis.Globals.Localization;
using PassportPDF.Model;
using static PassportPDF.Model.Error;
using PassportPDF.Tools.Framework.Utilities;

namespace PassportPDF.Tools.Framework.Errors
{
    public static class PassportPDFErrorUtilities
    {
        private static readonly OrpalisLocalizer PassportPDFResultCodesMessagesLocalizer = new OrpalisLocalizer(AssemblyUtilities.GetResourceStream("l10n.PassportPDFResultCodesMessages.json"));

        public static string GetMessageFromResultCode(ResultcodeEnum? Resultcode) //Important: each new member added to ResultCodeEnum in a future version of the SDK must be handled here.
        {
            if (Resultcode == null)
            {
                throw new ArgumentNullException(nameof(Resultcode));
            }

            switch (Resultcode)
            {
                case ResultcodeEnum.OK: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_OK", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.ActionTimedOut: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_ActionTimedOut", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.EmptyParameter: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_EmptyParameter", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.InvalidAPIKey: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_InvalidAPIKey", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.NotEnoughTokens: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_NotEnoughTokens", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.GenericError: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_GenericError", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.InvalidParameter: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_InvalidParameter", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.InvalidColor: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_InvalidColor", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.OutOfMemory: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_OutOfMemory", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.NotImplemented: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_NotImplemented", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.FileNotFound: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_FileNotFound", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.AccessDenied: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_AccessDenied", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotImportFileToPDF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotImportFileToPDF", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfCanNotBeDecrypted: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfCanNotBeDecrypted", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfOperationNotAllowed: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfOperationNotAllowed", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfCanNotOpenFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfCanNotOpenFile", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfCanNotSaveFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfCanNotSaveFile", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotCreateFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotCreateFile", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.NoDocumentProvided: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_NoDocumentProvided", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotRemovePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRemovePage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSwapPages: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSwapPages", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotMovePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotMovePage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotRotatePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRotatePage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotFlipPage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotFlipPage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.InvalidPageRange: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_InvalidPageRange", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotReducePDF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotReducePDF", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotAutoDeskew: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotAutoDeskew", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSplit: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSplit", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSaveAsJPEG: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsJPEG", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotDigiSign: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDigiSign", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotProtect: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotProtect", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotConvertToPDFA: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotConvertToPDFA", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotAnnotate: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotAnnotate", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotClearPage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotClearPage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotMerge: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotMerge", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotGetPageThumbnail: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotGetPageThumbnail", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotGetDocumentPreview: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotGetDocumentPreview", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotRemovePageFormFields: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRemovePageFormFields", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotInsertImage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotInsertImage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotDrawImage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDrawImage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotInsertPageNumber: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotInsertPageNumber", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotInsertText: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotInsertText", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotReadBarcode: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotReadBarcode", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotFlattenFormFields: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotFlattenFormFields", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotExportMoreThan2GigabyteFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotExportMoreThan2GigabyteFile", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotOpenSession: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotOpenSession", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.UnknownOrExpiredSession: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_UnknownOrExpiredSession", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSaveFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveFile", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotRepairPDF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.UnsupportedImageFormat: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRepairPDF", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotOCR: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotOCR", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotOpenImage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotOpenImage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSaveAsPNG: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsPNG", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSaveAsTIFF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsTIFF", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSaveAsTIFFMultipage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsTIFFMultipage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSetInfo: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSetInfo", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSetPageBox: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSetPageBox", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotExtractPage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotExtractPage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotInsertNewPage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotInsertNewPage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotClonePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotClonePage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSetInitialView: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSetInitialView", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotAdjust: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotAdjust", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotResize: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotResize", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotFilter: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotFilter", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotCleanupDocument: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotCleanupDocument", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.UnknownDocumentFormat: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_UnknownDocumentFormat", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotCrop: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotCrop", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotRotate: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRotate", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotDetectColor: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDetectColor", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotConvertColorDepth: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotConvertColorDepth", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfCanNotAddFont: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfCanNotAddFont", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.ActionExecutionRejected: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_ActionExecutionRejected", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotExtractText: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotExtractText", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotDetectBlankPages: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDetectBlankPages", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotMICR: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotMICR", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotDetectPageOrientation: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDetectPageOrientation", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotDeletePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDeletePage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSaveAsPDF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsPDF", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotExtractImage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotExtractImage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotCloneImageRegion: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotCloneImageRegion", FrameworkGlobals.ApplicationLanguage);

                default: return "Unknown error"; //here for forward compatibility purpose.
            }
        }


        public static string GetErrorMessageFromReduceErrorInfo(ReduceErrorInfo reduceErrorInfo)
        {
            if (reduceErrorInfo.errorCode == null)
            {
                throw new ArgumentNullException(nameof(reduceErrorInfo));
            }

            switch (reduceErrorInfo.errorCode)
            {
                case ReduceErrorInfo.ErrorCodeEnum.OK: return PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_OK", FrameworkGlobals.ApplicationLanguage);
                case ReduceErrorInfo.ErrorCodeEnum.GetPageImagesCount: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_GetPageImagesCount", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.pageNumber);
                case ReduceErrorInfo.ErrorCodeEnum.MRCPostOperationsFailure: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_MRCPostOperationsFailure", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.pageNumber);
                case ReduceErrorInfo.ErrorCodeEnum.PageConversionFailure: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_PageConversionFailure", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.pageNumber);
                case ReduceErrorInfo.ErrorCodeEnum.DocumentEncrypted: return PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_DocumentEncrypted", FrameworkGlobals.ApplicationLanguage);
                case ReduceErrorInfo.ErrorCodeEnum.UnexpectedError: return PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_UnexpectedError", FrameworkGlobals.ApplicationLanguage);

                default: return "Unknown reduce error"; //here for forward compatibility purpose.
            }
        }
    }
}