using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Drawing;

namespace SnipSnap
{
    public class KeyboardHook
    {
        private Thread hookThread;
        private IntPtr hook;
        private Win32ApiWrapper.HookIn callback;

        private ScreenImageGenerator generator;

        public KeyboardHook() 
        {
            this.hookThread = null;
            this.hook = IntPtr.Zero;
            this.callback = new Win32ApiWrapper.HookIn(LowLevelKeyboardHook);
            this.generator = new ScreenImageGenerator();
        }

        ~KeyboardHook() { Deregister(); }

        public void Hook()
        {
            hookThread = new Thread(Register);
            hookThread.Priority = ThreadPriority.Highest;
            hookThread.Start();
        }

        private IntPtr LowLevelKeyboardHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            WindowsMessage msg = (WindowsMessage)wParam;
            bool cntrlPressed = (Win32ApiWrapper.GetAsyncKeyState((int)VirtualKeyCode.VK_CONTROL) & 0x8000) != 0;

            if (nCode >= 0 && msg == WindowsMessage.WM_KEYUP)
            {
                KBDLLHook d = (KBDLLHook)Marshal.PtrToStructure(lParam, typeof(KBDLLHook));
                if (d.vkCode == (int)VirtualKeyCode.VK_INSERT && cntrlPressed)
                {
                    ThreadMsgQueue<Image>.Enqueue(generator.GetFocusedWindowImage());
                }
            }

            return Win32ApiWrapper.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private void Register()
        {
            Deregister();
            using (Process p = Process.GetCurrentProcess())
            using (ProcessModule curMod = p.MainModule)
            {
                this.hook = Win32ApiWrapper.SetWindowsHookEx(Win32ApiWrapper.HookType.WH_KEYBOARD_LL,
                                                             this.callback,
                                                             Win32ApiWrapper.GetModuleHandle(curMod.ModuleName),
                                                             0); // Thread.CurrentThread.ManagedThreadId);
            }

            System.Windows.Forms.Application.Run();
        }

        private void Deregister()
        {
            IntPtr ret = Interlocked.Exchange(ref this.hook, IntPtr.Zero);
            if (ret != IntPtr.Zero)
            {
                Win32ApiWrapper.UnhookWindowsHookEx(ret);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHook
        {
            public UInt32 vkCode;
            public UInt32 scanCode;
            public UInt32 flags;
            public UInt32 time;
            public IntPtr dwExtraInfo;

            public override string ToString()
            {
                return "vkCode: " + vkCode + "\n" +
                       "scanCode: " + scanCode + "\n" +
                       "flags: " + flags + "\n" +
                       "time: " + time + "\n";
            }
        }

        private enum WindowsMessage : int
        {
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101
        }
    }
}