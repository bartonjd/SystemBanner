using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Timers;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DesktopBannerCleanupHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public class App
    {
        private static System.Timers.Timer timer;
        public static void Main()
        {

            timer = new System.Timers.Timer(5000);
            timer.AutoReset = true;
            timer.Elapsed += CheckBannerProcess;
            timer.Start();



            Console.ReadLine();
        }

        private static void BannerCleanup()
        {
            string uniqueIdentifier = "DesktopBanner_AppBarMessage_ECDFB5206FC2"; // Use the same unique identifier
            string keyPath = "Software\\DesktopBanner\\ABRegistrations"; // Replace with your actual key path

            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(keyPath))
            {
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



        private static void TimedAction()
        {

            // Stop timer and cleanup
            timer.Stop();
            timer.Dispose(); 

            BannerCleanup();

            Console.WriteLine("Banner process not running!");
            // Cleanup is complete, exit and free up resources
            Environment.Exit(0);

        }
        private static void CheckBannerProcess(object sender, ElapsedEventArgs e)
        {
            if (Process.GetProcessesByName("DesktopBanner").Any())
            {
                // Reset the timer if process is running

                Console.WriteLine("Banner process is running!");
                return;
            }
            else
            {
                TimedAction();
            }
        }

    }

}


