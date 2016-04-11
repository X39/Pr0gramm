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
                APIProvider.UseHttps = value;
            }
        }

        private HttpCookie _StoredCookie = null;
        public HttpCookie StoredCookie
        {
            get
            {
                if (_StoredCookie == null)
                {
                    var val = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Cookie"];
                    if (val == null)
                        return null;
                    this._StoredCookie = new HttpCookie("me", this.APIProvider.Base, "");
                    this._StoredCookie.Value = (string)val;
                }
                return this._StoredCookie;
            }
            set { this._StoredCookie = value; Windows.Storage.ApplicationData.Current.LocalSettings.Values["Cookie"] = value == null ? null : value.Value; }
        }

        public User Pr0User { get; internal set; }
        public ApiProvider APIProvider { get; internal set; }
        
        private Settings()
        {
            this.APIProvider = new ApiProvider(@"Pr0gramm/UWP/1.0", this.UseHttps);
            this.Pr0User = null;
            var cookie = this.StoredCookie;
            if(cookie != null)
            {
                this.APIProvider.Cookie = cookie;
            }

        }
    }
}
