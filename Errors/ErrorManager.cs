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
using System.Text;
using PassportPDF.Client;
using PassportPDF.Model;
using PassportPDF.Tools.Framework.Utilities;
using PassportPDF.Tools.Framework.Business;

namespace PassportPDF.Tools.Framework.Errors
{
    public static class ErrorManager
    {
        public static string GetMessageFromPassportPDFError(Error error, Operation.OperationType failingOperation, string fileName)
        {
            StringBuilder errorMessage = new StringBuilder();

            errorMessage.Append("(" + failingOperation.ToString());
            errorMessage.Append(") ");
            errorMessage.Append(PassportPDFErrorUtilities.GetMessageFromResultCode(error.resultcode));
            errorMessage.Append(": ");
            errorMessage.Append(fileName);

            if (!string.IsNullOrEmpty(error.extResultMessage))
            {
                errorMessage.Append(" - ");
                errorMessage.Append(error.extResultMessage);
            }
            else if (error.extResultStatus != null && error.extResultStatus != "OK")
            {
                errorMessage.Append(" - " + FrameworkGlobals.MessagesLocalizer.GetString("status", FrameworkGlobals.ApplicationLanguage) + error.extResultStatus);
            }

            if (error.internalErrorId != null && !string.IsNullOrEmpty(error.internalErrorId))
            {
                errorMessage.Append(" - " + FrameworkGlobals.MessagesLocalizer.GetString("internal_error_id_message", FrameworkGlobals.ApplicationLanguage) + (" ") + error.internalErrorId);
            }

            return errorMessage.ToString();
        }


        public static string GetMessageFromReduceActionError(ReduceErrorInfo reduceError, string fileName)
        {
            StringBuilder errorMessage = new StringBuilder();

            errorMessage.Append("(Reduce) ");
            errorMessage.Append(PassportPDFErrorUtilities.GetErrorMessageFromReduceErrorInfo(reduceError));
            errorMessage.Append(": ");
            errorMessage.Append(fileName);

            if (!string.IsNullOrEmpty(reduceError.extErrorMessage))
            {
                errorMessage.Append(" - ");
                errorMessage.Append(reduceError.extErrorMessage);
            }

            return errorMessage.ToString();
        }


        public static string GetMessageFromException(Exception exception, string fileName)
        {
            StringBuilder errorMessage = new StringBuilder();

            errorMessage.Append("(" + FrameworkGlobals.MessagesLocalizer.GetString("label_client_exception", FrameworkGlobals.ApplicationLanguage) + ") ");

            if (exception.GetType() == typeof(ApiException))
            {
                ApiException apiException = (ApiException)exception;

                if (apiException.ErrorCode != 0)
                {
                    errorMessage.Append(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_server_returned_http_error", FrameworkGlobals.ApplicationLanguage), fileName: fileName, httpCode: apiException.ErrorCode));
                }
                else
                {
                    errorMessage.Append(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_reaching_remote_server_failure", FrameworkGlobals.ApplicationLanguage), fileName: fileName));
                }
                errorMessage.Append(" - " + apiException.ErrorContent);
                // Retrieve the name of called action which caused the exception
                if (exception.Message.StartsWith("Error calling"))
                {
                    string[] splitMessage = exception.Message.Split(' ');
                    if (splitMessage.Length > 3)
                    {
                        errorMessage.Append(" (" + splitMessage[2].TrimEnd(':') + ")");
                    }
                }
            }
            else
            {
                errorMessage.Append(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_unexpected_error", FrameworkGlobals.ApplicationLanguage), fileName: fileName, additionalMessage: exception.Message));
            }

            return errorMessage.ToString();
        }
    }
}
