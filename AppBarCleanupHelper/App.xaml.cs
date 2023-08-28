using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Media3D;
using static AppBarCleanupHelper.App;

namespace AppBarCleanupHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            string uniqueIdentifier = "DesktopBanner_AppBarMessage_ECDFB5206FC2"; // Use the same unique identifier
            string keyPath = "Software\\DesktopBanner\\ABRegistrations"; // Replace with your actual key path

            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(keyPath)) {
                if (key != null)
                {
                    // Enumerate through the value names
                    foreach (string valueName in key.GetValueNames())
                    {
                        
                        uint AppBarId = uint.Parse(valueName.Split('_')[0]);
                        // Get the value data
                        string windowHandleString = key.GetValue(valueName) as string;
                        //string windowHandleString = key.GetValue(uniqueIdentifier) as string;
                        if (!string.IsNullOrEmpty(windowHandleString) && IntPtr.TryParse(windowHandleString, out IntPtr appBarWindowHandle))
                        {
                            // Perform cleanup using appBarWindowHandle
                            UnregisterAppBar(appBarWindowHandle, AppBarId);
                        }

                    }
                }
            }
        }


        private const uint ABM_REMOVE = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern uint SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);
        private static void UnregisterAppBar(IntPtr appBarWindowHandle, uint AppBarId)
        {
            APPBARDATA appBarData = new APPBARDATA
            {
                uCallbackMessage = AppBarId,
                cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA)),
                hWnd = appBarWindowHandle
            };
           
            


        SHAppBarMessage(ABM_REMOVE, ref appBarData);
            var a1 = 5;
        }



        // Helper Program (During Cleanup)
    }

}
