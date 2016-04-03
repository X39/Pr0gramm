using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Pr0gramm.API.inbox
{
    public class Post
    {
        private Post()
        {

        }

        public static async Task<bool> SendMessage(string content, Profile receiver, User user, ApiProvider apiProvider
        {
            StringBuilder postDataBuilder = new StringBuilder();
            postDataBuilder.Append("comment="); postDataBuilder.Append(WebUtility.UrlEncode(content));
            postDataBuilder.Append("&parentId=\"\"");
            postDataBuilder.Append("&itemId=\"\"");
            postDataBuilder.Append("&recipientId="); postDataBuilder.Append(receiver.UserID);
            postDataBuilder.Append("&_nonce="); postDataBuilder.Append(WebUtility.UrlEncode(user._nonce));
            var response = await apiProvider.Client.PostAsync(new Uri(apiProvider.Api + "inbox/post"), new Windows.Web.Http.HttpStringContent(postDataBuilder.ToString(), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded"));
            response.Dispose();
            return response.StatusCode == Windows.Web.Http.HttpStatusCode.Ok;
        }
    }
}
