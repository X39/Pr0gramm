﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asapJson;

namespace Pr0gramm.pr0
{
    public class Image
    {

        public Image(asapJson.JsonNode sourceNode)
        {
            this.Id = (long)sourceNode.getValue_Object()["id"].getValue_Number();
            this.Promoted = (long)sourceNode.getValue_Object()["promoted"].getValue_Number();
            this.Up = (long)sourceNode.getValue_Object()["up"].getValue_Number();
            this.Down = (long)sourceNode.getValue_Object()["down"].getValue_Number();
            this.Created = (long)sourceNode.getValue_Object()["created"].getValue_Number();
            this.ImagePath = sourceNode.getValue_Object()["image"].getValue_String();
            this.Thumb = sourceNode.getValue_Object()["thumb"].getValue_String();
            this.Fullsize = sourceNode.getValue_Object()["fullsize"].getValue_String();
            this.Source = sourceNode.getValue_Object()["source"].getValue_String();
            this.Flags = (long)sourceNode.getValue_Object()["flags"].getValue_Number();
            this.User = sourceNode.getValue_Object()["user"].getValue_String();
            this.Mark = (long)sourceNode.getValue_Object()["mark"].getValue_Number();
        }

        public long Created { get; private set; }
        public long Down { get; private set; }
        public long Flags { get; private set; }
        public string Fullsize { get; private set; }
        public long Id { get; private set; }
        public string ImagePath { get; private set; }
        public long Mark { get; private set; }
        public long Promoted { get; private set; }
        public string Source { get; private set; }
        public string Thumb { get; private set; }
        public long Up { get; private set; }
        public string User { get; private set; }
    }
}
