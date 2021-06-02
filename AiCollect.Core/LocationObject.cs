using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class LocationObject
    {
        [DataMember]
        public string DISTRICT { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string SUB_COUNTY { get; set; }
        [DataMember]
        public string PARISH { get; set; }
        [DataMember]
        public string VILLAGE { get; set; }
    }
}

