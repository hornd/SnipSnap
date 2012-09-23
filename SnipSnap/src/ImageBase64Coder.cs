using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SnipSnap
{
    // Methods to encode images to base64 and decode from base64
    public static class ImageBase64Coder
    {
        public static string Encode(Image imageToBase64)
        {
            return Encode(imageToBase64, imageToBase64.RawFormat);
        }

        public static string Encode(Image imageToBase64, ImageFormat format)
        {
            using (MemoryStream st = new MemoryStream())
            {
                imageToBase64.Save(st, GetImageFormat(imageToBase64));
                return Convert.ToBase64String(st.ToArray());
            }
        }

        public static Image Decode(string base64ToImage)
        {
            MemoryStream st = new MemoryStream(Convert.FromBase64String(base64ToImage));
            return Image.FromStream(st);
        }

        // TODO: Debug   image.Save(stream, image.RawForamt) throwing NullValueException
        private static ImageFormat GetImageFormat(Image image)
        {
            ImageFormat ret = ImageFormat.Jpeg;

            Dictionary<string, ImageFormat> formatLookup = new Dictionary<string, ImageFormat>()
            {
               { "BMP",  ImageFormat.Bmp  },
               { "JPEG", ImageFormat.Jpeg },
               { "GIF",  ImageFormat.Gif  },
               { "TIFF", ImageFormat.Tiff },
               { "PNG",  ImageFormat.Png  }
            };

            Guid imageGuid = image.RawFormat.Guid;
            foreach (ImageCodecInfo encoder in ImageCodecInfo.GetImageEncoders())
            {
                if (encoder.FormatID == imageGuid)
                {
                    try { ret = formatLookup[encoder.FormatDescription]; }
                    catch { }
                }
            }

            return ret;
        }
    }
}