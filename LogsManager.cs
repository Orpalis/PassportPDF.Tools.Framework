/**********************************************************************
 * Project:                 PassportPDF.Tools.Framework
 * Authors:                 - Evan Carr�re.
 *                          - Lo�c Carr�re.
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

namespace PassportPDF.Tools.Framework
{
    public sealed class LogsManager
    {
        private readonly object LogMutex = new object();

        private string _logFilePath;

        public Exception Error { get; private set; }


        public void Reset(string logFilePath)
        {
            _logFilePath = logFilePath;
            Error = null;
        }


        public void LogMessage(string message)
        {
            if (Error == null)
            {
                try
                {
                    lock (LogMutex)
                    {
                        File.AppendAllText(_logFilePath, message + Environment.NewLine);
                    }
                }
                catch (Exception exception)
                {
                    Error = exception;
                }
            }
        }


        public void LogMessages(IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                LogMessage(message);
            }
        }
    }
}