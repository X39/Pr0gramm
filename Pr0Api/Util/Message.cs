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

        public Message(asapJson.JsonNode souceNode)
        {
            //api call profile/info has a bug for messages which causes all numbers to be strings ...
            //that is why this weirdo casting has to happen for the generic message object

            asapJson.JsonNode tmpNode;
            double tmpVal;

            tmpNode = souceNode.getValue_Object()["id"];
            if (tmpNode.Type == asapJson.JsonNode.EJType.Number)
                this.Id = (long)tmpNode.getValue_Number();
            else if (tmpNode.Type == asapJson.JsonNode.EJType.String)
            {
                if (double.TryParse(tmpNode.getValue_String(), out tmpVal))
                    this.Id = (long)tmpVal;
                else
                    this.Id = -1;
            }
            tmpNode = souceNode.getValue_Object()["up"];
            if (tmpNode.Type == asapJson.JsonNode.EJType.Number)
                this.Up = (long)tmpNode.getValue_Number();
            else if (tmpNode.Type == asapJson.JsonNode.EJType.String)
            {
                if (double.TryParse(tmpNode.getValue_String(), out tmpVal))
                    this.Up = (long)tmpVal;
                else
                    this.Up = -1;
            }
            tmpNode = souceNode.getValue_Object()["down"];
            if (tmpNode.Type == asapJson.JsonNode.EJType.Number)
                this.Down = (long)tmpNode.getValue_Number();
            else if (tmpNode.Type == asapJson.JsonNode.EJType.String)
            {
                if (double.TryParse(tmpNode.getValue_String(), out tmpVal))
                    this.Down = (long)tmpVal;
                else
                    this.Down = -1;
            }
            this.Content = souceNode.getValue_Object()["content"].getValue_String();
            tmpNode = souceNode.getValue_Object()["created"];
            if (tmpNode.Type == asapJson.JsonNode.EJType.Number)
                this.Created = ApiProvider.UnixTimestamp0.AddSeconds(tmpNode.getValue_Number());
            else if (tmpNode.Type == asapJson.JsonNode.EJType.String)
            {
                if (double.TryParse(tmpNode.getValue_String(), out tmpVal))
                    this.Created = ApiProvider.UnixTimestamp0.AddSeconds(tmpVal);
                else
                    this.Created = ApiProvider.UnixTimestamp0;
            }

            //Only for ItemInfo messages
            this.Parent = souceNode.getValue_Object().ContainsKey("parent") ? (long)souceNode.getValue_Object()["parent"].getValue_Number() : 0;
            this.Confidence = souceNode.getValue_Object().ContainsKey("confidence") ? (long)souceNode.getValue_Object()["confidence"].getValue_Number() : 0;
            this.Author = souceNode.getValue_Object().ContainsKey("name") ? souceNode.getValue_Object()["name"].getValue_String() : "";
            this.Mark = souceNode.getValue_Object().ContainsKey("mark") ? (long)souceNode.getValue_Object()["mark"].getValue_Number() : 0;

            //Only for Profile messages
            this.Thumb = souceNode.getValue_Object().ContainsKey("thumb") ? souceNode.getValue_Object()["thumb"].getValue_String() : "";
            if(souceNode.getValue_Object().ContainsKey("itemId"))
            {
                tmpNode = souceNode.getValue_Object()["itemId"];
                if (tmpNode.Type == asapJson.JsonNode.EJType.Number)
                    this.ItemId = (long)tmpNode.getValue_Number();
                else if (tmpNode.Type == asapJson.JsonNode.EJType.String)
                {
                    if (double.TryParse(tmpNode.getValue_String(), out tmpVal))
                        this.ItemId = (long)tmpVal;
                    else
                        this.ItemId = -1;
                }
            }
            else
            {
                this.ItemId = 0;
            }
        }
    }
}
