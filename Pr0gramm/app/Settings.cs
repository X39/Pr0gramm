using Pr0gramm.API;
using System;
using System.Net;
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
                ApiProvider.UseHttps = value;
            }
        }

        private CookieContainer _StoredCookie = null;
        public CookieContainer StoredCookie
        {
            get
            {
                if (_StoredCookie == null)
                {
                    var val = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Cookie"];
                    if (val == null)
                        return null;
                    this._StoredCookie = new CookieContainer();
                    Cookie c = new Cookie();
                    c.Value = (string)val;
                    //this._StoredCookie.Add(new Uri(ApiProvider.Base,UriKind.Absolute),c);
                }
                return this._StoredCookie;
            }
            set {
                this._StoredCookie = value;
                var me = this._StoredCookie.GetCookies(new Uri(ApiProvider.Base))["me"];
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["Cookie"] = me == null ? null : me;
            }
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
                this.APIProvider = new ApiProvider(@"Pr0gramm/UWp/1.0", this.UseHttps, cookie);
            }

        }
    }
}
