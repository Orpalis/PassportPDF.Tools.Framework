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
using PassportPDF.Tools.Framework.Models;
using PassportPDF.Tools.Framework.Utilities;

namespace PassportPDF.Tools.Framework.Business
{
    public static class FileToProcessCollector
    {
        public static CollectionOperationResult ProceedToFileCollection(string sourceInput, string destinationFolder)
        {
            if (!IsSourceInputValid(sourceInput))
            {
                return GetOperationsPreparationResult(OperationsPreparationResultType.UnfullfiledWithError, FrameworkGlobals.MessagesLocalizer.GetString("noSourceFilesSpecified", FrameworkGlobals.ApplicationLanguage));
            }

            if (!ParseSourceInput(sourceInput, out string[] data, out bool inputIsFolder))
            {
                return GetOperationsPreparationResult(OperationsPreparationResultType.UnfullfiledWithError, FrameworkGlobals.MessagesLocalizer.GetString("invalidSourceFile", FrameworkGlobals.ApplicationLanguage));
            }

            return ProceedToFileCollection(data, destinationFolder, inputIsFolder);
        }


        public static CollectionOperationResult ProceedToFileCollection(string[] data, string destinationFolder, bool inputIsFolder)
        {
            List<FileToProcess> collectedFiles;

            destinationFolder = ParsingUtils.EnsureFolderPathEndsWithBackSlash(destinationFolder);

            if (!IsDestinationFolderValid(destinationFolder))
            {
                return GetOperationsPreparationResult(OperationsPreparationResultType.UnfullfiledWithError, FrameworkGlobals.MessagesLocalizer.GetString("outputFolderNoExists", FrameworkGlobals.ApplicationLanguage));
            }

            try
            {
                collectedFiles = FileUtils.CollectFiles(data, FrameworkGlobals.ApplicationConfiguration.ProcessSubFolders, inputIsFolder, ParsingUtils.ParseInputFileExtensions(FrameworkGlobals.ApplicationConfiguration.InputFileFormats, FrameworkGlobals.PassportPDFConfiguration.SupportedFormatsExtensions, FrameworkGlobals.ApplicationConfiguration.OnlyProcessPDF));
            }
            catch (Exception exception)
            {
                return GetOperationsPreparationResult(OperationsPreparationResultType.UnfullfiledWithError, FrameworkGlobals.MessagesLocalizer.GetString("errorBuildingList", FrameworkGlobals.ApplicationLanguage) + " " + exception.Message);
            }

            if (collectedFiles.Count == 0)
            {
                return GetOperationsPreparationResult(OperationsPreparationResultType.Unfullfilled, FrameworkGlobals.MessagesLocalizer.GetString("noFile", FrameworkGlobals.ApplicationLanguage));
            }


            if (MustInputAndOutputDirectoryWarningBeNotified(data, destinationFolder))
            {
                return GetOperationsPreparationResult(OperationsPreparationResultType.SuccessWithWarning, FrameworkGlobals.MessagesLocalizer.GetString("sameInputAndOutputFolder", FrameworkGlobals.ApplicationLanguage), collectedFiles);
            }

            return GetOperationsPreparationResult(collectedFiles);
        }


        private static bool ParseSourceInput(string sourceInput, out string[] data, out bool inputIsFolder)
        {

            inputIsFolder = Directory.Exists(sourceInput);
            data = sourceInput.Split('|');

            if (data.Length == 1)
            {
                if (!inputIsFolder && !(File.Exists(sourceInput)))
                {
                    // No valid input file/folder
                    return false;
                }
            }

            if (inputIsFolder)
            {
                data[0] = ParsingUtils.EnsureFolderPathEndsWithBackSlash(data[0]);
            }

            return true;
        }


        private static bool IsSourceInputValid(string sourceInput)
        {
            return !string.IsNullOrEmpty(sourceInput);
        }


        private static bool IsDestinationFolderValid(string destinationFolder)
        {
            return Directory.Exists(destinationFolder);
        }


        private static bool MustInputAndOutputDirectoryWarningBeNotified(string[] data, string destinationFolder)
        {
            return FrameworkGlobals.ApplicationConfiguration.WarnWhenSameInputOutputDirectory && ParsingUtils.IsAnyInputDirectorySameAsOutputDirectory(data, destinationFolder);
        }


        private static CollectionOperationResult GetOperationsPreparationResult(List<FileToProcess> collectedFiles)
        {
            return new CollectionOperationResult(OperationsPreparationResultType.Success, null, collectedFiles);
        }


        private static CollectionOperationResult GetOperationsPreparationResult(OperationsPreparationResultType resultType, string additionalMessage = null, List<FileToProcess> collectedFiles = null)
        {
            return new CollectionOperationResult(resultType, additionalMessage, collectedFiles);
        }


        public sealed class CollectionOperationResult
        {
            public OperationsPreparationResultType ResultType { get; }
            public string ResultMessage { get; }
            public List<FileToProcess> CollectedFiles { get; }

            public CollectionOperationResult(OperationsPreparationResultType resultType, string resultMessage, List<FileToProcess> collectedFiles)
            {
                ResultType = resultType;
                ResultMessage = resultMessage;
                CollectedFiles = collectedFiles;
            }
        }


        public enum OperationsPreparationResultType
        {
            Success,
            SuccessWithWarning,
            Unfullfilled,
            UnfullfiledWithError
        }
    }
}
