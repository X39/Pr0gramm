using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr0gramm.API
{
    public class UrlProvider
    {
        public string Base { get { return UseHttps ? "https://pr0gramm.com/" : "http://pr0gramm.com/"; } }
        public string Api { get { return UseHttps ? "https://pr0gramm.com/api/" : "http://pr0gramm.com/api/"; } }
        public string Thumb { get { return UseHttps ? "https://thumb.pr0gramm.com/" : "http://thumb.pr0gramm.com/"; } }
        public string Image { get { return UseHttps ? "https://img.pr0gramm.com/" : "http://img.pr0gramm.com/"; } }
        public string Full { get { return UseHttps ? "https://full.pr0gramm.com/" : "http://full.pr0gramm.com/"; } }

        public bool UseHttps { get; private set; }
        public string UserAgent { get; private set; }

        public UrlProvider(string userAgent, bool useHttps = true)
        {
            this.UseHttps = useHttps;
            this.UserAgent = userAgent;
        }
    }
}
