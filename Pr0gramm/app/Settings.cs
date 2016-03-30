using Pr0gramm.API;
using Windows.Web.Http;

namespace Pr0gramm.app
{
    public class Settings
    {
        private static Settings instance;
        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                }
                return instance;
            }
        }

        public bool UseHttps
        {
            get
            {
                var val = Windows.Storage.ApplicationData.Current.LocalSettings.Values["UseHttps"];
                return val == null ? false : (bool)val;
            }
            set
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["UseHttps"] = value;
                this.Url = new UrlProvider(@"Pr0gramm/UWP/1.0", value);
            }
        }

        private HttpCookie _Cookie = null;
        public HttpCookie Cookie
        {
            get
            {
                if (_Cookie == null)
                {
                    var val = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Cookie"];
                    if (val == null)
                        return null;
                    this._Cookie = new HttpCookie("me", this.Url.Base, "");
                    this._Cookie.Value = (string)val;
                }
                return this._Cookie;
            }
            set { this._Cookie = value; Windows.Storage.ApplicationData.Current.LocalSettings.Values["Cookie"] = value == null ? null : value.Value; }
        }

        public User Pr0User { get; internal set; }
        public UrlProvider Url { get; internal set; }

        
        ~Settings()
        {
        }
        private Settings()
        {
            this.Url = new UrlProvider(@"Pr0gramm/UWP/1.0", this.UseHttps);
            this.Pr0User = null;
        }
    }
}
