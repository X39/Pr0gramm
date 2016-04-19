using System;
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

            public Tag(OpenPr0gramm.Tag sourceNode)
            {
                this.Id = sourceNode.Id;
                this.Confidence = (long)sourceNode.Confidence;
                this.Text = sourceNode.Content;
            }
        }
        public class Comment
        {
            private Util.Message _msg;
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

            public Comment(OpenPr0gramm.Comment souceNode, ItemInfo owner)
            {
                this._msg = new Util.Message(souceNode as OpenPr0gramm.ItemComment);
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

        private ItemInfo(OpenPr0gramm.GetItemsInfoResponse sourceNode, ItemsGetterUtil.Image owner)
        {
            this.Tags = new List<Tag>();
            foreach (var node in sourceNode.Tags)
            {
                this.Tags.Add(new Tag(node));
            }

            this.Comments = new List<Comment>();
            var tmpComments = new List<Comment>();
            foreach (var node in sourceNode.Comments)
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

            this.Timestamp = sourceNode.TS;
            this.Cache = sourceNode.Cache.ToString();
            this.Rt = (long)sourceNode.RT;
            this.Qc = (long)sourceNode.QC;

            this.Owner = owner;
        }
        public static async Task<ItemInfo> Fetch(ItemsGetterUtil.Image img, ApiProvider apiProvider)
        {
            var response = await apiProvider.bridge.Client.Item.GetInfo((int)img.Id);

            return new ItemInfo(response, img);
        }
    }
}
