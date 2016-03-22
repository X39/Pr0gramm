using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Pr0gramm.pr0
{
    public class ItemInfo
    {
        public class Tag
        {
            public long Id { get; private set; }
            public long Confidence { get; private set; }
            public string Text { get; private set; }

            public Tag(asapJson.JsonNode sourceNode)
            {
                this.Id = (long)sourceNode.getValue_Object()["id"].getValue_Number();
                this.Confidence = (long)sourceNode.getValue_Object()["confidence"].getValue_Number();
                this.Text = sourceNode.getValue_Object()["tag"].getValue_String();
            }
        }
        public class Comment
        {
            public List<Comment> Children { get; private set; }
            public string Content { get; private set; }
            public long Confidence { get; private set; }
            public long Created { get; private set; }
            public long Down { get; private set; }
            public long Id { get; private set; }
            public long Mark { get; private set; }
            public long Parent { get; private set; }
            public string PosterName { get; private set; }
            public long Up { get; private set; }

            public Comment(asapJson.JsonNode souceNode)
            {
                this.Children = new List<Comment>();
                this.Id = (long)souceNode.getValue_Object()["id"].getValue_Number();
                this.Parent = (long)souceNode.getValue_Object()["parent"].getValue_Number();
                this.Content = souceNode.getValue_Object()["content"].getValue_String();
                this.Created = (long)souceNode.getValue_Object()["created"].getValue_Number();
                this.Up = (long)souceNode.getValue_Object()["up"].getValue_Number();
                this.Down = (long)souceNode.getValue_Object()["down"].getValue_Number();
                this.Confidence = (long)souceNode.getValue_Object()["confidence"].getValue_Number();
                this.PosterName = souceNode.getValue_Object()["name"].getValue_String();
                this.Mark = (long)souceNode.getValue_Object()["mark"].getValue_Number();
            }

        }
        public List<Tag> Tags { get; private set; }
        public List<Comment> Comments { get; private set; }
        public long Ts { get; private set; }
        public string Cache { get; private set; }
        public long Rt { get; private set; }
        public long Qc { get; private set; }

        public ItemInfo(asapJson.JsonNode sourceNode)
        {
            this.Tags = new List<Tag>();
            foreach (var node in sourceNode.getValue_Object()["tags"].getValue_Array())
            {
                this.Tags.Add(new Tag(node));
            }

            this.Comments = new List<Comment>();
            var tmpComments = new List<Comment>();
            foreach (var node in sourceNode.getValue_Object()["comments"].getValue_Array())
            {
                tmpComments.Add(new Comment(node));
            }
            foreach(var it in tmpComments)
            {
                if(it.Parent == 0)
                {
                    this.Comments.Add(it);
                }
                else
                {
                    tmpComments.Find((obj) => obj.Id == it.Parent).Children.Add(it);
                }
            }

            this.Ts = (long)sourceNode.getValue_Object()["ts"].getValue_Number();
            this.Cache = sourceNode.getValue_Object()["cache"].getValue_String();
            this.Rt = (long)sourceNode.getValue_Object()["rt"].getValue_Number();
            this.Qc = (long)sourceNode.getValue_Object()["qc"].getValue_Number();
        }
        public static async Task<ItemInfo> Fetch(long id)
        {
            string url = Settings.Pr0grammUrl.Api;
            url += "items/info?itemId=" + id;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers["User-Agent"] = Settings.UserAgent;
            ((HttpWebRequest)request).CookieContainer = Settings.Instance.Cookie;

            var response = await request.GetResponseAsync();

            asapJson.JsonNode responseNode = new asapJson.JsonNode(new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd(), true);
            return new ItemInfo(responseNode);
        }
    }
}
