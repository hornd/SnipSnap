using System;
using System.Net;

namespace SnipSnap
{
    public class ImgurUploader : WebClient
    {
        public CookieContainer cookies;

        public ImgurUploader()
        {
            cookies = new CookieContainer();
            this.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.12 (KHTML, like Gecko) Chrome/24.0.1274.0 Safari/537.12");
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var req = base.GetWebRequest(address);
            if (req is HttpWebRequest)
            {
                ((HttpWebRequest)req).CookieContainer = cookies;
            }
            return req;
        }
    }
}