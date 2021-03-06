﻿using System;
using System.Runtime.InteropServices;

namespace SnipSnap
{
    // Provides access to some WINAPI functionality.
    public class Win32ApiWrapper
    {
        public enum RasterOptions : int
        {
            SRC_COPY = 0x00CC0020,
        }

        public enum HookType : int
        {
            WH_KEYBOARD = 2,
            WH_KEYBOARD_LL = 13
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left, top, right, bottom;

            public int GetWidth() { return this.right - this.left; }

            public int GetHeight() { return this.bottom - this.top; }
        }

        public delegate IntPtr HookIn(int nCode, IntPtr wParam, IntPtr lParam);
             
        // Get the rectangle associated witht he window hWnd.
        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        // Get a handle to the current foreground window
        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();

        // Query the API for a handle to the desktop window
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        // Retrieve the device context for the referenced pointer.
        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        // Release DC hDc belonging to window hWnd.
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        // Delete the DC hDc
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hDc);

        // Create a new DC compatible with DC hDc.
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        // Release the object hObject
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        // Create a new bitmap of size nWidth x nHeight compatible with the DC hdc
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        // Selects the object hgdiobj into the DC hdc
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        // Transfers pixels from the retangular area bounded by nWidth, nHeight of nXSrc, nYSrc of hdcSrc
        // into the pixels bounded by nWidth, nHeight of nXDest, nYDest of hdcDest
        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, RasterOptions dwRop);

        // Install thread-specific hook for trapping keyboard interrupts.
        [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx")]
        public static extern IntPtr SetWindowsHookEx(HookType idHook, HookIn lpfn, IntPtr hMod, int dwThreadId);
        
        // Remove the hook.
        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx")]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        // If we dont need to act, pass hook to next process/thread.
        [DllImport("user32.dll", EntryPoint = "CallNextHookEx")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        // Returns the state of vKey when function is called.
        [DllImport("user32.dll", EntryPoint = "GetAsyncKeyState")]
        public static extern Int16 GetAsyncKeyState(int vKey);

        // Returns handle to module lpModuleName
        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
