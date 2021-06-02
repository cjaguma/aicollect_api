using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class OtherQuestion : Question
    {
        public string Answer { get; set; }
        public OtherQuestion(AiCollectObject parent) : base(parent)
        {

        }

        public override void Validate()
        {
            
        }

        public override void Cancel()
        {
           
        }

        public override void Update()
        {
            
        }
    }
}
