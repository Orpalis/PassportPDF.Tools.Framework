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

namespace PassportPDF.Tools.Framework.Utilities
{
    public static class FileUtils
    {
        public static void SaveFile(byte[] data, string inputFileAbsolutePath, string outputFileAbsolutePath, bool keepOriginalLastAccessTime)
        {
            EnsureDirectoryExists(Path.GetDirectoryName(outputFileAbsolutePath));

            bool outputIsInput = false; //overwrite input file ?
            string bak_outFile = outputFileAbsolutePath;

            if (File.Exists(outputFileAbsolutePath))
            {
                outputIsInput = AreSamePath(inputFileAbsolutePath, outputFileAbsolutePath);
                if (outputIsInput)
                {
                    outputFileAbsolutePath = Path.GetTempFileName();
                }
                else
                {
                    DeleteFileEx(outputFileAbsolutePath);
                }
            }

            // Save file to the destination folder
            File.WriteAllBytes(outputFileAbsolutePath, data);

            File.SetCreationTime(outputFileAbsolutePath, File.GetCreationTime(inputFileAbsolutePath));
            if (keepOriginalLastAccessTime)
            {
                SetOriginalLastAccessTime(inputFileAbsolutePath, outputFileAbsolutePath);
            }

            if (outputIsInput)
            {
                //we have to move outFile to bak_outFile 
                DeleteFileEx(bak_outFile);
                File.Move(outputFileAbsolutePath, bak_outFile);
            }
        }


        public static void SetOriginalLastAccessTime(string inputFileName, string outputFileName)
        {
            File.SetLastAccessTime(outputFileName, File.GetLastAccessTime(inputFileName));
            File.SetLastWriteTime(outputFileName, File.GetLastWriteTime(inputFileName));
        }


        public static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }


        public static bool AreSamePath(string path1, string path2)
        {
            FileInfo fileInfo1 = new FileInfo(path1);
            FileInfo fileInfo2 = new FileInfo(path2);

            if (!fileInfo1.Exists || !fileInfo2.Exists)
            {
                return false;
            }

            return fileInfo1.FullName.ToUpper() == fileInfo2.FullName.ToUpper();
        }


        /// <summary>
        /// Remove a file. Can throw exception.
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFileEx(string path)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.IsReadOnly)
            {
                fileInfo.IsReadOnly = false;
            }

            fileInfo.Delete();
        }


        public static long GetFileSize(string fileName)
        {
            return new FileInfo(fileName).Length;
        }


        public static List<FileToProcess> CollectFiles(string[] data, bool processSubFolders, bool singlePath, string[] supportedExtensions)
        {
            List<FileToProcess> collectedFiles = new List<FileToProcess>();

            if (!singlePath)
            {
                foreach (string inputPath in data)
                {
                    if (File.Exists(inputPath))
                    {
                        string fileExtension = Path.GetExtension(inputPath)?.Replace(".", "");

                        if (IsOneOf(supportedExtensions, fileExtension))
                        {
                            collectedFiles.Add(new FileToProcess(inputPath, GetOutputFileName(inputPath)));
                        }
                    }
                    else
                    {
                        if (processSubFolders)
                        {
                            DirSearch(collectedFiles, inputPath, Path.GetFileName(inputPath), true,
                                supportedExtensions);
                        }
                    }
                }
            }
            else
            {
                DirSearch(collectedFiles, data[0], "", processSubFolders, supportedExtensions);
            }


            return collectedFiles;
        }


        private static void DirSearch(List<FileToProcess> collectedFiles, string dir, string destDir, bool processSubFolders, string[] supportedExtensions)
        {
            try
            {
                if (processSubFolders)
                {
                    foreach (string directory in Directory.GetDirectories(dir))
                    {
                        DirSearch(collectedFiles, directory, destDir + "\\" + Path.GetFileName(directory), true, supportedExtensions);
                    }
                }
                foreach (string fileName in Directory.GetFiles(dir))
                {
                    string fileExtension = Path.GetExtension(fileName)?.Replace(".", "");

                    if (IsOneOf(supportedExtensions, fileExtension))
                    {
                        collectedFiles.Add(new FileToProcess(fileName, destDir + "\\" + GetOutputFileName(fileName)));
                    }
                }
            }
            catch (Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }


        private static string GetOutputFileName(string inputFilePath)
        {
            string outputFileName = Path.GetFileName(inputFilePath);

            if (Path.GetExtension(outputFileName) != "pdf")
            {
                outputFileName = Path.ChangeExtension(outputFileName, "pdf");
            }

            return outputFileName;
        }


        private static bool IsOneOf(string[] data, string candidate)
        {
            foreach (string entry in data)
            {
                if (string.CompareOrdinal(candidate.ToUpper(), entry) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}