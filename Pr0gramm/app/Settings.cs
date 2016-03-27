using System;
using System.Net;

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
            set { Windows.Storage.ApplicationData.Current.LocalSettings.Values["UseHttps"] = value; }
        }

        private Windows.Web.Http.HttpCookie _Cookie = null;
        public Windows.Web.Http.HttpCookie Cookie
        {
            get
            {
                if (_Cookie == null)
                {
                    var val = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Cookie"];
                    if (val == null)
                        return null;
                    this._Cookie = new Windows.Web.Http.HttpCookie("me", Settings.Pr0grammUrl.Base, "");
                    this._Cookie.Value = (string)val;
                }
                return this._Cookie;
            }
            set { this._Cookie = value; Windows.Storage.ApplicationData.Current.LocalSettings.Values["Cookie"] = value == null ? null : value.Value; }
        }

        public static readonly string UserAgent = @"Pr0gramm/UWP/1.0";

        public struct Pr0grammUrl
        {
            public static string Base { get { return Settings.Instance.UseHttps ? "https://pr0gramm.com/" : "http://pr0gramm.com/"; } }
            public static string Api { get { return Settings.Instance.UseHttps ? "https://pr0gramm.com/api/" : "http://pr0gramm.com/api/"; } }
            public static string Thumb { get { return Settings.Instance.UseHttps ? "https://thumb.pr0gramm.com/" : "http://thumb.pr0gramm.com/"; } }
            public static string Image { get { return Settings.Instance.UseHttps ? "https://img.pr0gramm.com/" : "http://img.pr0gramm.com/"; } }
            public static string Full { get { return Settings.Instance.UseHttps ? "https://full.pr0gramm.com/" : "http://full.pr0gramm.com/"; } }
        }

        ~Settings()
        {
        }
        private Settings()
        {
        }
    }
}
