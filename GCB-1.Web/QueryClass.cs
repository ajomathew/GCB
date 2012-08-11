using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GCB_1.Web.filldata
{
    [DataContract]
    
    public class QueryClass
    {
        [DataMember]
        public string str { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public string xml { get; set; }
        [DataMember]
        public string PictureName { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public byte[] PictureStream { get; set; }
        [DataMember]
        public bool ret { get; set; }
    }
}