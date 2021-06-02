using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class UserSubscriptionDetails
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime? ExpiryDate { get; set; }
        [DataMember]
        public int? Projects { get; set; }

        [DataMember]
        public int? Users { get; set; }

        [DataMember]
        public int? DatabaseSize { get; set; }

        [DataMember]
        public int? Devices { get; set; }

        [DataMember]
        public int? APICalls { get; set; }

        [DataMember]
        public int? DesktopInstalls { get; set; }
        [DataMember]
        public int? SubscriptionId { get; set; }
        [DataMember]
        public int? UserId { get; set; }
        public UserSubscriptionDetails()
        {

        }
    }
}
