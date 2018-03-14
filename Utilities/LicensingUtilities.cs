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

using Microsoft.Win32;

namespace PassportPDF.Tools.Framework.Utilities
{
    //todo: implement registry access mechanism aware of the executing platform (so far assuming the platform is Windows)
    public static class LicensingUtilities
    {
        private const string LICENSE_KEY_ENTRY = "PassportKey";


        public static bool IsPassportKeyRegistered(string productName)
        {
            return GetRegisterPassportId(productName) != "";
        }


        public static string GetRegisterPassportId(string productName)
        {
            RegistryKey rk = null;
            try
            {
                rk = Registry.CurrentUser.OpenSubKey(GetRegistryKey(productName, false));
            }
            catch { }

            if (rk == null)
            {
                try
                {
                    rk = Registry.CurrentUser.OpenSubKey(GetRegistryKey(productName, true));
                }
                catch { }
            }

            if (rk == null)
            {
                try
                {
                    rk = Registry.LocalMachine.OpenSubKey(GetRegistryKey(productName, false));
                }
                catch { }
            }

            if (rk == null)
            {
                try
                {
                    rk = Registry.LocalMachine.OpenSubKey(GetRegistryKey(productName, true));
                }
                catch { }
            }

            if (rk != null)
            {
                return rk.GetValue(LICENSE_KEY_ENTRY).ToString();
            }
            else
            {
                return "";
            }
        }


        public static bool RegisterPassportIdInRegister(string appID, string passportKey)
        {/*
            return (RegisterPassportKeyInRegistry(Registry.CurrentUser, GetRegistryKey(productName, false), passportKey) |
                               RegisterPassportKeyInRegistry(Registry.CurrentUser, GetRegistryKey(productName, true), passportKey) |
                               RegisterPassportKeyInRegistry(Registry.LocalMachine, GetRegistryKey(productName, false), passportKey) |
                               RegisterPassportKeyInRegistry(Registry.LocalMachine, GetRegistryKey(productName, true), passportKey));
                               */
            return (RegisterPassportKeyInRegistry(Registry.CurrentUser, GetRegistryKey(appID, false), passportKey) |
           RegisterPassportKeyInRegistry(Registry.CurrentUser, GetRegistryKey(appID, true), passportKey));
        }


        private static bool RegisterPassportKeyInRegistry(RegistryKey registry, string subKey, string passportKey)
        {
            try
            {
                registry.DeleteSubKey(subKey, false);
                RegistryKey registrySubKey = registry.CreateSubKey(subKey);
                registrySubKey.SetValue(LICENSE_KEY_ENTRY, passportKey, RegistryValueKind.String);
                registrySubKey.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


        private static string GetRegistryKey(string productName, bool Wow6432Node)
        {
            string rootNode = Wow6432Node ? "Software\\Wow6432Node" : "Software";
            return rootNode + "\\Orpalis\\" + productName;
        }
    }
}
