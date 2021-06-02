using AiCollect.Core;
using AiCollect.Core.Sync;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace AiCollect.Data.Sync
{

    public enum SyncResult
    {
        Unknown = 0,
        Success = 1,
        Failed = 2,
        Conflict = 4
    }

    [DataContract]
    public class dsoDataRow
    {
        public object[] ItemArray { get; private set; }

        [DataMember]
        public string Key { get; set; }

        public dsoDataColumns Columns { get { return Table.Columns; } }

        public dsoDataTable Table { get; private set; }

        [DataMember]
        public List<ColumnValueWrapper> ColumnValues { get; private set; }

        [DataMember]
        public SyncResult SyncResult { get; set; }

        [DataMember]
        public bool HasErrors { get; internal set; }

        [DataMember]
        public string RowError { get; set; }

        internal dsoDataRow(dsoDataTable parent)
        {
            SyncResult = Sync.SyncResult.Unknown;

            ColumnValues = new List<ColumnValueWrapper>();

            Table = parent;

            ItemArray = new object[Table.Columns.Count];

            //fill ths values
            foreach (dsoDataColumn c in parent.Columns)
                ColumnValues.Add(new ColumnValueWrapper() { Index = GetColumnIndex(c), Column = c });
        }

        private int GetColumnIndex(dsoDataColumn column)
        {
            return Table.Columns.IndexOf(column);
        }
        public object this[string columnName]
        {
            get
            {
                ColumnValueWrapper col = ByName(columnName);
                if (col != null)
                    return col.Data;
                else
                    return null;
            }
            set
            {
                ColumnValueWrapper col = ByName(columnName);
                if (col != null)
                {
                    col.Data = value;

                    //Save data in th eitemarray
                    int index = GetColumnIndex(col.Column);
                    ItemArray[index] = value;

                }
                else
                    throw new Exception(columnName + " Column doesn't exist");

            }
        }


        public ColumnValueWrapper ByName(string name)
        {
            return ColumnValues.Find(x => x.Column.ColumnName == name);
        }

        public void writeXml(XmlWriter writer)
        {
            Columns.writeXml(writer);
        }

        public string WriteSyncError()
        {


            StringBuilder sb = new StringBuilder();
            XmlWriterSettings sets = new XmlWriterSettings();
            sets.CheckCharacters = true;
            sets.ConformanceLevel = ConformanceLevel.Document;

            using (XmlWriter writer = XmlWriter.Create(sb, sets))
            {
                writer.WriteStartElement("dsoDataTable");
                writer.WriteElementString("Key", Key);
                writer.WriteElementString("TableName", Table.TableName);
                writer.WriteElementString("deviceImei", "device-id");

                //writting dsorow
                writer.WriteStartElement("dsoDataRow");
                this.writeXml(writer);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

           
            return sb.ToString();

        }

    }

    [DataContract]
    public class ColumnValueWrapper
    {
        public int Index { get; set; }

        [DataMember]
        public string Key { get { return Column.Key; } }

        [DataMember]
        public string ColumnName { get { return Column.ColumnName; } }

        public dsoDataColumn Column { get; set; }

        [DataMember]
        public object Data { get; set; }
    }
}