﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KioskChromeHelper
{
    public class ExitApp
    {
        private const UInt32 WM_CLOSE = 0x0010;

        public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [STAThread]
        public static void ExitByName(string name)
        {
            // Find process.
            Process[] p = Process.GetProcessesByName(name);

            if (p.Length > 0)
            {
                foreach (Process proc in p)
                {
                    // Check if main window exists. If the window is minimized to the tray this might be not the case.
                    if (proc.MainWindowHandle == IntPtr.Zero)
                    {
                        // Try closing application by sending WM_CLOSE to all child windows in all threads.
                        foreach (ProcessThread pt in proc.Threads)
                        {
                            EnumThreadWindows((uint)pt.Id, new EnumThreadDelegate(ExitApp.EnumThreadCallback), IntPtr.Zero);
                        }
                    }
                    else
                    {
                        // Try to close main window.
                        if (proc.CloseMainWindow())
                        {
                            // Free resources used by this Process object.
                            proc.Close();
                        }
                    }
                }
            }
        }

        private static bool EnumThreadCallback(IntPtr hWnd, IntPtr lParam)
        {
            // Close the enumerated window.
            PostMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            return true;
        }
    }
}
