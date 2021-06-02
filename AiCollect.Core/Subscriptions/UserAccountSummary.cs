using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class UserAccountSummary
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public Subscription Subscription { get; set; }

        [DataMember]
        public List<UserProject> Projects { get; set; }

        public UserAccountSummary()
        {
            Subscription = new Subscription();
        }
    }
}
