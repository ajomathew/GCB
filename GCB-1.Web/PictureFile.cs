using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GCB_1.Web
{
    [DataContract]
    public class PictureFile
    {
        [DataMember]
        public string PictureName { get; set; }

        [DataMember]
        public byte[] PictureStream { get; set; }
    }
}