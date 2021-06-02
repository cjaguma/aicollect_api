using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCollect.Core;
using System.Data;
using AiCollect.Data.Services;

namespace AiCollect.Data.Providers
{
    public class UserRightProvider : Provider
    {
        public UserRightProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        private void InitUserRight(UserRight userRight, DataRow row)
        {
            userRight.Key = row["guid"].ToString();
            userRight.OID = int.Parse(row["oid"].ToString());
            userRight.ObjectName = row["objectname"].ToString();
            userRight.ObjectType = (ObjectType)Enum.Parse(typeof(ObjectType), row["objecttype"].ToString());

            userRight.PrimaryKey = row["primarykey"].ToString();
            userRight.SecondaryKey = row["secondarykey"].ToString();
            userRight.UserId = Convert.ToInt32(row["yref_user"]);
        }

        public UserRight UserRight(string permission_id)
        {
            try
            {
                string query = $"select * from dsto_userright where yref_permission='{permission_id}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    UserRight userRight = new UserRights(null).Add();
                    InitUserRight(userRight, table.Rows[0]);

                    return userRight;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public UserRight GetUserRight(string userRightKey)
        {
            try
            {
                string query = $"select * from dsto_userright where guid='{userRightKey}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    UserRight userRight = new UserRights(null).Add();
                    if (table.Rows[0]["yref_configuration"] != DBNull.Value)
                        userRight.Configuration.OID = int.Parse(table.Rows[0]["yref_configuration"].ToString());

                    InitUserRight(userRight, table.Rows[0]);
                    return userRight;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public UserRights GetUserRightByUserId(int UserId)
        {
            UserRights userRights = new UserRights(null);
            try
            {
                string query = $"select * from dsto_userright where yref_user='{UserId}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        UserRight userRight = userRights.Add();
                        if (row["yref_configuration"] != DBNull.Value)
                        {
                            userRight.Configuration = new Configuration();
                            userRight.Configuration.OID = int.Parse(row["yref_configuration"].ToString());
                        }

                        InitUserRight(userRight, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userRights;
        }

        public override bool Save(AiCollectObject obj)
        {
            try
            {
                UserRight userRight = obj as UserRight;
                string query = string.Empty;
                var exists = RecordExists("dsto_userright", userRight.Key);
                if (!exists)
                    query = $"INSERT INTO dsto_userright(guid,objectname,objecttype,primarykey,yref_user,yref_configuration) values('{userRight.Key}','{userRight.ObjectName}','{(int)userRight.ObjectType}','{userRight.PrimaryKey}', '{userRight.UserId}', '{userRight.Configuration.OID}')";
                else
                    query = $"UPDATE dsto_userright set objecttype='{(int)userRight.ObjectType}', " +
                        $"objectname='{userRight.ObjectName}', yref_user='{userRight.UserId}', deleted='{userRight.Deleted}' " +
                        $"where guid='{userRight.Key}'";

                if( (DbInfo.ExecuteNonQuery(query) > -1))
                {
                    foreach(var userPermission in userRight.UserPermissions)
                        new UserPermissionProvider(DbInfo).Save(userPermission);                
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteUserRight(int oid)
        {
            string query = $"delete from dsto_userright where oid='{oid}'";
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }
    }
}
