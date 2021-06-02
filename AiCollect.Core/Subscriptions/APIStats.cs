using System;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class APIStats
    {
        public int Id { get; set; }

        [DataMember]
        public string ProjectId { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string Device { get; set; }

        [DataMember]
        public string Platform { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string Resource { get; set; }

        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public DateTime CallDate { get; set; }


        /// <summary>
        /// Generates a sql statement for an insert
        /// </summary>
        /// <returns></returns>
        public string InsertQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO APIStats (ProjectId, UserId, Device, Platform, Action, Resource, Result)");
            sb.Append("VALUES");
            sb.Append(string.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}'", ProjectId, UserId, Device,
                Platform, Action, Resource, Result));


            return sb.ToString();
        }

    }
}