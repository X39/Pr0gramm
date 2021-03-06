﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pr0gramm.API
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
            private API.Util.Message _msg;
            public List<Comment> Children { get; private set; }
            public string Content { get { return _msg.Content; } }
            public long Confidence { get { return _msg.Confidence; } }
            public DateTime Created { get { return _msg.Created; } }
            public long Down { get { return _msg.Down; } }
            public long Id { get { return _msg.Id; } }
            public long Mark { get { return _msg.Mark; } }
            public long Parent { get { return _msg.Parent; } }
            public string Author { get { return _msg.Author; } }
            public long Up { get { return _msg.Up; } }
            public ItemInfo Owner { get; private set; }

            public Comment(asapJson.JsonNode souceNode, ItemInfo owner)
            {
                this._msg = new Util.Message(souceNode);
                this.Children = new List<Comment>();
                this.Owner = owner;
            }
        }
        public List<Tag> Tags { get; private set; }
        public List<Comment> Comments { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Cache { get; private set; }
        public long Rt { get; private set; }
        public long Qc { get; private set; }
        public ItemsGetterUtil.Image Owner { get; private set; }

        private ItemInfo(asapJson.JsonNode sourceNode, ItemsGetterUtil.Image owner)
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
                tmpComments.Add(new Comment(node, this));
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

            this.Timestamp = ApiProvider.UnixTimestamp0.AddSeconds(sourceNode.getValue_Object()["ts"].getValue_Number());
            this.Cache = sourceNode.getValue_Object()["cache"].getValue_String();
            this.Rt = (long)sourceNode.getValue_Object()["rt"].getValue_Number();
            this.Qc = (long)sourceNode.getValue_Object()["qc"].getValue_Number();

            this.Owner = owner;
        }
        public static async Task<ItemInfo> Fetch(ItemsGetterUtil.Image img, ApiProvider apiProvider)
        {
            string url = apiProvider.Api;
            url += "items/info?itemId=" + img.Id;

            var response = await apiProvider.Client.GetAsync(new Uri(url));

            asapJson.JsonNode responseNode = new asapJson.JsonNode(await response.Content.ReadAsStringAsync(), true);
            response.Dispose();
            return new ItemInfo(responseNode, img);
        }
    }
}
