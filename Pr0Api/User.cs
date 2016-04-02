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

        private User(JsonNode node, Windows.Web.Http.HttpCookie cookie)
        {
            this.Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(node.getValue_Object()["ts"].getValue_Number());
            this.rt = (long)node.getValue_Object()["rt"].getValue_Number();
            this.qc = (long)node.getValue_Object()["qc"].getValue_Number();
            bool flag = false;
            string tmp = "";
            string output = "";
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
            JsonNode cookieContent = new JsonNode(output, true);
            this.Username = cookieContent.getValue_Object()["n"].getValue_String();
            this.UserID = cookieContent.getValue_Object()["id"].getValue_String();
            //this.Unknown = (long)cookieContent.getValue_Object()["a"].getValue_Number();
            //this.Unknown = (long)cookieContent.getValue_Object()["pp"].getValue_Number();
            this.Paid = cookieContent.getValue_Object()["paid"].getValue_Boolean();
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
            StringBuilder postDataBuilder = new StringBuilder();
            postDataBuilder.Append("name=" + WebUtility.UrlEncode(username));
            postDataBuilder.Append("&password=" + WebUtility.UrlEncode(password));
            
            var response = await apiProvider.Client.PostAsync(new Uri(apiProvider.Api + "user/login"), new Windows.Web.Http.HttpStringContent(postDataBuilder.ToString()));
            var responseNode = new JsonNode(response.Content.ToString(), true);
            response.Dispose();
            if (responseNode.getValue_Object().ContainsKey("error"))
            {
                switch (responseNode.getValue_Object()["error"].getValue_String())
                {
                    case "limitReached":
                        throw new Exceptions.RateLimitReached();
                }
                throw new Exception(responseNode.getValue_Object()["error"].getValue_String());
            }
            if (responseNode.getValue_Object()["success"].getValue_Boolean())
            {
                return new User(responseNode, apiProvider.Cookie);
            }
            else
            {
                return null;
            }
        }
        public static User LoadFromCookie(Windows.Web.Http.HttpCookie cookie)
        {
            JsonNode node = new JsonNode(new System.Collections.Generic.Dictionary<string, JsonNode>());
            node.getValue_Object()["ts"] = new JsonNode((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            node.getValue_Object()["rt"] = new JsonNode(0);
            node.getValue_Object()["qc"] = new JsonNode(0);
            return new User(node, cookie);
        }
        public async void Logout(ApiProvider apiProvider)
        {
            StringBuilder postDataBuilder = new StringBuilder();
            postDataBuilder.Append("id=" + WebUtility.UrlEncode(this.UserID));
            postDataBuilder.Append("&_nonce=" + WebUtility.UrlEncode(this._nonce));
            var response = await apiProvider.Client.PostAsync(new Uri(apiProvider.Api + "user/logout"), new Windows.Web.Http.HttpStringContent(postDataBuilder.ToString()));
            response.Dispose();
        }

    }
}
