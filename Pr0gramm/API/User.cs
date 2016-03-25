using System;
using asapJson;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;

namespace Pr0gramm.API
{
    public class User
    {
        public DateTime Timestamp { get; private set; }
        public string Username { get; private set; }
        public string UserID { get; private set; }
        public string _nonce { get { return this.UserID.Substring(0, 16); } }
        public bool Paid { get; private set; }

        public long qc { get; private set; }
        public long rt { get; private set; }

        private User(JsonNode node, CookieContainer cc)
        {
            this.Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(node.getValue_Object()["ts"].getValue_Number());
            this.rt = (long)node.getValue_Object()["rt"].getValue_Number();
            this.qc = (long)node.getValue_Object()["qc"].getValue_Number();

            string cookieString = cc.GetCookieHeader(new Uri(@"http://pr0gramm.com"));
            cookieString = cookieString.Substring(cookieString.IndexOf("me=") + 3);
            if (cookieString.Contains(";"))
                cookieString = cookieString.Substring(0, cookieString.IndexOf(';') + 1);
            bool flag = false;
            string tmp = "";
            string output = "";
            foreach (var c in cookieString)
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
            JsonNode cookieContent = new JsonNode(output, true);
            this.Username = cookieContent.getValue_Object()["n"].getValue_String();
            this.UserID = cookieContent.getValue_Object()["id"].getValue_String();
            //this.Unknown = (long)cookieContent.getValue_Object()["a"].getValue_Number();
            //this.Unknown = (long)cookieContent.getValue_Object()["pp"].getValue_Number();
            this.Paid = cookieContent.getValue_Object()["paid"].getValue_Boolean();
        }
        public static async Task<User> Login(string username, string password)
        {
            StringBuilder postDataBuilder = new StringBuilder();
            postDataBuilder.Append("name=" + WebUtility.UrlEncode(username));
            postDataBuilder.Append("&password=" + WebUtility.UrlEncode(password));
            byte[] data = Encoding.ASCII.GetBytes(postDataBuilder.ToString());

            string url = app.Settings.Pr0grammUrl.Api;
            url += "user/login";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers["User-Agent"] = app.Settings.UserAgent;
            request.Headers["ContentLength"] = app.Settings.UserAgent;
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream stream = await request.GetRequestStreamAsync())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = await request.GetResponseAsync();

            asapJson.JsonNode responseNode = new asapJson.JsonNode(new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd(), true);
            app.Settings.Instance.Cookie = ((HttpWebRequest)request).CookieContainer;
            response.Dispose();
            return new User(responseNode, ((HttpWebRequest)request).CookieContainer);
        }
        public static User LoadFromSettings()
        {
            if(app.Settings.Instance.Cookie == null)
            {
                throw new Exception("Cannot execute without Settings.Cookie containing a cookie");
            }
            JsonNode node = new JsonNode();
            node.getValue_Object()["ts"] = new JsonNode((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            node.getValue_Object()["rt"] = new JsonNode(0);
            node.getValue_Object()["qt"] = new JsonNode(0);
            return new User(node, app.Settings.Instance.Cookie);
        }
        public async void Logout()
        {
            StringBuilder postDataBuilder = new StringBuilder();
            postDataBuilder.Append("id=" + WebUtility.UrlEncode(this.UserID));
            postDataBuilder.Append("&_nonce=" + WebUtility.UrlEncode(this._nonce));
            byte[] data = Encoding.ASCII.GetBytes(postDataBuilder.ToString());

            string url = app.Settings.Pr0grammUrl.Api;
            url += "user/logout";
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers["User-Agent"] = app.Settings.UserAgent;
            request.Headers["ContentLength"] = app.Settings.UserAgent;
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream stream = await request.GetRequestStreamAsync())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = await request.GetResponseAsync();
            app.Settings.Instance.Cookie = null;
            response.Dispose();
        }

    }
}
