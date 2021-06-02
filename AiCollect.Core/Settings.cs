using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Settings
    {
        private string _webServiceUrl;
        private string _companyName;
        [DataMember]
        public string WebServiceUrl
        {
            get
            {
                return _webServiceUrl;
            }
            set
            {
                if (_webServiceUrl != value)
                {
                    _webServiceUrl = value;
                }
            }
        }
        [DataMember]
        public string CompanyName
        {
            get
            {
                return _companyName;
            }
            set
            {
                if (_companyName != value)
                {
                    _companyName = value;
                }
            }
        }

        public Settings() { }

        public  int CompareTo(Settings other)
        {
            Settings settings = this;
            Settings settings1 = other as Settings;
            var sameName = settings.CompanyName.Equals(settings1.CompanyName);
            var sameUrl = settings.WebServiceUrl.Equals(settings1.WebServiceUrl);
            return sameName ? 1 : 0;
        }

    }
}
