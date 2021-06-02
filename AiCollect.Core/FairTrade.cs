using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class FairTrade : Certification
    {      
        public FairTrade(AiCollectObject parent):base(parent)
        {
            CerificationType = CertificationTypes.FairTrade;
        }

        public override void Cancel()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void Validate()
        {
            
        }
    }
}
