using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace AiCollect.Core.Sync
{
    public class dsoDataRows : ObservableCollection<dsoDataRow>
    {
        internal dsoDataTable Table { get; private set; }

        public dsoDataRows(dsoDataTable parent)
        {
            Table = parent;
        }

        public dsoDataRow Add()
        {
            dsoDataRow row = new dsoDataRow(Table);
            base.Add(row);
            return row;
        }

        public void writeXml(XmlWriter writer)
        {
            writer.WriteStartElement("dsoDataRows");
            foreach (dsoDataRow row in this)
            {
                writer.WriteStartElement("dsoDataRow");
                row.writeXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

        }
    }
}