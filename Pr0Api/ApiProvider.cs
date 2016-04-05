using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Pr0gramm.API
{
    /// <summary>
    /// Lifetime of this object has to be over the whole course of the application runtime
    /// </summary>
    public class ApiProvider
    {
        public static readonly DateTime UnixTimestamp0 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public string Base { get { return UseHttps ? "https://pr0gramm.com/" : "http://pr0gramm.com/"; } }
        public string Api { get { return UseHttps ? "https://pr0gramm.com/api/" : "http://pr0gramm.com/api/"; } }
        public string Thumb { get { return UseHttps ? "https://thumb.pr0gramm.com/" : "http://thumb.pr0gramm.com/"; } }
        public string Image { get { return UseHttps ? "https://img.pr0gramm.com/" : "http://img.pr0gramm.com/"; } }
        public string Full { get { return UseHttps ? "https://full.pr0gramm.com/" : "http://full.pr0gramm.com/"; } }

        public bool UseHttps { get; set; }
        public string UserAgent { get; private set; }
        public HttpClient Client { get; private set; }
        public HttpBaseProtocolFilter Filter { get; private set; }
        public HttpCookie Cookie
        {
            get
            {
                foreach (var cookie in this.Filter.CookieManager.GetCookies(new Uri(this.Base)))
                {
                    if (cookie.Name == "me")
                    {
                        return cookie;
                    }
                }
                return default(HttpCookie);
            }
            set
            {
                this.Filter.CookieManager.SetCookie(value);
            }
        }

        public ApiProvider(string userAgent, bool useHttps = true, HttpCookie cookie = null)
        {
            this.UseHttps = useHttps;
            this.UserAgent = userAgent;
            this.Filter = new HttpBaseProtocolFilter();
            if(cookie != null)
                this.Filter.CookieManager.SetCookie(cookie);
            this.Client = new HttpClient(this.Filter);
            this.Client.DefaultRequestHeaders.UserAgent.TryParseAdd(this.UserAgent);
        }
    }
}
