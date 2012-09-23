using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Net;

namespace SnipSnap
{
    public class SnipSnap
    {
        static void TestDequeueSave()
        {
            string file;

            for (int i = 0; i < 15; i++)
            {
                file = Path.Combine(Environment.CurrentDirectory, "aaaa" + i + ".jpg");
                ThreadMsgQueue<Image>.Dequeue().Save(file, ImageFormat.Jpeg);
            }
        }

        static void Main(string[] args)
        {
            KeyboardHook kb = new KeyboardHook();
            kb.Hook();
            //TestDequeueSave();          
        }

        static void CheckBase64()
        {
            ScreenImageGenerator generator = new ScreenImageGenerator();
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

            ScreenImageGenerator generator = new ScreenImageGenerator();
            Image m = generator.GetFocusedWindowImage();

            m.Save(file, ImageFormat.Jpeg);
        }
    }
}

