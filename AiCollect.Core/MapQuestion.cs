using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class MapQuestion : Question
    {
        [DataMember]
        public new Answers Answers
        {
            get;
            set;
        }
        public MapQuestion(AiCollectObject parent) : base(parent)
        {
            Answers = new Answers(this);
        }

    }
}
