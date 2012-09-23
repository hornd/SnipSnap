using System;
using System.Drawing;

namespace SnipSnap
{
    public abstract class ImageGenerator
    {
        public ImageGenerator() { }


        protected virtual Image GetBitmapFromHandle(IntPtr handle)
        {
            Image ret;

            Win32ApiWrapper.Rect winRect = new Win32ApiWrapper.Rect();
            Win32ApiWrapper.GetWindowRect(handle, ref winRect);

            IntPtr hdcSrc = Win32ApiWrapper.GetWindowDC(handle);
            IntPtr hdcDest = Win32ApiWrapper.CreateCompatibleDC(hdcSrc);

            IntPtr hBitmap = Win32ApiWrapper.CreateCompatibleBitmap(hdcSrc, winRect.GetWidth(), winRect.GetHeight());
            IntPtr hOld = Win32ApiWrapper.SelectObject(hdcDest, hBitmap);

            Win32ApiWrapper.BitBlt(hdcDest, 0, 0, winRect.GetWidth(), winRect.GetHeight(), hdcSrc, 0, 0, Win32ApiWrapper.RasterOptions.SRC_COPY);

            Win32ApiWrapper.SelectObject(hdcDest, hOld);

            Win32ApiWrapper.DeleteDC(hdcDest);
            Win32ApiWrapper.ReleaseDC(handle, hdcSrc);

            ret = Image.FromHbitmap(hBitmap);

            Win32ApiWrapper.DeleteObject(hBitmap);

            return ret;
        }
    }

    public class ScreenImageGenerator : ImageGenerator
    {
        public ScreenImageGenerator() { }

        public Image GetScreenImage()
        {
            return GetBitmapFromHandle(Win32ApiWrapper.GetDesktopWindow());
        }

        public Image GetFocusedWindowImage()
        {
            return GetBitmapFromHandle(Win32ApiWrapper.GetForegroundWindow());
        }
    }
}
