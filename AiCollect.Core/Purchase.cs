using AiCollect.Core.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(PurchaseConverter))]
    public class Purchase : AiCollectObject
    {
        [DataMember]
        public int Product { get; set; }

        [DataMember]
        public int Station { get; set; }

        [DataMember]
        public new string Key
        {
            get
            {
                return base.Key;
            }
            set
            {
                base.Key = value;
            }
        }
        [DataMember]
        public new int OID
        {
            get
            {
                return base.OID;
            }
            set
            {
                base.OID = value;
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

        #region members
        private decimal _price;
        private decimal _qty;
        private DateTime _dateOfPurchase;
        private Questionaire _farmer;
        private string _lotid;
        #endregion

        #region Properties

        [DataMember]
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public decimal Quantity
        {
            get
            {
                return _qty;
            }
            set
            {
                if (_qty != value)
                {
                    _qty = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public DateTime DateOfPurchase
        {
            get
            {
                return _dateOfPurchase;
            }
            set
            {
                if (_dateOfPurchase != value)
                {
                    _dateOfPurchase = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        [DataMember]
        public decimal TotalAmount
        {
            get
            {
                return _price * _qty;
            }
        }

        [DataMember]
        public string Farmer
        {
            get; set;
        }
        [DataMember]
        public string Lotid { get => _lotid; set => _lotid = value; }

        [DataMember]
        public string ConfigurationId { get; set; }

        #endregion

        public Purchase()
        {
            Init();
        }

        public Purchase(AiCollectObject parent) : base(parent)
        {
            Init();
        }

        private void Init()
        {
            //Product = new Product(this);
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

            if (obj["Price"] != null && ((JValue)obj["Price"]).Value != null)
                Price = decimal.Parse(((JValue)obj["Price"]).Value.ToString());

            if (obj["Quantity"] != null && ((JValue)obj["Quantity"]).Value != null)
                Quantity = int.Parse(((JValue)obj["Quantity"]).Value.ToString());

            if (obj["Product"] != null && ((JValue)obj["Product"]).Value != null)
                Product = int.Parse(((JValue)obj["Product"]).Value.ToString());

            if (obj["Lotid"] != null && ((JValue)obj["Lotid"]).Value != null)
                Lotid = ((JValue)obj["Lotid"]).Value.ToString();

            if (obj["Farmer"] != null && ((JValue)obj["Farmer"]).Value != null)
                Farmer = ((JValue)obj["Farmer"]).Value.ToString();

            if (obj["Station"] != null && ((JValue)obj["Station"]).Value != null)
                Station = int.Parse(((JValue)obj["Station"]).Value.ToString());

            if (obj["DateOfPurchase"] != null && ((JValue)obj["DateOfPurchase"]).Value != null)
                DateOfPurchase = DateTime.Parse(((JValue)obj["DateOfPurchase"]).Value.ToString());

            if (obj["ConfigurationId"] != null && ((JValue)obj["ConfigurationId"]).Value != null)
                ConfigurationId = ((JValue)obj["ConfigurationId"]).Value.ToString();
        }

    }
}
