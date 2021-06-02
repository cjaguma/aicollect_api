using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class Report
    {
        public Questionaire Questionaire { get; set; }
        public FieldInspection FieldInspection { get; set; }
        public Certification Certification { get; set; }
        public Purchase Purchase { get; set; }
        public Training Training { get; set; }
    }
}
