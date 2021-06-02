using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class Subscription
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Projects { get; set; }

        [DataMember]
        public int Users { get; set; }

        [DataMember]
        public Guid? ParentId { get; set; }

        [DataMember]
        public DateTime? Expiry { get; set; }

        [DataMember]
        public int? ExtraUsers { get; set; }

        [DataMember]
        public int? ExtraDevices { get; set; }

        [DataMember]
        public int? ExtraStorage { get; set; }

        [DataMember]
        public int MaxUsers { get; set; }

        [DataMember]
        public int MaxProjects { get; set; }
    }
}
