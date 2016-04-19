using System;

namespace Pr0gramm.API.Util
{
    public class Message
    {
        public string Content { get; private set; }
        public long Confidence { get; private set; }
        public DateTime Created { get; private set; }
        public long Down { get; private set; }
        public long Id { get; private set; }
        public long Mark { get; private set; }
        public long Parent { get; private set; }
        public string Author { get; private set; }
        public long Up { get; private set; }
        public ItemInfo Owner { get; private set; }
        public string Thumb { get; private set; }
        public long ItemId { get; private set; }

        public Message(object souceNode)
        {
            OpenPr0gramm.Item _node = (OpenPr0gramm.Item)souceNode;
            this.Id = _node.Id;
            this.Up = _node.Upvotes;
            this.Down = _node.Downvotes;
            this.Content = _node.ToString();
            this.Created = _node.CreatedAt;

            if (souceNode.GetType() == typeof(OpenPr0gramm.ItemComment))
            {
                OpenPr0gramm.ItemComment node = (OpenPr0gramm.ItemComment)souceNode;

                this.Parent = node.ParentId;
                this.Confidence = (long)node.Confidence;
                this.Author = node.Name;
                this.Mark = (long)node.Mark;
            }
            if (souceNode.GetType() == typeof(OpenPr0gramm.ProfileComment))
            {
                OpenPr0gramm.ProfileComment node = (OpenPr0gramm.ProfileComment)souceNode;
                this.Thumb = node.ThumbnailUrl;
                this.ItemId = (long)node.ItemId;
            } 

        }
    }
}
