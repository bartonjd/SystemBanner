using System;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;

namespace DesktopBanner
{
    public class Utils
    {
        static public string GetCurrentUser() { 
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').ToArray().Last();
        }

        static public string GetHostName() {
            return Environment.MachineName;
        }

        static public Dictionary<string, RegistryKey> GetRegHives()
        {
            Dictionary<string, RegistryKey> RegHives = new Dictionary<string, RegistryKey>();
            RegHives.Add("LocalMachine", Registry.LocalMachine);
            RegHives.Add("CurrentUser", Registry.CurrentUser);
            return RegHives;
        }
    }
}