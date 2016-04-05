using System;
using System.Threading;
namespace Pr0gramm.API.ProfileUtil
{
    public class ImgLink
    {

        public ImgLink(asapJson.JsonNode sourceNode)
        {
            this.ID = sourceNode.getValue_Object()["id"].getValue_String();
            this.Thumb = sourceNode.getValue_Object()["thumb"].getValue_String();
        }

        public string ID { get; private set; }
        public string Thumb { get; private set; }
        
    }
}
