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
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace PassportPDF.Tools.Framework.Utilities
{
    public static class ParsingUtils
    {
        public static string EnsureFolderPathEndsWithBackSlash(string folderPath)
        {
            if (!folderPath.EndsWith("\\"))
            {
                folderPath += "\\";
            }
            return folderPath;
        }


        public static string[] ParseInputFileExtensions(string input, string[] supportedFormatsFileExtensions, bool onlyProcessPdf)
        {
            if (onlyProcessPdf)
            {
                return new[] { "PDF" };
            }
            if (string.IsNullOrWhiteSpace(input) || input == "all")
            {
                return supportedFormatsFileExtensions;
            }

            string[] extensions = (from entry in input.Split('|') where supportedFormatsFileExtensions.Contains(entry.ToUpper()) select entry.ToUpper()).ToArray();

            return extensions.Length != 0 ? extensions : supportedFormatsFileExtensions;
        }


        public static bool IsAnyInputDirectorySameAsOutputDirectory(string[] inputPaths, string destinationFolder)
        {
            List<string> inputDirectories = new List<string>(inputPaths.Length);

            destinationFolder = EnsureFolderPathEndsWithBackSlash(destinationFolder);
            foreach (string path in inputPaths)
            {
                inputDirectories.Add(EnsureFolderPathEndsWithBackSlash(Path.GetDirectoryName(path)));
            }

            return inputDirectories.Contains(destinationFolder, StringComparer.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Function to convert the given bytes to either Kilobyte, Megabyte, or Gigabyte
        /// </summary>
        /// <param name="bytes">Double -> Total bytes to be converted</param>
        /// <param name="type">String -> Type of conversion to perform</param>
        /// <returns>Int32 -> Converted bytes</returns>
        /// <remarks></remarks>
        public static double ConvertSize(double bytes, string type)
        {
            try
            {
                const int CONVERSION_VALUE = 1024;
                //determine what conversion they want
                switch (type)
                {
                    case "BY":
                        //convert to bytes (default)
                        return bytes;
                    case "KB":
                        //convert to kilobytes
                        return (bytes / CONVERSION_VALUE);
                    case "MB":
                        //convert to megabytes
                        return Math.Round((bytes / Math.Pow(CONVERSION_VALUE, 2)), 2);
                    case "GB":
                        //convert to gigabytes
                        return (bytes / Math.Pow(CONVERSION_VALUE, 3));
                    default:
                        //default
                        return bytes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }


        public static string GetElapsedTimeString(int hours, int minutes, int seconds, int milliseconds)
        {
            return string.Format("{0:00}:{1:00}:{2:00}.{3:00}", hours, minutes, seconds, milliseconds);
        }
    }
}