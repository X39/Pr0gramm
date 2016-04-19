using System;
using System.Net;
using Windows.Web.Http;

namespace Pr0gramm.API
{
    /// <summary>
    /// Lifetime of this object has to be over the whole course of the application runtime
    /// </summary>
    public class ApiProvider
    {
        public static readonly DateTime UnixTimestamp0 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static string Base { get { return UseHttps ? "https://pr0gramm.com/" : "http://pr0gramm.com/"; } }
        public static string Api { get { return UseHttps ? "https://pr0gramm.com/api/" : "http://pr0gramm.com/api/"; } }
        public static string Thumb { get { return UseHttps ? "https://thumb.pr0gramm.com/" : "http://thumb.pr0gramm.com/"; } }
        public static string Image { get { return UseHttps ? "https://img.pr0gramm.com/" : "http://img.pr0gramm.com/"; } }
        public static string Full { get { return UseHttps ? "https://full.pr0gramm.com/" : "http://full.pr0gramm.com/"; } }

        public static bool UseHttps { get; set; }
        public string UserAgent { get; private set; }

        public ApiBridge bridge;
        public CookieContainer Cookie = new CookieContainer();

        public ApiProvider(string userAgent, bool useHttps = true, CookieContainer cookie = null)
        {
            UseHttps = useHttps;
            this.UserAgent = userAgent;
            this.Cookie = cookie;
            bridge = new ApiBridge(Cookie);
        }
    }
}
