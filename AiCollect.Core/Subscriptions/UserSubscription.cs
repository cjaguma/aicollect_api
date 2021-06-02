using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class UserSubscription
    {
        public UserSubscriptionDetails Details { get; set; }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public Guid? SubscriptionId { get; set; }
        [DataMember]
        public Guid? ParentId { get; set; }
        public DateTime? Renewed { get; set; }
        [DataMember]
        public DateTime? Expiry { get; set; }
        [DataMember]
        public int? ExtraUsers { get; set; }
        [DataMember]
        public int? ExtraDevices { get; set; }
        [DataMember]
        public int? ExtraStorage { get; set; }
        [DataMember]
        public int Status { get; set; }
       
        public UserSubscription()
        {
            Id = Guid.NewGuid();
            Details = new UserSubscriptionDetails();
        }
    }
}
