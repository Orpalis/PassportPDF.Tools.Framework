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
                    configurationInstance = JsonConvert.DeserializeObject<ApplicationConfiguration>(File.ReadAllText(configurationFileName));
                }
                else if (configurationType == typeof(ReduceActionConfiguration))
                {
                    configurationInstance = JsonConvert.DeserializeObject<ReduceActionConfiguration>(File.ReadAllText(configurationFileName));
                }
                else //if (configurationType == typeof(OCRActionConfiguration))
                {
                    configurationInstance = JsonConvert.DeserializeObject<OCRActionConfiguration>(File.ReadAllText(configurationFileName));
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


        public static ReduceActionConfiguration ResetDefaultReduceActionConfiguration()
        {
            return new ReduceActionConfiguration();
        }


        public static OCRActionConfiguration ResetDefaultOCRActionConfiguration()
        {
            return new OCRActionConfiguration();
        }


        private static object ResetDefaultConfiguration(Type configurationType)
        {
            if (configurationType == typeof(ApplicationConfiguration))
            {
                return ResetDefaultApplicationConfiguration();
            }
            else if (configurationType == typeof(ReduceActionConfiguration))
            {
                return ResetDefaultReduceActionConfiguration();
            }
            else /*if (configurationType == typeof(OCRActionConfiguration))*/
            {
                return ResetDefaultOCRActionConfiguration();
            }
        }
    }
}