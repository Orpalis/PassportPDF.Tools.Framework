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
using System.IO;
using Newtonsoft.Json;

namespace PassportPDF.Tools.Framework.Configuration
{
    public static class ConfigurationManager
    {
        public static object InitializeConfigurationInstanceEx(string configurationFileName, Type configurationType)
        {
            object configurationInstance;

            if (File.Exists(configurationFileName))
            {
                if (configurationType == typeof(ApplicationConfiguration))
                {
                    configurationInstance = DeserializeApplicationConfigurationData(configurationFileName);
                }
                else if (configurationType == typeof(PDFReduceActionConfiguration))
                {
                    configurationInstance = JsonConvert.DeserializeObject<PDFReduceActionConfiguration>(File.ReadAllText(configurationFileName));
                }
                else if (configurationType == typeof(PDFOCRActionConfiguration))
                {
                    configurationInstance = JsonConvert.DeserializeObject<PDFOCRActionConfiguration>(File.ReadAllText(configurationFileName));
                }
                else //if (configurationType == typeof(ImageSaveAsPDFActionConfiguration))
                {
                    configurationInstance = JsonConvert.DeserializeObject<ImageSaveAsPDFMRCActionConfiguration>(File.ReadAllText(configurationFileName));
                }

                if (configurationInstance == null)
                {
                    configurationInstance = ResetDefaultConfiguration(configurationType);
                }
            }
            else
            {
                configurationInstance = ResetDefaultConfiguration(configurationType);
            }

            return configurationInstance;
        }


        public static bool SaveConfiguration(string configurationFileName, object configurationInstance)
        {
            try
            {
                string pathRoot = Path.GetDirectoryName(configurationFileName);

                if (Directory.Exists(pathRoot))
                {
                    if (File.Exists(configurationFileName))
                    {
                        File.Delete(configurationFileName);
                    }
                }
                else
                {
                    Directory.CreateDirectory(pathRoot);
                }


                using (StreamWriter fileStream = File.CreateText(configurationFileName))
                {
                    new JsonSerializer().Serialize(fileStream, configurationInstance);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }


        public static ApplicationConfiguration ResetDefaultApplicationConfiguration()
        {
            return new ApplicationConfiguration();
        }


        public static PDFReduceActionConfiguration ResetDefaultPDFReduceActionConfiguration()
        {
            return new PDFReduceActionConfiguration();
        }


        public static PDFOCRActionConfiguration ResetDefaultPDFOCRActionConfiguration()
        {
            return new PDFOCRActionConfiguration();
        }


        public static ImageSaveAsPDFMRCActionConfiguration ResetDefaultImageSaveAsPDFMRCActionConfiguration()
        {
            return new ImageSaveAsPDFMRCActionConfiguration();
        }


        private static object ResetDefaultConfiguration(Type configurationType)
        {
            if (configurationType == typeof(ApplicationConfiguration))
            {
                return ResetDefaultApplicationConfiguration();
            }
            else if (configurationType == typeof(PDFReduceActionConfiguration))
            {
                return ResetDefaultPDFReduceActionConfiguration();
            }
            else if (configurationType == typeof(PDFOCRActionConfiguration))
            {
                return ResetDefaultPDFOCRActionConfiguration();
            }
            else //if (configurationType == typeof(ImageSaveAsPDFActionConfiguration))
            {
                return ResetDefaultImageSaveAsPDFMRCActionConfiguration();
            }
        }


        private static ApplicationConfiguration DeserializeApplicationConfigurationData(string fileName)
        {
            ApplicationConfiguration applicationConfiguration = JsonConvert.DeserializeObject<ApplicationConfiguration>(File.ReadAllText(fileName));

            if (applicationConfiguration != null)
            {
                applicationConfiguration.ThreadCount = Math.Max(1, applicationConfiguration.ThreadCount); //Ensure ThreadCount is always valid.
            }

            return applicationConfiguration;
        }
    }
}