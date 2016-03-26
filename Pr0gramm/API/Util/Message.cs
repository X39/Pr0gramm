using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Message(asapJson.JsonNode souceNode)
        {
            asapJson.JsonNode tmpNode;
            this.Id = (long)souceNode.getValue_Object()["id"].getValue_Number();
            this.Up = (long)souceNode.getValue_Object()["up"].getValue_Number();
            this.Down = (long)souceNode.getValue_Object()["down"].getValue_Number();
            this.Content = souceNode.getValue_Object()["content"].getValue_String();
            this.Created = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(souceNode.getValue_Object()["created"].getValue_Number());

            //Only for ItemInfo messages
            tmpNode = souceNode.getValue_Object()["parent"];
            this.Parent = tmpNode == null ? 0 : (long)tmpNode.getValue_Number();
            tmpNode = souceNode.getValue_Object()["confidence"];
            this.Confidence = tmpNode == null ? 0 : (long)tmpNode.getValue_Number();
            tmpNode = souceNode.getValue_Object()["name"];
            this.Author = tmpNode == null ? "" : tmpNode.getValue_String();
            tmpNode = souceNode.getValue_Object()["mark"];
            this.Mark = tmpNode == null ? 0 : (long)tmpNode.getValue_Number();

            //Only for Profile messages
            tmpNode = souceNode.getValue_Object()["thumb"];
            this.Thumb = tmpNode == null ? "" : tmpNode.getValue_String();
            tmpNode = souceNode.getValue_Object()["itemId"];
            this.ItemId = tmpNode == null ? 0 : (long)tmpNode.getValue_Number();
        }
    }
}
