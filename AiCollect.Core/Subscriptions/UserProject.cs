using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class UserProject
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public int DatabaseSize { get; set; }

        [DataMember]
        public int APICalls { get; set; }

        [DataMember]
        public List<User> Users { get; set; }

        [DataMember]
        public List<Device> Devices { get; set; }


        public UserProject()
        {
            Users = new List<User>();
            Devices = new List<Device>();
        }
    }
}
