using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SnipSnap
{
    public class SnipSnap
    {
        static void Main(string[] args)
        {
            CheckBase64();
        }

        static void CheckBase64()
        {
            ScreenBitmapGenerator generator = new ScreenBitmapGenerator();
            Image m, n;
            string file, base64;

            file = Path.Combine(Environment.CurrentDirectory, "aaaaa.jpg");
            m = generator.GetFocusedWindowImage();
            base64 = ImageBase64Coder.Encode(m, ImageFormat.Jpeg);
            n = ImageBase64Coder.Decode(base64);

            Console.WriteLine("Base 64: " + base64);
            n.Save(file, ImageFormat.Jpeg);
        }

        static void TryGet1()
        {
            string file = Path.Combine(Environment.CurrentDirectory, "aaaaa.jpg");

            ScreenBitmapGenerator generator = new ScreenBitmapGenerator();
            Image m = generator.GetFocusedWindowImage();

            m.Save(file, ImageFormat.Jpeg);
        }
    }
}

