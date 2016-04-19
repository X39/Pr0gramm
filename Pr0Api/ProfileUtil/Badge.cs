using System;
namespace Pr0gramm.API.ProfileUtil
{
    public class Badge
    {

        public Badge(OpenPr0gramm.ProfileBadge sourceNode)
        {
            this.Link = sourceNode.Link;
            this.Image = sourceNode.Image;
            this.Description = sourceNode.Description;
            this.Created = sourceNode.CreatedAt;
        }

        public DateTime Created { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public string Link { get; private set; }
    }
}
