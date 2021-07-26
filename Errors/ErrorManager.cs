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
            errorMessage.Append(PassportPDFErrorUtilities.GetMessageFromResultCode(error.ResultCode));
            errorMessage.Append(": ");
            errorMessage.Append(fileName);

            if (!string.IsNullOrEmpty(error.ExtResultMessage))
            {
                errorMessage.Append(" - ");
                errorMessage.Append(error.ExtResultMessage);
            }
            else if (error.ExtResultStatus != null && error.ExtResultStatus != "OK")
            {
                errorMessage.Append(" - " + FrameworkGlobals.MessagesLocalizer.GetString("status", FrameworkGlobals.ApplicationLanguage) + error.ExtResultStatus);
            }

            if (error.InternalErrorId != null && !string.IsNullOrEmpty(error.InternalErrorId))
            {
                errorMessage.Append(" - " + FrameworkGlobals.MessagesLocalizer.GetString("internal_error_id_message", FrameworkGlobals.ApplicationLanguage) + (" ") + error.InternalErrorId);
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

            if (!string.IsNullOrEmpty(reduceError.ExtErrorMessage))
            {
                errorMessage.Append(" - ");
                errorMessage.Append(reduceError.ExtErrorMessage);
            }

            return errorMessage.ToString();
        }


        public static string GetMessageFromException(Exception exception, string fileName)
        {
            StringBuilder errorMessage = new StringBuilder();

            if (exception is ApiException apiException)
            {
                if (apiException.ErrorCode != null && apiException.ErrorCode != 0)
                {
                    errorMessage.Append(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_server_returned_http_error", FrameworkGlobals.ApplicationLanguage), fileName: fileName, httpCode: (int?)apiException.ErrorCode, additionalMessage: exception.Message));
                }
                else
                {
                    errorMessage.Append(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_reaching_remote_server_failure", FrameworkGlobals.ApplicationLanguage), fileName: fileName, additionalMessage: exception.Message));
                }
            }
            else
            {
                errorMessage.Append(LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_unexpected_error", FrameworkGlobals.ApplicationLanguage), fileName: fileName));
            }


            if (!string.IsNullOrEmpty(exception.Message))
            {
                errorMessage.Append(" " + LogMessagesUtils.ReplaceMessageSequencesAndReferences(FrameworkGlobals.MessagesLocalizer.GetString("message_exception_message", FrameworkGlobals.ApplicationLanguage), additionalMessage: exception.Message));
            }

            return errorMessage.ToString();
        }
    }
}
