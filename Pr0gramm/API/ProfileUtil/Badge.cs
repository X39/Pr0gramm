using System;
namespace Pr0gramm.API.ProfileUtil
{
    public class Badge
    {

        public Badge(asapJson.JsonNode sourceNode)
        {
            this.Link = sourceNode.getValue_Object()["link"].getValue_String();
            this.Image = sourceNode.getValue_Object()["image"].getValue_String();
            this.Description = sourceNode.getValue_Object()["description"].getValue_String();
            this.Created = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(sourceNode.getValue_Object()["created"].getValue_Number());
        }

        public DateTime Created { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public string Link { get; private set; }
    }
}
