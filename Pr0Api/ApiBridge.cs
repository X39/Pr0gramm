using System.Net;
using OpenPr0gramm;

namespace Pr0gramm.API
{
    public class ApiBridge
    {
        public Pr0grammClient Client;


        // singleton stuff
        private static ApiBridge instance;
        private static CookieContainer cookie;
        public static ApiBridge Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = init();
                }
                return instance;
            }
        }
        private ApiBridge() { }
        public static ApiBridge init() {
            instance = new ApiBridge();
            if (instance.Client == null)
            {
                if (cookie != null)
                {
                    instance.Client = new Pr0grammClient(cookie);
                } else
                {
                    instance.Client = new Pr0grammClient();
                }
            }
            return instance;
        }
        public ApiBridge(CookieContainer cookie)
        {
            this.Client = new Pr0grammClient(cookie);
            Instance.Client = this.Client;
        }
    }
}
