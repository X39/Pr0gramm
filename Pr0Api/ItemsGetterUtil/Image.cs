using System;

namespace Pr0gramm.API.ItemsGetterUtil
{
    public class Image
    {

        public Image(OpenPr0gramm.Item sourceNode)
        {
            this.Id = (long)sourceNode.Id;
            this.Promoted = (long)sourceNode.PromotedId;
            this.Up = (long)sourceNode.Upvotes;
            this.Down = (long)sourceNode.Downvotes;
            this.Created = sourceNode.CreatedAt;
            this.ImagePath = sourceNode.ImageUrl;
            this.Thumb = sourceNode.ThumbnailUrl;
            this.Fullsize = sourceNode.FullSizeUrl;
            this.Source = sourceNode.Source;
            this.Flags = (long)sourceNode.Flags;
            this.User = sourceNode.User;
            
            this.UserMark = new ProfileUtil.Mark((int)sourceNode.Mark);
        }

        public DateTime Created { get; private set; }
        public long Down { get; private set; }
        public long Flags { get; private set; }
        public string Fullsize { get; private set; }
        public long Id { get; private set; }
        public string ImagePath { get; private set; }
        public ProfileUtil.Mark UserMark { get; private set; }
        public long Promoted { get; private set; }
        public string Source { get; private set; }
        public string Thumb { get; private set; }
        public long Up { get; private set; }
        public string User { get; private set; }
    }
}
