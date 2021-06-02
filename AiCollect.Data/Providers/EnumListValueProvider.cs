using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class EnumListValueProvider : Provider
    {
        public EnumListValueProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public EnumListValue GetEnumList(string id)
        {
            try
            {
                string query = $"select * from dsto_enumListValues where guid='{id}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0 && table.Rows.Count == 1)
                {
                    System.Data.DataRow row = table.Rows[0];
                    EnumListValue enumListValue = new EnumListValue(null);
                    InitEnumListValue(enumListValue, row);
                    return enumListValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private void InitEnumListValue(EnumListValue enumListValue, DataRow row)
        {
            try
            {
                enumListValue.Key = row["guid"].ToString();
                enumListValue.CreatedBy = row["created_by"].ToString();
                enumListValue.OID = int.Parse(row["oid"].ToString());
                enumListValue.Description = row["Description"].ToString();
                enumListValue.Deleted = bool.Parse(row["deleted"].ToString());
                enumListValue.Code = int.Parse(row["Code"].ToString());
                enumListValue.EnumListId = row["enumListId"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EnumListValues GetEnumListValue(string key)
        {
            EnumListValues enumListValues = new EnumListValues();
            try
            {
                string query = $"select * from dsto_enumListValues where enumListId = '{key}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                    foreach (DataRow row in table.Rows)
                    {
                        EnumListValue enumListValue = enumListValues.Add();
                        InitEnumListValue(enumListValue, row);
                    }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return enumListValues;
        }

        public override bool Save(AiCollectObject obj)
        {
            EnumListValue enumListValue = obj as EnumListValue;
            string query = string.Empty;
            var exists = RecordExists("dsto_enumListValues", enumListValue.Key);
            if (!exists)
            {
                query = $"insert into dsto_enumListValues(guid,Description,Code,created_by,enumListId) values('{enumListValue.Key}','{enumListValue.Description}','{enumListValue.Code}','Admin','{enumListValue.EnumListId}')";
            }
            else
            {
                //update
                query = $"UPDATE dsto_enumListValues SET " +
                    $"Description='{enumListValue.Description}', " +
                    $"Code='{enumListValue.Code}', " +
                    $"Deleted = '{enumListValue.Deleted}' " +
                    $"WHERE guid='{enumListValue.Key}'";
            }

            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        public bool DeleteEnumValue(string key)
        {
            string query = $"delete from dsto_enumListValues where guid='{key}'";

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }
    }
}
