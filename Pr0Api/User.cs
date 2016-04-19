using System;
using System.Threading.Tasks;
using System.Net;
using System.Text;


namespace Pr0gramm.API
{
    public class User
    {
        public DateTime Timestamp { get; private set; }
        public string Username { get; private set; }
        public string UserID { get; private set; }
        public string _nonce { get { return this.UserID.Substring(0, 16); } }
        public bool Paid { get; private set; }

        //public long qc { get; private set; }
        // long rt { get; private set; }

        private User(OpenPr0gramm.User node, CookieContainer cookie)
        {
            this.Timestamp = node.RegisteredSince;
            //this.rt = (long)0;// ?
            //this.qc = (long)0;// ?

// needs commenting: what does it do? v
/*
            foreach (var c in cookie.Value)
            {
                if (flag)
                {
                    if (tmp.Length == 2)
                    {
                        flag = false;
                        output += ((char)Convert.ToInt32(tmp, 16));
                        tmp = "";
                        if (c == '%')
                        {
                            flag = true;
                        }
                        else
                        {
                            output += (c);
                        }
                    }
                    else
                    {
                        tmp += (c);
                    }
                }
                else
                {
                    if (c == '%')
                    {
                        flag = true;
                    }
                    else
                    {
                        output += (c);
                    }
                }
            }
            output += ((char)Convert.ToInt32(tmp, 16));
*/
// needs commenting: what does it do? ^

            this.Username = node.Name;
            this.UserID = node.Id+"";

            //this.Unknown = (long)cookieContent.getValue_Object()["a"].getValue_Number();
            //this.Unknown = (long)cookieContent.getValue_Object()["pp"].getValue_Number();
            this.Paid = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="apiProvider"></param>
        /// <exception cref="Exceptions.RateLimitReached">Will be thrown when you eg. tried to login too often</exception>
        /// <exception cref="Exception">Will be thrown if something moves wrong and the actual root cause is unknown</exception>
        /// <returns></returns>
        public static async Task<User> Login(string username, string password, ApiProvider apiProvider)
        {
            var response = await apiProvider.bridge.Client.User.LogIn(username, password);
            if (!response.Success)
            {
                if(response.Ban != null && response.Ban.IsBanned)
                {
                    throw new Exception($"Du bist bis {response.Ban.Until} gebannt. Warum? \"{response.Ban.Reason}\".");
                }
                else
                {
                    throw new Exception("Das Passwort war wohl falsch oder so.");
                }
                //                throw new Exceptions.RateLimitReached();
                //                throw new Exception(response.);
            }
            return new User(new OpenPr0gramm.User(), apiProvider.bridge.Client.GetCookies());
        }
        public static User LoadFromCookie(CookieContainer cookie)
        {
            ApiBridge.Instance.Client = new OpenPr0gramm.Pr0grammClient(cookie);
            return new User(new OpenPr0gramm.User(), cookie);
        }
        public async void Logout(ApiProvider apiProvider)
        {
           var response = await apiProvider.bridge.Client.User.LogOut();
        }

    }
}
