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

        public static async Task<bool> SendMessage(string content, Profile receiver, User user, ApiProvider apiProvider)
        {
            var response = await apiProvider.bridge.Client.Inbox.SendMessage((int)receiver.UserID, content);
            return response.ToString().Equals("");
        }
    }
}
