using System;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Security.Principal;
//using System.Diagnostics;

namespace ClassBanner
{
    public class Utils
    {


        static public Dictionary<string, RegistryKey> GetRegHives()
        {
            Dictionary<string, RegistryKey> RegHives = new Dictionary<string, RegistryKey>();
            RegHives.Add("LocalMachine", Registry.LocalMachine);
            RegHives.Add("CurrentUser", Registry.CurrentUser);
            return RegHives;
        }

        static public string SelectHive(string path)
        {
            Regex regexMatchHive = new Regex(@"(\w+)(?=\\)");
            string hiveName;
            Match matchHive = regexMatchHive.Match(path);
            string matchedHive = matchHive.Value;

            switch (matchedHive)
            {
                case "HKCU":
                    goto case "HKEY_CURRENT_USER";
                case "HKEY_CURRENT_USER":
                    hiveName = "CurrentUser";
                    break;
                case "HKLM":
                    goto case "HKEY_LOCAL_MACHINE";
                case "HKEY_LOCAL_MACHINE":
                    hiveName = "LocalMachine";
                    break;
                default:
                    hiveName = "LocalMachine";
                    break;
            }
            return hiveName;
        }

        static public string FormatRegPath(string path)
        {
            Regex regexMatchHive = new Regex(@"(\w+)(?=\\)");
            Match matchHive = regexMatchHive.Match(path);
            string matchedHive = matchHive.Value;
            string subKeyPath = path.Replace(matchHive.Value + @"\", "");
            return subKeyPath;
        }

        static public RegistryKey GetRegKey(string path)
        {
            Dictionary<string, RegistryKey> RegHives = GetRegHives();
            //    return Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Peernet");
            string hive = SelectHive(path);
            string regPath = FormatRegPath(path);
            RegistryKey selectedValue = RegHives[hive].OpenSubKey(regPath);
            return selectedValue;
            //return RegHives["LocalMachine"].OpenSubKey((@"SOFTWARE\Policies\Microsoft\Peernet");
        }


        static public bool TestRegPath(string path, string entry = null)
        {
            bool assertKeyExists;
            try
            {
                RegistryKey testKey = GetRegKey(path);
                if (entry != null)
                {
                    assertKeyExists = (testKey.GetValue(entry, null) != null);
                }
                else
                {
                    assertKeyExists = (testKey != null);
                }
                if (null == testKey) { return false; }
                testKey.Close();
            }
            catch
            {
                return false;
            }

            return assertKeyExists;
        }

        static public string GetRegValue(string path, string entry)
        {
            RegistryKey regKey = GetRegKey(path);
            return (string)regKey.GetValue(entry);
        }

        static public void NewRegKey(string path)
        {
            Dictionary<string, RegistryKey> RegHives = GetRegHives();
            string formattedPath = FormatRegPath(path);
            string selectedHive = SelectHive(path);
            try
            {
                RegistryKey newKey = RegHives[selectedHive].CreateSubKey(formattedPath);
                newKey.Close();

            }
            catch { }
        }
        static public void NewRegValue(string path, string name, string value)
        {
            Dictionary<string, RegistryKey> RegHives = GetRegHives();
            string formattedPath = FormatRegPath(path);
            string selectedHive = SelectHive(path);
            try
            {
                RegHives[selectedHive].OpenSubKey(formattedPath, true).SetValue(name, value);
            }
            catch { }
        }

        /*    bool test3 = TestRegPath(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Peernet", "Disabled");
            Console.WriteLine(test + " " + (test3 ? "yes" : "no"));
            NewRegKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\SDL-RM-Mgmt");

            NewRegValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\SDL-RM-Mgmt", "DTAGroupName", "dta");
        */

    }
}