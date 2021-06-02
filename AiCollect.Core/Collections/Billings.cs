
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core.Collections
{
    public class Billings : AiCollectObject, IEnumerable<Billing>
    {
        private List<Billing> _billings;

        public Billings()
        {
            _billings = new List<Billing>();
        }

        public Billing Add()
        {
            Billing billing = new Billing();
            _billings.Add(billing);
            return billing;
        }

        public override void Cancel()
        {
            
        }

        public IEnumerator<Billing> GetEnumerator()
        {
            foreach (var b in _billings)
                yield return b;
        }

        public override void Update()
        {
            
        }

        public override void Validate()
        {
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
