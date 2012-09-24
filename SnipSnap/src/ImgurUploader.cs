using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
using System.Xml;

namespace SnipSnap
{
    public class ImgurUploader
    {
        private CookieCollection cookies = new CookieCollection();
        private const string baseUri     = "http://www.imgur.com";
        private const string uploadUri   = "http://api.imgur.com/2/upload";

        private const string IMGUR_ANONYMOUS_KEY = "-1";

        public ImgurUploader() 
        {
            if (String.IsNullOrEmpty(IMGUR_ANONYMOUS_KEY))
            {
                throw new MissingFieldException("Must provide your imgur API key");
            }
        }

        public Uri UploadImage(Image image)
        {
            return UploadImage(image, image.RawFormat);
        }

        public Uri UploadImage(Image image, ImageFormat format)
        {
            GetCookies();

            string base64Rep = ImageBase64Coder.Encode(image);
            string postData = GetImagePostString(base64Rep);
    
            HttpWebRequest request = SendRequest(uploadUri, postData);
            HttpWebResponse getResponse = (HttpWebResponse)request.GetResponse();

            string imgurResponse;
            using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
            {
                imgurResponse = sr.ReadToEnd();
            }

            return ParseUri(imgurResponse);
        }

        private Uri ParseUri(string xmlString)
        {
            string imageUri;
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                reader.ReadToFollowing("original");
                imageUri = reader.ReadElementContentAsString();
            }
            return new Uri(imageUri);
        }

        private string GetImagePostString(string imageBase64)
        {
            return String.Format("key={0}&image={1}", IMGUR_ANONYMOUS_KEY,
               HttpUtility.UrlEncode(imageBase64));
        }

        private HttpWebRequest SendRequest(string uri, string postData)
        {
            byte[] toPost = Encoding.ASCII.GetBytes(postData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(this.cookies);
            request.Method = WebRequestMethods.Http.Post;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.12 (KHTML, like Gecko) Chrome/24.0.1274.0 Safari/537.12";
            request.AllowWriteStreamBuffering = true;
            request.ProtocolVersion = HttpVersion.Version11;
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = toPost.Length;

            Stream webStream = request.GetRequestStream();
            webStream.Write(toPost, 0, toPost.Length);
            webStream.Close();

            return request;
        }

        private void GetCookies()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUri);
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(this.cookies);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            this.cookies = response.Cookies;
        }
    }
}