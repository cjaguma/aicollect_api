using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Billing : AiCollectObject
    {
        [DataMember]
        public DateTime BillingDate { get; set; }

        [DataMember]
        public string InvoiceNo { get; set; }

        [DataMember]
        public string Bill { get; set; }

        [DataMember]
        public PaymentStatus PaymentStatus { get; set; }

        [DataMember]
        public Client Client { get; set; }

        [DataMember]
        public Package Package { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        public Billing()
        {
        }

        public Billing(AiCollectObject parent) : base(parent)
        {
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

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);

            if (obj["CreatedBy"] != null && ((JValue)obj["CreatedBy"]).Value != null)
                CreatedBy = ((JValue)obj["CreatedBy"]).Value.ToString();

            if (obj["Bill"] != null && ((JValue)obj["Bill"]).Value != null)
                Bill = ((JValue)obj["Bill"]).Value.ToString();

            if (obj["BillingDate"] != null && ((JValue)obj["BillingDate"]).Value != null)
                BillingDate = DateTime.Parse(((JValue)obj["BillingDate"]).Value.ToString());

            if (obj["PaymentStatus"] != null && ((JValue)obj["PaymentStatus"]).Value != null)
                PaymentStatus = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), ((JValue)obj["PaymentStatus"]).Value.ToString());

            if (obj["InvoiceNo"] != null && ((JValue)obj["InvoiceNo"]).Value != null)
                InvoiceNo = ((JValue)obj["InvoiceNo"]).Value.ToString();

            Client.ReadJson(obj);
            Package.ReadJson(obj);
        }
    }
}
