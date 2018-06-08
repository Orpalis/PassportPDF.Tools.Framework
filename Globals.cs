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

using System.Threading;
using Orpalis.Globals.Localization;
using PassportPDF.Model;
using PassportPDF.Tools.Framework.Configuration;
using PassportPDF.Tools.Framework.Utilities;
using PassportPDF.Tools.Framework.Models;

namespace PassportPDF.Tools.Framework
{
    public static class FrameworkGlobals
    {
        private const string API_SERVER_URI = "https://passportpdfapi.com";
        public const int MAX_RETRYING_REQUESTS = 3;

        // Default configuration
        public const bool TIMESTAMP_LOGS_DEFAULT = true;
        public const bool EXPORT_LOGS_DEFAULT = false;
        public const bool WARN_WHEN_SAME_INPUT_OUTPUT_DIRECTORY_DEFAULT = true;
        public const bool KEEP_WRITE_AND_ACCESS_TIME_DEFAULT = false;
        public const bool DELETE_ORIGINAL_FILE_ON_SUCCES_DEFAULT = false;
        public const bool PROCESS_SUBFOLDERS_DEFAULT = true;
        public const bool ONLY_PROCESS_PDF_DEFAULT = false;
        public const bool MINIMIZED_WINDOW_DEFAULT = false;
        public const int THREAD_NUMBER_DEFAULT = 1;
        public const bool AUTOMATICALLY_CHECK_FOR_UPDATE_DEFAULT = true;

        public static ApplicationConfiguration ApplicationConfiguration;
        public static PassportPDFConfiguration PassportPDFConfiguration;
        public static PassportInfo PassportInfo;

        public static readonly OrpalisLocalizer MessagesLocalizer = new OrpalisLocalizer(AssemblyUtilities.GetResourceStream("l10n.labels.json"));
        public static readonly LogsManager LogsManager = new LogsManager();

        public static string PassportPdfApiUri { get; set; } = API_SERVER_URI;


        public static string ApplicationLanguage
        {
            get
            {
                if (ApplicationConfiguration?.Language != null)
                {
                    return ApplicationConfiguration.Language;
                }
                else
                {
                    return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                }
            }
        }


        public static void FetchPassportPDFConfigurationEx(string appId)
        {
            PassportPDFConfiguration = new PassportPDFConfiguration(
                PassportPDFRequestsUtilities.GetMaxClientThreads(appId),
                PassportPDFRequestsUtilities.GetPdfApiSupportedFileExtensions(),
                PassportPDFRequestsUtilities.GetImageApiSupportedFileExtensions(),
                PassportPDFRequestsUtilities.GetSuggestedClientTimeout(),
                PassportPDFRequestsUtilities.GetMaxAllowedContentLength());
        }


        public static void SynchronizePassportInfoEx(string passportId)
        {
            PassportPDFPassport passportPdfPassport = PassportPDFRequestsUtilities.GetPassportInfo(passportId);
            PassportInfo = new PassportInfo
            {
                PassportNumber = passportPdfPassport.PassportId,
                IsActive = passportPdfPassport.IsActive.Value,
                SubscriptionDate = passportPdfPassport.SubscriptionDate.Value,
                TokensUsed = passportPdfPassport.CurrentTokensUsed.Value,
                RemainingTokens = passportPdfPassport.RemainingTokens.Value,
            };
        }
    }
}
