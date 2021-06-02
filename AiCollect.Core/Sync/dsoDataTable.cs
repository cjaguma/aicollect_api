using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace AiCollect.Core.Sync
{
    [DataContract]
    public class dsoDataTable
    {
        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public dsoDataColumns Columns { get; set; }

        [DataMember]
        public dsoDataRows Rows { get; set; }

        public dsoDataTable()
        {
            Columns = new dsoDataColumns();
            Rows = new dsoDataRows(this);
        }

        public IEnumerable<dsoDataRow> GetErrors()
        {
            List<dsoDataRow> errors = new List<dsoDataRow>();
            foreach(dsoDataRow dr in Rows)
            {
                if (dr.HasErrors)
                    errors.Add(dr);
            }

            return errors;
        }

        public dsoDataColumn FindColumn(String fieldName)
        {
            foreach (dsoDataRow row in this.Rows)
            {
                return row.Columns[fieldName];
            }
            return null;
        }

        public string WriteConflictError()
        {

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings sets = new XmlWriterSettings();
            sets.CheckCharacters = true;
            sets.ConformanceLevel = ConformanceLevel.Document;

            using (XmlWriter writer = XmlWriter.Create(sb, sets))
            {
                writer.WriteStartElement("dsoDataTable");
                writer.WriteElementString("Key", this.Key);
                writer.WriteElementString("TableName", this.TableName);
                writer.WriteElementString("deviceImei", "device-id");
                this.Rows.writeXml(writer);
                writer.WriteEndElement();
            }

            return sb.ToString();
        }
 
    }
}