
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core.Collections
{
    public class Reports : AiCollectObject, IEnumerable<Report>
    {
        private List<Report> _reports;

        public Reports()
        {
            _reports = new List<Report>();
        }

        public Report Add()
        {
            Report report = new Report();
            _reports.Add(report);
            return report;
        }

        public override void Cancel()
        {
            
        }

        public IEnumerator<Report> GetEnumerator()
        {
            foreach (var b in _reports)
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
