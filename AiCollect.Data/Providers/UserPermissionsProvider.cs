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
    public class UserPermissionProvider : Provider
    {
        public UserPermissionProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public UserPermission GetUserPermission(int id)
        {
            string query = $"select * from dsto_userpermission where oid = '{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                UserPermission userPermission = new UserPermissions(null).Add();
                InitUserPermission(userPermission, row);
                return userPermission;
            }
            return null;
        }

        public UserPermission GetUserPermissionByUserRightId(string userRightId)
        {
            string query = $"select * from dsto_userpermission where yref_userRight = '{userRightId}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                UserPermission userPermission = new UserPermissions(null).Add();
                InitUserPermission(userPermission, row);
                return userPermission;
            }
            return null;
        }

        private void InitUserPermission(UserPermission userPermission, DataRow row)
        {
            try
            {
                userPermission.Key = row["guid"].ToString();
                userPermission.OID = int.Parse(row["oid"].ToString());
                userPermission.Permission = (PermisionType)Enum.Parse(typeof(PermisionType), row["permission"].ToString());
                userPermission.UserRight = new UserRightProvider(DbInfo).GetUserRight(Convert.ToString(row["yref_userRight"]));
            }
            catch(Exception ex)
            {

            }
        }

        public UserPermissions UserPermissions(UserRight userRight)
        {
            UserPermissions userPermissions = new UserPermissions(null);
            try
            {
                string query = $"select * from dsto_userpermission where yref_userRight = '{userRight.OID}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        UserPermission userPermission = userPermissions.Add();
                        InitUserPermission(userPermission, row);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return userPermissions;
        }

        public override bool Save(AiCollectObject obj)
        {
            try
            {
                UserPermission userPermission = obj as UserPermission;
                string query = string.Empty;
                var exists = RecordExists("dsto_userpermission", userPermission.Key);
                if (!exists)
                    query = $"INSERT INTO dsto_userpermission(guid,permission,permissionObject,yref_userRight) values('{userPermission.Key}', '{(int)userPermission.Permission}',{(int)userPermission.PermissionObject},'{userPermission.UserRight.Key}')";
                else
                    query = $"UPDATE dsto_userpermission set permission='{(int)userPermission.Permission}', permissionObject='{(int)userPermission.PermissionObject}',deleted='{userPermission.Deleted}' where guid='{userPermission.Key}'";

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteUserPermission(int oid)
        {
            string query = $"delete from dsto_userpermission where oid='{oid}'";

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }
    }
}
