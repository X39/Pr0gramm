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
        public static async Task<Tuple<User, Windows.Web.Http.HttpCookie>> Login(string username, string password, UrlProvider urlProvider)
        {
            StringBuilder postDataBuilder = new StringBuilder();
            postDataBuilder.Append("name=" + WebUtility.UrlEncode(username));
            postDataBuilder.Append("&password=" + WebUtility.UrlEncode(password));
            var filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient(filter);
            
            client.DefaultRequestHeaders.UserAgent.TryParseAdd(urlProvider.UserAgent);
            var response = await client.PostAsync(new Uri(urlProvider.Api + "user/login"), new Windows.Web.Http.HttpStringContent(postDataBuilder.ToString()));
            var responseNode = new JsonNode(response.Content.ToString(), true);
            if (responseNode.getValue_Object()["success"].getValue_Boolean())
            {
                foreach (var cookie in filter.CookieManager.GetCookies(new Uri(urlProvider.Base)))
                {
                    if (cookie.Name == "me")
                    {
                        var usr = new User(responseNode, cookie);
                        response.Dispose();
                        return new Tuple<User, Windows.Web.Http.HttpCookie>(usr, cookie);
                    }
                }
                throw new Exception();
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
        public async void Logout(UrlProvider urlProvider, Windows.Web.Http.HttpCookie cookie)
        {
            StringBuilder postDataBuilder = new StringBuilder();
            postDataBuilder.Append("id=" + WebUtility.UrlEncode(this.UserID));
            postDataBuilder.Append("&_nonce=" + WebUtility.UrlEncode(this._nonce));
            var filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            filter.CookieManager.SetCookie(cookie);
            Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient(filter);

            client.DefaultRequestHeaders.UserAgent.TryParseAdd(urlProvider.UserAgent);
            var response = await client.PostAsync(new Uri(urlProvider.Api + "user/logout"), new Windows.Web.Http.HttpStringContent(postDataBuilder.ToString()));
            response.Dispose();
        }

    }
}
