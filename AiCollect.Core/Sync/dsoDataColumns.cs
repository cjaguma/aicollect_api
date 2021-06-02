using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace AiCollect.Core
{
    public class dsoDataColumns : ObservableCollection<dsoDataColumn>
    {
        public dsoDataColumns()
        {
           
        }

        public dsoDataColumn this[string name]
        {
            get
            {
                return this.FirstOrDefault(sc => sc.ColumnName == name);
            }
        }

        public dsoDataColumn ByKey(string key)
        {
            return this.FirstOrDefault(sc => sc.Key == key);
        }

        public dsoDataColumn Add()
        {
            dsoDataColumn dc = new dsoDataColumn();
            base.Add(dc);
            return dc;
        }

        public void writeXml(XmlWriter writer)
        {
            writer.WriteStartElement("mdlDataColumns");
            foreach (dsoDataColumn c in this)
            {
                if (!c.ColumnName.Equals("conflict_type"))
                {
                    if (!c.ColumnName.Equals("master_row_oid"))
                    {
                        writer.WriteStartElement("dsoDataColumn");
                        c.writeXml(writer);
                        writer.WriteEndElement();
                    }
                }
            }
            writer.WriteEndElement();

        }
    }
}