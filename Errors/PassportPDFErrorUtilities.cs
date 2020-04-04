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

        public static string GetMessageFromResultCode(PassportPDFStatus resultCode) //Important: each new member added to PassportPDFStatus in a future version of the SDK must be handled here.
        {
            switch (resultCode)
            {
                case PassportPDFStatus.OK: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_OK", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.ActionTimedOut: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_ActionTimedOut", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.EmptyParameter: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_EmptyParameter", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.InvalidAPIKey: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_InvalidAPIKey", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.NotEnoughTokens: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_NotEnoughTokens", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.GenericError: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_GenericError", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.InvalidParameter: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_InvalidParameter", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.InvalidColor: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_InvalidColor", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.OutOfMemory: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_OutOfMemory", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.NotImplemented: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_NotImplemented", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.FileNotFound: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_FileNotFound", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.AccessDenied: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_AccessDenied", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotImportFileToPDF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotImportFileToPDF", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.PdfCanNotBeDecrypted: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfCanNotBeDecrypted", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.PdfOperationNotAllowed: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfOperationNotAllowed", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.PdfCanNotOpenFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfCanNotOpenFile", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.PdfCanNotSaveFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfCanNotSaveFile", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotCreateFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotCreateFile", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.NoDocumentProvided: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_NoDocumentProvided", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotRemovePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRemovePage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSwapPages: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSwapPages", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotMovePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotMovePage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotRotatePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRotatePage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotFlipPage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotFlipPage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.InvalidPageRange: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_InvalidPageRange", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotReducePDF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotReducePDF", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotAutoDeskew: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotAutoDeskew", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSplit: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSplit", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSaveAsJPEG: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsJPEG", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotDigiSign: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDigiSign", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotProtect: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotProtect", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotConvertToPDFA: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotConvertToPDFA", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotAnnotate: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotAnnotate", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotClearPage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotClearPage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotMerge: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotMerge", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotGetPageThumbnail: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotGetPageThumbnail", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotGetDocumentPreview: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotGetDocumentPreview", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotRemovePageFormFields: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRemovePageFormFields", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotInsertImage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotInsertImage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotDrawImage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDrawImage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotInsertPageNumber: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotInsertPageNumber", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotInsertText: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotInsertText", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotReadBarcode: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotReadBarcode", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotFlatten: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotFlattenFormFields", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotExportMoreThan2GigabyteFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotExportMoreThan2GigabyteFile", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotOpenSession: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotOpenSession", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.UnknownOrExpiredSession: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_UnknownOrExpiredSession", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSaveFile: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveFile", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotRepairPDF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRepairPDF", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.UnsupportedImageFormat: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_UnsupportedImageFormat", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotOCR: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotOCR", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotOpenImage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotOpenImage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSaveAsPNG: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsPNG", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSaveAsTIFF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsTIFF", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSaveAsTIFFMultipage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsTIFFMultipage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSetInfo: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSetInfo", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSetPageBox: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSetPageBox", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotExtractPage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotExtractPage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotInsertNewPage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotInsertNewPage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotClonePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotClonePage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSetInitialView: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSetInitialView", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotAdjust: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotAdjust", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotResize: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotResize", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotFilter: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotFilter", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotCleanupDocument: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotCleanupDocument", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.UnknownDocumentFormat: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_UnknownDocumentFormat", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotCrop: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotCrop", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotRotate: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotRotate", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotDetectColor: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDetectColor", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotConvertColorDepth: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotConvertColorDepth", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.PdfCanNotAddFont: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_PdfCanNotAddFont", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.ActionExecutionRejected: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_ActionExecutionRejected", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotExtractText: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotExtractText", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotDetectBlankPages: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDetectBlankPages", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotMICR: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotMICR", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotDetectPageOrientation: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDetectPageOrientation", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotDeletePage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotDeletePage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotSaveAsPDF: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotSaveAsPDF", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotExtractImage: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotExtractImage", FrameworkGlobals.ApplicationLanguage);
                case PassportPDFStatus.CanNotCloneImageRegion: return PassportPDFResultCodesMessagesLocalizer.GetString("resultCode_CanNotCloneImageRegion", FrameworkGlobals.ApplicationLanguage);

                default: return "Unknown error"; //here for forward compatibility purpose.
            }
        }


        public static string GetErrorMessageFromReduceErrorInfo(ReduceErrorInfo reduceErrorInfo)
        {
            switch (reduceErrorInfo.ErrorCode)
            {
                case ReduceErrorCode.OK: return PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_OK", FrameworkGlobals.ApplicationLanguage);
                case ReduceErrorCode.GetPageImagesCount: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_GetPageImagesCount", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.PageNumber);
                case ReduceErrorCode.MRCPostOperationsFailure: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_MRCPostOperationsFailure", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.PageNumber);
                case ReduceErrorCode.PageConversionFailure: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_PageConversionFailure", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.PageNumber);
                case ReduceErrorCode.DocumentEncrypted: return PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_DocumentEncrypted", FrameworkGlobals.ApplicationLanguage);
                case ReduceErrorCode.UnexpectedError: return PassportPDFResultCodesMessagesLocalizer.GetString("reduceErrorCode_UnexpectedError", FrameworkGlobals.ApplicationLanguage);

                default: return "Unknown reduce error"; //here for forward compatibility purpose.
            }
        }
    }
}