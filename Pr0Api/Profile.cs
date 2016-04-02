using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using asapJson;
using Pr0gramm.API.ProfileUtil;
using Pr0gramm.API.Util;
using System.Net;

namespace Pr0gramm.API
{
    public class Profile
    {
        private Profile(JsonNode node)
        {
            JsonNode tmpNode;
            tmpNode = node.getValue_Object()["user"];
            this.UserID = (long)tmpNode.getValue_Object()["id"].getValue_Number();
            this.Username = (long)tmpNode.getValue_Object()["name"].getValue_Number();
            this.Registered = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tmpNode.getValue_Object()["registered"].getValue_Number());
            this.Score = (long)tmpNode.getValue_Object()["score"].getValue_Number();
            this.MarkObj = new API.ProfileUtil.Mark((int)tmpNode.getValue_Object()["score"].getValue_Number());
            this.IsAdmin = tmpNode.getValue_Object()["admin"].getValue_Boolean();
            this.IsBanned = tmpNode.getValue_Object()["banned"].getValue_Boolean();
            this.CommentCount = (long)node.getValue_Object()["commentCount"].getValue_Number();
            this.LikeCount = (long)node.getValue_Object()["likeCount"].getValue_Number();
            this.TagCount = (long)node.getValue_Object()["tagCount"].getValue_Number();
            this.FollowCount = (long)node.getValue_Object()["followCount"].getValue_Number();
            this.Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tmpNode.getValue_Object()["ts"].getValue_Number());
            this.Following = node.getValue_Object()["following"].getValue_Boolean();
            //this.Cache = node.getValue_Object()["cache"];
            this.Rt = (long)node.getValue_Object()["rt"].getValue_Number();
            this.Qc = (long)node.getValue_Object()["qc"].getValue_Number();

            if (tmpNode.getValue_Object().ContainsKey("bannedUntil") && tmpNode.getValue_Object()["bannedUntil"].Type != JsonNode.EJType.Object)
                this.BannedUntil = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(tmpNode.getValue_Number());

            tmpNode = node.getValue_Object()["comments"];
            this.Comments = new List<API.Util.Message>();
            foreach (var it in tmpNode.getValue_Array())
            {
                this.Comments.Add(new Message(it));
            }

            tmpNode = node.getValue_Object()["uploads"];
            this.Uploads = new List<API.ItemsGetterUtil.Image>();
            foreach (var it in tmpNode.getValue_Array())
            {
                this.Uploads.Add(new API.ItemsGetterUtil.Image(it));
            }

            tmpNode = node.getValue_Object()["badges"];
            this.Badges = new List<API.ProfileUtil.Badge>();
            foreach (var it in tmpNode.getValue_Array())
            {
                this.Badges.Add(new API.ProfileUtil.Badge(it));
            }
        }

        public List<Badge> Badges { get; private set; }
        public DateTime? BannedUntil { get; private set; }
        public long CommentCount { get; private set; }
        public List<API.Util.Message> Comments { get; private set; }
        public long FollowCount { get; private set; }
        public bool Following { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsBanned { get; private set; }
        public long LikeCount { get; private set; }
        public Mark MarkObj { get; private set; }
        public long Qc { get; private set; }
        public DateTime Registered { get; private set; }
        public long Rt { get; private set; }
        public long Score { get; private set; }
        public long TagCount { get; private set; }
        public DateTime Timestamp { get; private set; }
        public List<API.ItemsGetterUtil.Image> Uploads { get; private set; }
        public long UserID { get; private set; }
        public long Username { get; private set; }

        public async Task<Profile> Fetch(string profileName, ApiProvider apiProvider)
        {
            string url = apiProvider.Api;
            url += "profile/info?name=" + WebUtility.UrlEncode(profileName);
            var response = await apiProvider.Client.GetAsync(new Uri(url));
            asapJson.JsonNode responseNode = new asapJson.JsonNode(await response.Content.ReadAsStringAsync(), true);
            response.Dispose();
            return new Profile(responseNode);
        }
    }
}
