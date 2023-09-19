using System;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Diagnostics.Eventing.Reader;

namespace DesktopBanner
{
    public class Reg
    {
        static public int? GetInt(string Path, string Property)
        {
            Path = FormatPath(Path);
            int? value = (int?)Registry.GetValue(Path, Property, -1);
            if (value == -1)
            {
                value = null;
            }
            return value;
        }
        static public double GetDouble(string Path, string Property)
        {
            Path = FormatPath(Path);
            var value = Registry.GetValue(Path, Property, -1);
            if (null == value)
            {
                value = -1;
            }
            return double.Parse((string)value.ToString());
        }
        static public string? GetString(string Path, string Property)
        {
            Path = FormatPath(Path);
            var value = Registry.GetValue(Path, Property, "");
            if (value is not null)
            {
                return (string)value;
            }
            else
            {
                return "";
            }
        }
        static public dynamic? GetValue(string Path, string Property)
        {
            Path = FormatPath(Path);
            var value = Registry.GetValue(Path, Property, "");
            return value;
        }
        static public bool PropertyExists(string Path, string Property)
        {
            Path = FormatPath(Path);
            var value = Registry.GetValue(Path, Property, null);

            if (null == value)
            {
                return false;
            }
            return true;
        }
        static public Dictionary<string, dynamic> GetProperties(string Path, string[] properties)
        {
            Dictionary<string, dynamic> results = new();

            foreach (string property in properties)
            {
                var prop = GetValue(Path, property);
                results.Add(property, prop ?? "");
            }

            return results;
        }
        static private string GetHiveName(string Path)
        {
            Path = Path.Replace(":", "");
            //Get the hivename (string before the first \)
            Regex whichHiveRegEx = new(@"(\w+)(?=\\)");
            Match match = whichHiveRegEx.Match(Path);
            string hiveName = "HKEY_LOCAL_MACHINE";
            //TODO: Implement a result class like: https://www.youtube.com/watch?v=a1ye9eGTB98 rather than using a default

            if (match.Success)
            {
                hiveName = match.Groups[1].Value;
                hiveName = hiveName.ToUpper();

                //Take the string and find which registry hive it refers to
                if ((hiveName == "HKEY_LOCAL_MACHINE") || (hiveName == "HKLM"))
                {
                    hiveName = "HKEY_LOCAL_MACHINE";
                }
                if ((hiveName == "HKEY_CURRENT_USER") || (hiveName == "HKCU"))
                {
                    hiveName = "HKEY_CURRENT_USER";
                }
                if ((hiveName == "HKEY_CLASSES_ROOT") || (hiveName == "HKCR"))
                {
                    hiveName = "HKEY_CLASSES_ROOT";
                }
                if ((hiveName == "HKEY_USERS") || (hiveName == "HKU"))
                {
                    hiveName = "HKEY_USERS";
                }
                if ((hiveName == "HKEY_CURRENT_CONFIG") || (hiveName == "HKCC"))
                {
                    hiveName = "HKEY_CURRENT_CONFIG";
                }
                if ((hiveName == "HKEY_PERFORMANCE_DATA") || (hiveName == "HKPD"))
                {
                    hiveName = "HKEY_PERFORMANCE_DATA";
                }
            }
            return hiveName;
        }

        static private RegistryKey GetHive(string Path)
        {
            return GetHiveName(Path) switch
            {
                "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
                "HKEY_CURRENT_USER" => Registry.CurrentUser,
                "HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
                "HKEY_USERS" => Registry.Users,
                "HKEY_CURRENT_CONFIG" => Registry.CurrentConfig,
                "HKEY_PERFORMANCE_DATA" => Registry.PerformanceData,
                _ => Registry.LocalMachine, //If string doesn't match assume HKLM was intended
            };
        }
        static public bool KeyExists(string Path)
        {
            string relativePath = RelativizePath(FormatPath(Path));
            bool keyExists = (null != GetHive(Path).OpenSubKey(relativePath));
            return keyExists;
        }
        static private string RelativizePath(string Path)
        {
            Path = Path.Replace(GetHiveName(Path) + @"\", "");
            return Path;
        }
        static private string FormatPath(string Path)
        {
            //Cleanup path if user used colon, e.g. HKLM:\SOFTWARE becomes HKLM\SOFTWARE
            Path = Path.Replace(":", "");
            //Ensure there are no double \ after concatenation or @ escaping
            Path = Path.Replace(@"\\", @"\");
            string hiveName;

            //Remove trailing slash if there is one
            Regex trailingSlash = new(@"\\$");
            Path = trailingSlash.Replace(Path,"");

            //Get the hivename (string before the first \)
            Regex whichHiveRegEx = new(@"(\w+)(?=\\)");
            Match match = whichHiveRegEx.Match(Path);

            if (match.Success)
            {
                hiveName = match.Groups[1].Value;
                Path = Path.Replace(hiveName, GetHiveName(Path));
            }
            return Path;
        }
        static private string GetParentPath(string Path) {
            string newPath = FormatPath(Path);
            string trailingKey = GetTrailingPath(newPath);
            string parentPath = newPath.Replace(trailingKey, "");
            return parentPath;
        }
        static private string GetTrailingPath(string Path)
        {
            //Remove trailing slash if there is one
            Regex trailingSlash = new(@"\\$");
            Path = trailingSlash.Replace(Path, "");

            Regex pattern = new(@"(?<=\\)(\w+)((?=[\\]{2}$)|(?=$))");
            Match match = pattern.Match(Path);
            //Add more error handling logic here
            return match.Value;
        }

        static public void NewKey(string Path, string KeyName)
        {
            string fullPath = ($"{Path}\\{KeyName}").Replace(@"\\", @"\");
            var hive = GetHive(Path);
            if (!KeyExists(fullPath))
            {
                if (KeyName != "")
                {
                    //Ensure keyname is at the end of the path if it was provided
                    Path = RelativizePath(FormatPath(Path));
                }
                var key = hive.OpenSubKey(Path, true);
                if (key is not null)
                {
                    key.CreateSubKey(KeyName);
                }
                else
                {
                    throw new Exception("No such key");
                }
            }
        }

        // Future Enhancement, flesh out these functions 
        /*
        static public void SetProperty(String Path,String Property, String Value)
        {

        }
        */
        static public RegistryKey? GetRegistryKey(string Path)
        {
            var newPath = FormatPath(Path);
            bool keyExists = KeyExists(newPath);
            RegistryKey? key;
            if (keyExists)
            {
                string relativePath = RelativizePath((string)newPath);
                key = GetHive(Path).OpenSubKey(relativePath, true);
                return key;
            }
            string keyName = GetTrailingPath(newPath);
            string parentPath = GetParentPath(newPath);
            NewKey(parentPath, keyName);
            return GetRegistryKey(newPath);
        }
    
        static public void SetIntProperty(String Path, String Property, Int32 Value)
        {

        }
        static public void SetStringProperty(String Path, String Property, string Value)
        {
            using (RegistryKey? key = GetRegistryKey(Path))
            {
                {
                    if (key != null)
                    {
                        key.SetValue(Property, Value);
                    }
                }
            }
        }
        /*
        static public void DeleteKey(String Path)
        {

        }
        static public void DeleteProperty()
        {

        }
        */
    }
}
