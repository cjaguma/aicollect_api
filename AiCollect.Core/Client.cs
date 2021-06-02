using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Client : AiCollectObject, IUserParent
    {
        private string _key;
        private string _name;
        private string _email;
        private string _contact;
        private string _location;
        private string _logo;
        public new Clients Parent
        {
            get
            {
                return base.Parent as Clients;
            }
        }

        [DataMember]
        public string Name { get => _name; set => _name = value; }
        [DataMember]
        public string Email { get => _email; set => _email = value; }
        [DataMember]
        public string Contact { get => _contact; set => _contact = value; }
        [DataMember]
        public string Logo { get => _logo; set => _logo = value; }
        [DataMember]
        public string Location { get => _location; set => _location = value; }
        [DataMember]
        public bool Deleted { get; set; }
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
        public bool IsParent
        {
            get
            {
                return true;
            }
        }

        [DataMember]
        public Package Package { get; set; }

        [DataMember]
        public Users Users { get; set; }

        [DataMember]
        public List<Configuration> Configurations { get; set; }

        public Client(AiCollectObject parent) : base(parent)
        {
            Init();
        }

        public Client() : base()
        {
            Init();
        }

        private void Init()
        {
            Users = new Users(this);
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

        public int Compare(Client other)
        {
            var chkEmail = this.Email == other.Email;
            var chkName = this.Name == other.Name;
            var chkLocation = this.Location == other.Location;
            var chkContact = this.Contact == other.Contact;
            return chkEmail && chkName && chkLocation && chkContact ? 1 : 0;
        }
    }
}
