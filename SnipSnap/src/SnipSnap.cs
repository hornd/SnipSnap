using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SnipSnap
{
    public class SnipSnap
    {
        [STAThread]
        static void Main(string[] args)
        {
            KeyboardHook kb = new KeyboardHook();
            kb.Hook();

            ImgurUploader upper = new ImgurUploader();
            while (true)
            {
                Image img = ThreadMsgQueue<Image>.Dequeue();
                Uri res = upper.UploadImage(img, ImageFormat.Jpeg);
                System.Windows.Forms.Clipboard.SetText(res.AbsoluteUri);
            }
        }
    }
}

