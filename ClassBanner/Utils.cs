using System;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;

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
            Dictionary<string, RegistryKey> RegHives = new();
            RegHives.Add("LocalMachine", Registry.LocalMachine);
            RegHives.Add("CurrentUser", Registry.CurrentUser);
            return RegHives;
        }

        public static SolidColorBrush? GetColorBrush(String? color)
        {
            SolidColorBrush brush;
            if (IsValidColor(color))
            {
                var mediaColor = System.Windows.Media.ColorConverter.ConvertFromString(color);
                brush = new SolidColorBrush((System.Windows.Media.Color)mediaColor);
                return brush;
            }
            return null;
        }
        //Determine if color is not null and is a valid color
        private static Boolean IsValidColor(String? color)
        {

            if ((color is not null) || (color != ""))
            {
                try
                {
                    var colorConverts = System.Windows.Media.ColorConverter.ConvertFromString(color);
                    return true;
                }
                catch
                {
                    //Bad color value log to file
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}