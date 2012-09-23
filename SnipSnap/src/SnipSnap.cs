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
            TryGet1();
        }

        static void TryGet1()
        {
            string file = Path.Combine(Environment.CurrentDirectory, "aaaaa.jpg");

            ScreenBitmapGenerator generator = new ScreenBitmapGenerator();
            Image m = generator.GenerateCurrentWindowBitmap();
            m.Save(file, ImageFormat.Jpeg);
        }
    }
}

