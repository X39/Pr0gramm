namespace Pr0gramm.API.ProfileUtil
{
    public class ImgLink
    {

        public ImgLink(object sourceNode)
        {
            if (sourceNode.GetType() == typeof(OpenPr0gramm.ProfileUpload))
            {
                var node = (OpenPr0gramm.ProfileUpload)sourceNode;
                this.ID = node.Id + "";
                this.Thumb = node.ThumbnailUrl;

            }
            if (sourceNode.GetType() == typeof(OpenPr0gramm.LikedItem))
            {
                var node = (OpenPr0gramm.LikedItem)sourceNode;
                this.ID = node.Id + "";
                this.Thumb = node.ThumbnailUrl;

            }

        }

        public string ID { get; private set; }
        public string Thumb { get; private set; }
        
    }
}
