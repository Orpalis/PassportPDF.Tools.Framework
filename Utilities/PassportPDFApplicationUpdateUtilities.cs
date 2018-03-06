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
using System.Net;
using System.IO;
using System.ComponentModel;
using System.IO.Compression;
using System.Diagnostics;
using PassportPDF.Api;

namespace PassportPDF.Tools.Framework.Utilities
{
    public static class PassportPDFApplicationUpdateUtilities
    {
        public static bool? IsNewVersionAvailable(string applicationId, Version currentVersion, out string latestVersionNumber)
        {
            try
            {
                PassportPDFApplicationManagerApi applicationManagerApi = new PassportPDFApplicationManagerApi();
                latestVersionNumber = applicationManagerApi.GetApplicationLatestVersion(applicationId).Value;
                return currentVersion.CompareTo(new Version(latestVersionNumber)) < 0;
            }
            catch
            {
                latestVersionNumber = null;
                return null;
            }
        }


        /// <summary>
        /// Determines whether the current version of a PassportPDF app is supported, using its Application ID.
        /// </summary>
        /// <param name="applicationId">The ID representing the PassportPDF app.</param>
        /// <param name="currentVersion">The current version of the PassportPDF app.</param>
        /// <returns>Null if the minimum app version failed to be retrieved, and whether the current application version is supported.</returns>
        public static bool? IsCurrentApplicationVersionSupported(string applicationId, Version currentVersion)
        {
            try
            {
                PassportPDFApplicationManagerApi applicationManagerApi = new PassportPDFApplicationManagerApi();
                string minimumSupportedVersion = applicationManagerApi.GetApplicationMinimumSupportedVersion(applicationId).Value;
                return currentVersion.CompareTo(new Version(minimumSupportedVersion)) >= 0;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Downloads the last version of a PassportPDF app, using its application ID.
        /// </summary>
        /// <param name="applicationId">The ID representing the PassportPDF app.</param>
        /// <param name="downloadCompletionEventHandler">The event handler for the download completion.</param>
        /// <param name="downloadProgressEventHandler">The (optional) event handler for the download progress.</param>
        /// <returns>Null if the download failed, the path to the archive containing the last version of the app otherwise.</returns>
        /// <remarks>
        /// The downloaded file should not be accesed before the download completion event handler is called.
        /// </remarks>
        public static string DownloadAppLatestVersion(string applicationId, AsyncCompletedEventHandler downloadCompletionEventHandler, DownloadProgressChangedEventHandler downloadProgressEventHandler = null)
        {
            if (downloadCompletionEventHandler == null)
            {
                throw new ArgumentNullException("downloadCompletionEventHandler");
            }

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    if (downloadProgressEventHandler != null)
                    {
                        webClient.DownloadProgressChanged += downloadProgressEventHandler;
                    }
                    if (downloadCompletionEventHandler != null)
                    {
                        webClient.DownloadFileCompleted += downloadCompletionEventHandler;
                    }
                    string downloadedFilePath = Path.GetTempPath() + Guid.NewGuid() + ".zip";
                    PassportPDFApplicationManagerApi applicationManagerApi = new PassportPDFApplicationManagerApi();

                    string appDownloadLink = applicationManagerApi.GetApplicationDownloadLink(applicationId).Value;

                    webClient.DownloadFileAsync(new Uri(appDownloadLink), downloadedFilePath);

                    return downloadedFilePath;
                }
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Starts the installer of a downloaded PassportPDF app in a new process.
        /// </summary>
        /// <param name="updatedFilePath">The path to the downloaded PassportPDF app.</param>
        /// <param name="appExecutableName">The name of the app.</param>
        public static void StartUpdatedAppInstallation(string updatedFilePath, string appExecutableName)
        {
            string extractionDirectory = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(updatedFilePath));

            Directory.CreateDirectory(extractionDirectory);

            ZipFile.ExtractToDirectory(updatedFilePath, extractionDirectory);
            Process.Start(Path.Combine(extractionDirectory, appExecutableName));
        }
    }
}
