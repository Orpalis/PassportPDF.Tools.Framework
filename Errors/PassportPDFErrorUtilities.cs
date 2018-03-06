/**********************************************************************
 * Project:                 PassportPDF.Tools.Framework
 * Authors:					- Evan Carrère.
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
using PassportPDF.Model;
using static PassportPDF.Model.Error;
using PassportPDF.Tools.Framework.Utilities;

namespace PassportPDF.Tools.Framework.Errors
{
    public static class PassportPDFErrorUtilities
    {
        public static string GetErrorMessageFromResultCode(ResultcodeEnum? Resultcode) //Important: each new member added to ResultCodeEnum in a future version of the SDK must be handled here.
        {
            if (Resultcode == null)
            {
                throw new ArgumentNullException(nameof(Resultcode));
            }

            switch (Resultcode)
            {
                case ResultcodeEnum.OK: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_ OK", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.ActionTimedOut: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_ActionTimedOut", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.EmptyParameter: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_EmptyParameter", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.InvalidAPIKey: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_InvalidAPIKey", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.NotEnoughTokens: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_NotEnoughTokens", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.GenericError: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_GenericError", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.InvalidParameter: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_InvalidParameter", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.OutOfMemory: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_OutOfMemory", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.NotImplemented: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_NotImplemented", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.FileNotFound: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_FileNotFound", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.AccessDenied: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_AccessDenied", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotImportFileToPDF: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_CanNotImportFileToPDF", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfCanNotBeDecrypted: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_PdfCanNotBeDecrypted", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfOperationNotAllowed: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_PdfOperationNotAllowed", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfCanNotOpenFile: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_PdfCanNotOpenFile", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.PdfCanNotSaveFile: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_PdfCanNotSaveFile", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotCreateFile: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_CanNotCreateFile", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.NoDocumentProvided: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_NoDocumentProvided", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotRemovePage: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_CanNotRemovePage", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotSwapPages: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_CanNotSwapPages", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.InvalidPageRange: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_InvalidPageRange", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotReducePDF: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_ CanNotReducePDF", FrameworkGlobals.ApplicationLanguage);
                case ResultcodeEnum.CanNotExportMoreThan2GigabyteFile: return FrameworkGlobals.MessagesLocalizer.GetString("resultCode_message_ CanNotExportMoreThan2GigabyteFile", FrameworkGlobals.ApplicationLanguage);

                default: return "Unknown error"; //here for forward compatibility purpose.
            }
        }


        public static string GetErrorMessageFromReduceErrorInfo(ReduceErrorInfo reduceErrorInfo)
        {
            if (reduceErrorInfo.ErrorCode == null)
            {
                throw new ArgumentNullException(nameof(reduceErrorInfo));
            }

            switch (reduceErrorInfo.ErrorCode)
            {
                case ReduceErrorInfo.ErrorCodeEnum.OK: return FrameworkGlobals.MessagesLocalizer.GetString("errorCode_message_OK", FrameworkGlobals.ApplicationLanguage);
                case ReduceErrorInfo.ErrorCodeEnum.GetPageImagesCount: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("errorCode_message_GetPageImagesCount", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.PageNumber);
                case ReduceErrorInfo.ErrorCodeEnum.MRCPostOperationsFailure: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("errorCode_message_MRCPostOperationsFailure", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.PageNumber);
                case ReduceErrorInfo.ErrorCodeEnum.PageConversionFailure: return LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("errorCode_message_PageConversionFailure", FrameworkGlobals.ApplicationLanguage), pageNumber: reduceErrorInfo.PageNumber);
                case ReduceErrorInfo.ErrorCodeEnum.DocumentEncrypted: return FrameworkGlobals.MessagesLocalizer.GetString("errorCode_message_DocumentEncrypted", FrameworkGlobals.ApplicationLanguage);
                case ReduceErrorInfo.ErrorCodeEnum.UnexpectedError: return FrameworkGlobals.MessagesLocalizer.GetString("errorCode_message_UnexpectedError", FrameworkGlobals.ApplicationLanguage); //todo: remove

                default: return "Unknown reduce error";//here for forward compatibility purpose.
            }
        }
    }
}