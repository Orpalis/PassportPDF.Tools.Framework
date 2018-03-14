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
using System.Globalization;

namespace PassportPDF.Tools.Framework.Configuration
{
    [Serializable]
    public sealed class ApplicationConfiguration
    {
        public FileProductionRules FileProductionRules { get; private set; }
        public int ThreadCount { get; set; }
        public string Language { get; set; } = GetDefaultLanguage();
        public bool MinimizedWindow { get; set; }
        public bool AutomaticallycheckForUpdates { get; set; }
        public bool WarnWhenSameInputOutputDirectory { get; set; }
        public bool ProcessSubFolders { get; set; }
        public bool OnlyProcessPDF { get; set; }
        public bool TimestampLogs { get; set; }
        public bool ExportLogs { get; set; }
        public string LogsPath { get; set; }
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string LatestVersion { get; set; }
        public string InputFileFormats { get; set; }


        public ApplicationConfiguration()
        {
            Reset();
        }


        public void Reset()
        {
            FileProductionRules = GetDefaultFileProductionRules();
            ThreadCount = FrameworkGlobals.THREAD_NUMBER_DEFAULT;
            MinimizedWindow = FrameworkGlobals.MINIMIZED_WINDOW_DEFAULT;
            WarnWhenSameInputOutputDirectory = FrameworkGlobals.WARN_WHEN_SAME_INPUT_OUTPUT_DIRECTORY_DEFAULT;
            ProcessSubFolders = FrameworkGlobals.PROCESS_SUBFOLDERS_DEFAULT;
            OnlyProcessPDF = FrameworkGlobals.ONLY_PROCESS_PDF_DEFAULT;
            TimestampLogs = FrameworkGlobals.TIMESTAMP_LOGS_DEFAULT;
            ExportLogs = FrameworkGlobals.EXPORT_LOGS_DEFAULT;
            AutomaticallycheckForUpdates = FrameworkGlobals.AUTOMATICALLY_CHECK_FOR_UPDATE_DEFAULT;
            LogsPath = default(string);
        }


        private static string GetDefaultLanguage()
        {
            string lang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;

            if (!FrameworkGlobals.MessagesLocalizer.IsSupportedLanguage(lang))
            {
                lang = FrameworkGlobals.MessagesLocalizer.DefaultLanguage;
            }

            return lang;
        }


        private static FileProductionRules GetDefaultFileProductionRules()
        {
            return new FileProductionRules()
            {
                KeepWriteAndAccessTime = FrameworkGlobals.KEEP_WRITE_AND_ACCESS_TIME_DEFAULT,
                DeleteOriginalFileOnSuccess = FrameworkGlobals.DELETE_ORIGINAL_FILE_ON_SUCCES_DEFAULT
            };
        }
    }
}