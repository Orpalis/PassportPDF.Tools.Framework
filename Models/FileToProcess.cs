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

namespace PassportPDF.Tools.Framework.Models
{
    /// <summary>
    /// Represents a file which has been collected from the file system and has yet to be processed.
    /// </summary>
    public struct FileToProcess
    {
        /// <summary>
        /// Specifies the absolute path of the file.
        /// </summary>
        public readonly string FileAbsolutePath;

        /// <summary>
        /// Specifies the relative path of the file, from the specified input directory.
        /// </summary>
        /// <example>
        /// If the input directory is "C:\Users\john", and the file has been collected in "C:\Users\john\documents", its relative path is "documents\foo.pdf".
        /// </example>
        public readonly string FileRelativePath;


        /// <summary>
        /// Specifies the password required to uncrypt the document, if any.
        /// </summary>
        public string Password; 


        public FileToProcess(string fileAbsolutePath, string fileRelativePath):this(fileAbsolutePath, fileRelativePath, "")
        {

        }


        public FileToProcess(string fileAbsolutePath, string fileRelativePath, string password)
        {
            FileAbsolutePath = fileAbsolutePath;
            FileRelativePath = fileRelativePath;
            Password = password;
        }
    }
}
