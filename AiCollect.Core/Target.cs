using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    public class Target
    {
        [DataMember]
        public Section Section { get; set; }

        [DataMember]
        public SubSection SubSection { get; set; }

        [DataMember]
        public Question Question { get; set; }
    }
}
