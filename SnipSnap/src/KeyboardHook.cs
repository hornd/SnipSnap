using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace SnipSnap
{
    public class KeyboardHook
    {
        private Thread hookThread = null;
        IntPtr hook = IntPtr.Zero;

        Win32ApiWrapper.HookIn callback;

        public KeyboardHook() 
        {
            this.callback = new Win32ApiWrapper.HookIn(LowLevelKeyboardHook);
        }

        ~KeyboardHook() { Deregister(); }

        public void Start()
        {
            hookThread = new Thread(Register);
            hookThread.Priority = ThreadPriority.Highest;
            hookThread.Start();
        }

        private IntPtr LowLevelKeyboardHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            WindowsMessage msg = (WindowsMessage)wParam;

            if (nCode >= 0 && msg == WindowsMessage.WM_KEYUP)
            {
                int scanned = Marshal.ReadInt32(lParam);
                Console.WriteLine("Scancode: 0x{0:x4}", scanned);

                // Insert into queue
                ThreadMsgQueue<int>.Enqueue(scanned);
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
            UInt32 vkCode;
            UInt32 scanCode;
            UInt32 flags;
            UInt32 time;
            IntPtr dwExtraInfo;
        }

        private enum WindowsMessage : int
        {
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101
        }
    }
}