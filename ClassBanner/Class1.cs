using System;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace ClassBanner
{
    public class Reg
    {
        static public Int32? GetInt(String Path, String Property)
        {
            Path = ExpandPath(Path);
            Int32? value = (Int32?) Registry.GetValue(Path,Property,-1);
            return value;
        }

        static public String? GetString(String Path, String Property)
        {
            Path = ExpandPath(Path);
            String? value = (string?)Registry.GetValue(Path, Property, "");
            return value;
        }
        static public Boolean PropertyExists(String Path, String Property) 
        {
            Path = ExpandPath(Path);
            var value = Registry.GetValue(Path, Property, false);
            if ((null != value) || ((Boolean)value != false)) 
            {
                return true;
            }
            return false;
        }
        static private String ExpandPath(String Path) 
        {
            //Expand HKLM,HKCU,HKCR in registry path to equivalent long-form
            String hive = @"(HKLM:|HKLM|HKCU:|HKCU|HKCR:|HKCR)(?:.+)";
            String? newPath;
            Match hiveNameResult = Regex.Match(Path, hive);
            if (hiveNameResult.Success)
            {
                String hiveName = hiveNameResult.Groups[1].Value;
                switch (hiveName)
                {
                    case "HKCU:":
                    case "HKCU":
                        newPath = Path.Replace(hiveName, "HKEY_CURRENT_USER");
                        break;
                    case "HKCR:":
                    case "HKCR":
                        newPath = Path.Replace(hiveName, "HKEY_CLASSES_ROOT");
                        break;
                    case "HKLM:":
                    case "HKLM":
                        newPath = Path.Replace(hiveName, "HKEY_LOCAL_MACHINE");
                        break;
                    default:
                        newPath = Path;
                        break;
                }
                return newPath;
            }
            else 
            {
                return Path;
            }
        }
        static  public void NewKey(String Path, String KeyName) 
        {
        
        }
        static public void SetProperty(String Path,String Property, String Value)
        {

        }
        static public void SetProperty(String Path, String Property, Int32 Value)
        {

        }
        static public void DeleteKey(String Path)
        {

        }
        static public void DeleteProperty()
        {

        }
    }
}
