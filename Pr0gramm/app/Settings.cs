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

        public bool HasChanges { get; private set; }

        private bool _UseHttps;
        public bool UseHttps { get { return this._UseHttps; } set { if (value != this._UseHttps) this.HasChanges = true; this._UseHttps = value; } }
        private CookieContainer _Cookie;
        public CookieContainer Cookie { get { return this._Cookie; } set { if (value != this._Cookie) this.HasChanges = true; this._Cookie = value; } }

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
            if (this.HasChanges)
                this.save();
        }
        private Settings()
        {
            this.load();
        }

        private void load()
        {
            //throw new NotImplementedException();
        }
        public void save()
        {
            throw new NotImplementedException();
        }
    }
}
