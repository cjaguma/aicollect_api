using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class DataLinkProvider : Provider
    {
        public DataLinkProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        
        public override bool Save(AiCollectObject obj)
        {
            DataLink dataLink = obj as DataLink;
            var exists = RecordExists("dsto_module_link", dataLink.Key);
            return !exists ? Insert(dataLink) : Edit(dataLink);
        }

        public override bool SoftDelete(string table, string key)
        {
            return base.SoftDelete(table, key);
        }

        public override bool SoftDelete(string table, int oid)
        {
            return base.SoftDelete(table, oid);
        }


        private bool Edit(DataLink dataLink)
        {
            string query = $"UPDATE dsto_module_link SET " +
                    $"name='{dataLink.Name}', " +
                    $"parentobject = '{dataLink.OriginObject}', " +
                    $"referredobject= '{dataLink.ReferredObject}', " +
                    $"deleted='{dataLink.Deleted}', " +
                    $"WHERE guid='{dataLink.Key}'";
            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        private bool Insert(DataLink dataLink)
        {
            try
            {
                string query = $"INSERT INTO dsto_module_link (guid,created_by,name,parentobject,referredobject) values('{dataLink.Key}','Admin', '{dataLink.Name}','{dataLink.OriginObject}','{dataLink.ReferredObject}')";
                if (DbInfo.ExecuteNonQuery(query) > 0)
                {

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetDataLink(DataLink dataLink, DataRow row)
        {
            dataLink.Key = row["guid"].ToString();
            dataLink.CreatedBy = row["created_by"].ToString();
            dataLink.OID = int.Parse(row["oid"].ToString());
            dataLink.Name = row["name"].ToString();
            dataLink.OriginObject = row["parentObject"].ToString();
            dataLink.ReferredObject = row["referredobject"].ToString();
        }

        public DataLink RetrieveDataLink(int id)
        {
            DataLink module = new DataLink();
            try
            {
                string query = string.Format("select * from dsto_module_link where oid='{0}'", id);
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    SetDataLink(module, row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return module;
        }

        public DataLinks RetrieveDataLinks()
        {
            DataLinks links = new DataLinks();
            try
            {
                string query = "select * from dsto_module_link ";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        DataLink dataLink = links.Add();
                        SetDataLink(dataLink, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return links;
        }

        public DataLinks RetrieveDataLinksByOrigin(string originKey)
        {
            DataLinks links = new DataLinks();
            try
            {
                string query = $"select * from dsto_module_link where parentobject='{originKey}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        DataLink dataLink = links.Add();
                        SetDataLink(dataLink, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return links;
        }

        public void GetModuleLinks(Module module)
        {
           
            try
            {
                string query = $"select * from dsto_module_link where parentobject='{module.Key}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        DataLink dataLink = module.DataLinks.Add();
                        SetDataLink(dataLink, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

    }
}
