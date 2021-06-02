using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class EnumListProvider : Provider
    {
        public EnumListProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }


        public EnumList GetEnumList(int id, EnumListTypes enumListType, bool fromQuestionaire = false)
        {
            try
            {
               
                EnumList enumList = new EnumList(null);
                string query = string.Empty;
                if (enumListType == EnumListTypes.Question)
                    query = $"select * from dsto_enumLists where questionId='{id}' and type='{(int)enumListType}' and deleted=false";
                else
                    query = $"select * from dsto_enumLists where ConfigurationId='{id}' and type='{(int)enumListType}' and deleted=false";

                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                    InitEnumList(enumList, table.Rows[0]);
                
                return enumList;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public EnumList GetEnumList(string id)
        {
            EnumList enumList = new EnumList(null);
            string query = $"select * from dsto_enumLists where guid='{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                InitEnumList(enumList, row);
            }
            return enumList;
        }

        private void InitEnumList(EnumList enumList, DataRow row)
        {
            try
            {
                enumList.Key = row["guid"].ToString();
                enumList.CreatedBy = row["created_by"].ToString();
                enumList.OID = int.Parse(row["oid"].ToString());
                enumList.Deleted = bool.Parse(row["deleted"].ToString());
                enumList.Name = row["Name"].ToString();
                enumList.EnumValues = new EnumListValueProvider(DbInfo).GetEnumListValue(enumList.Key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Save(AiCollectObject obj)
        {
            EnumListValueProvider enumListValueProvider = new EnumListValueProvider(DbInfo);
            EnumList enumList = obj as EnumList;

            var exists = RecordExists("dsto_enumLists", enumList.Key);

            string query = string.Empty;
            if (!exists)
            {
                if (enumList.ConfigurationId > 0)
                    query = $"insert into dsto_enumLists(guid,Name,created_by,type,configurationId) values('{enumList.Key}','{enumList.Name}','Admin','{(int)enumList.EnumListType}','{enumList.ConfigurationId}')";
                else
                    query = $"insert into dsto_enumLists(guid,Name,created_by,type,QuestionId) values('{enumList.Key}','{enumList.Name}','Admin','{(int)enumList.EnumListType}','{enumList.QuestionId}')";
            }
            else
            {
                //update
                query = $"UPDATE dsto_enumLists SET " +
                    $"Name='{enumList.Name}', " +
                    $"Deleted='{enumList.Deleted}' " +
                    $"WHERE guid = '{enumList.Key}'";
            }

            if (DbInfo.ExecuteNonQuery(query) > -1)
            {
                foreach (var en in enumList.EnumValues)
                {
                    en.EnumListId = enumList.Key;
                    var isSaved = enumListValueProvider.Save(en);
                }
                return true;
            }

            return false;

        }
        public bool DeleteEnumList(string key)
        {
            string query = $"delete from dsto_enumLists where guid='{key}'";

            EnumList enumList = GetEnumList(key);
            foreach (var enumValue in enumList.EnumValues)
            {
                new EnumListValueProvider(DbInfo).DeleteEnumValue(enumValue.Key);
            }
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }

    }
}
