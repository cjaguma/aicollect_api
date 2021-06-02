using System.Runtime.Serialization;
using System.Xml;

namespace AiCollect.Core
{
    [DataContract]
    public class dsoDataColumn
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string ColumnName { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public DataTypes DataType { get; set; }

        public dsoDataColumn()
        {
            DataType = DataTypes.Alphanumeric;
        }

        public void writeXml(XmlWriter writer)
        {
            writer.WriteElementString("Key", Key);
            writer.WriteElementString("ColumnName", ColumnName);
            writer.WriteElementString("Value", Value);
            writer.WriteElementString("DataType", DataType.ToString());
        }

    }
}