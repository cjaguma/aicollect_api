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
    public class UserProvider : Provider
    {
        public UserProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public User GetUser(int id)
        {
            
            string query = $"select * from dsto_user where oid = '{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                User user = new User();
                InitUser(user, table.Rows[0]);
                return user;
            }
           
            return null;
        }

        public User GetUser(string email)
        {
            string query = $"select * from dsto_user where email = '{email}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                User user = new User();
                InitUser(user, row);
                user.Usercode = new Random().Next(100000).ToString();
                return user;
            }
            return null;
        }

        private void InitUser(User user, DataRow row)
        {
            user.Key = row["guid"].ToString();
            user.OID = int.Parse(row["oid"].ToString());
            user.Firstname = row["Firstname"].ToString();
            user.Lastname = row["Lastname"].ToString();
            user.Email = row["Email"].ToString();
            user.CreatedBy = row["created_by"].ToString();
            user.IsAdmin = bool.Parse(row["IsAdmin"].ToString());
            user.UserType = (UserTypes)Enum.Parse(typeof(UserTypes), row["UserType"].ToString());
            user.Password = row["Password"].ToString();
            user.Usercode = row["Usercode"].ToString();
            user.Deleted = bool.Parse(row["deleted"].ToString());
            user.Enabled = bool.Parse(row["enabled"].ToString());
            user.UserName = row["UserName"].ToString();
            user.UserRights = new UserRightProvider(DbInfo).GetUserRightByUserId(user.OID);

            if (row["client_Id"] != DBNull.Value)
                user.ClientId = row["client_Id"].ToString();
        }

        public string GetCreatedByUser(string key)
        {
            string query = $"select * from dsto_user where guid = '{key}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                return table.Rows[0]["Firstname"].ToString() +" " + table.Rows[0]["Lastname"].ToString();
            }
            return null;
        }

        public Users ClientUsers(int clientId)
        {
            Users users = new Users(null);
            try
            {
                string query = $"select * from dsto_user where client_id = '{clientId}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        User user = new User();
                        InitUser(user, row);
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public Users ClientAdmins(int clientId)
        {
            Users users = new Users(null);
            try
            {
                string query = $"select * from dsto_user where isadmin=1 and client_id = '{clientId}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        User user = new User();
                        InitUser(user, row);
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public bool AddConfigurationUser(User user)
        {
            try
            {
                var cmd = DbInfo.CreateDbCommand();
                cmd.CommandText = $"select dsto_user.* from dsto_user inner join dsto_configurationuser on dsto_user.oid = dsto_configurationuser.yref_user where dsto_configurationuser.yref_configuration='{user.ConfigurationId}' and dsto_configurationuser.yref_user='{user.OID}'";
                var oid = DbInfo.ExecuteScalar(cmd);
                string Query = string.Empty;
                if (oid == null)
                    Query = $"INSERT INTO dsto_configurationuser(yref_user,yref_configuration) values('{user.OID}','{user.ConfigurationId}')";
                else
                    Query = $"UPDATE dsto_configurationuser SET deleted='{user.Deleted}' where yref_user='{user.OID}' and yref_configuration='{user.ConfigurationId}'";

                if (DbInfo.ExecuteNonQuery(Query) > -1)
                {
                    if (oid == null)
                    {
                        var innerHtml = "   <h5> Hello " + user.Firstname +",</h5>"
                                      + "   <p>" + "Your account has been added to a configuration, you're now eligible to access our mobile application" + "</p>"
                                      + "   <h5 style='margin-top: 85px;'> Best Regards,</h5>"
                                      + "   <span> AICollect Team </span>";
                        MailService.SendMail(user.Email, true, "Configuration Registration", innerHtml);
                    }

                    foreach (UserRight userRight in user.UserRights)
                    {
                        userRight.Configuration = new Configuration();
                        userRight.Configuration.OID = int.Parse(user.ConfigurationId);
                        userRight.UserId = user.OID;
                        new UserRightProvider(DbInfo).Save(userRight);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public Users ConfigurationUsers(int configuration_Id)
        {
            Users users = new Users(null);
            try
            {
                string query = $"select dsto_user.* from dsto_user inner join dsto_configurationuser on dsto_user.oid = dsto_configurationuser.yref_user where dsto_configurationuser.yref_configuration='{configuration_Id}' and dsto_user.deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        User user = new User();
                        InitUser(user, row);
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public User AuthoriseUser(Credentials credentials)
        {
            User user = new User();
            try
            {
                string query = $"select * from dsto_user where username='{credentials.Username}' and password='{credentials.Password}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0 && table.Rows.Count == 1)
                {
                    DataRow row = table.Rows[0];
                    InitUser(user, row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }

        public Users GetSuperAdmins()
        {
            Users users = new Users(null);
            try
            {
                string query = $"select * from dsto_user where client_id IS NULL";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        User user = new User();
                        InitUser(user, row);
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public bool AddOrUpdateUser(User user)
        {
            try
            {
                var cmd = DbInfo.CreateDbCommand();
                cmd.CommandText = string.Format("select guid from dsto_user where email='{0}'", user.Email);
                var guid = DbInfo.ExecuteScalar(cmd);

                string query = string.Empty;
                if (guid == null)
                    if (!string.IsNullOrEmpty(user.ClientId))
                        query = $"INSERT INTO dsto_user(guid,firstname,lastname,username,email,isadmin,password,client_id,userType) values('{user.Key}','','','','{user.Email}','{user.IsAdmin}','','{user.ClientId}','{(int)user.UserType}')";
                    else
                        query = $"INSERT INTO dsto_user(guid,firstname,lastname,username,email,isadmin,password,userType) values('{user.Key}','','','','{user.Email}','{user.IsAdmin}','','{(int)user.UserType}')";
                else
                    query = $"update dsto_user set firstname='{user.Firstname}', " +
                        $"lastname='{user.Lastname}', username='{user.UserName}', password='{user.Password}', deleted='{user.Deleted}', usercode = '{user.Usercode}', enabled = '{user.Enabled}'  where guid='{guid}'";

                if (DbInfo.ExecuteNonQuery(query) > -1)
                {
                    cmd.CommandText = string.Format("select oid from dsto_User where email='{0}'", user.Email);
                    var oid = DbInfo.ExecuteScalar(cmd);
                    if (guid == null)
                    {
                        var innerHtml = "";
                        if(user.IsAdmin)
                            innerHtml = "   <h1 style='text-align: center;color: #299def;'>Welcome aboard</h1>"
                                      + "   <h5> Hello " + user.Firstname + " " + user.Lastname + ",</h5>"
                                      + "   <p>We’re excited you're joining us! You have been invited by Aicollect as a System Admin or owner of the account.<p>"
                                      + "   <p>" + "Please click on the link below to complete your account registration" + "</p>"
                                      + "   <a href='" + $"http://www.aicollectapp.com/#/registration?Id={oid}" + "'> Click here to proceed </a>"
                                      + "   <h5 style='margin-top: 85px;'> Best Regards,</h5>"
                                      + "   <span> AICollect Team </span>";
                        else
                            innerHtml = "   <h5> Hello " + user.Firstname + " " + user.Lastname + ",</h5>"
                                      + "   <p>" + "Please click on the link below to complete your account registration" + "</p>"
                                      + "   <a href='" + $"http://www.aicollectapp.com/#/registration?Id={oid}" + "'> Click here to proceed </a>"
                                      + "   <h5 style='margin-top: 85px;'> Best Regards,</h5>"
                                      + "   <span> AICollect Team </span>";

                        MailService.SendMail(user.Email, true, "Account Registration", innerHtml);
                    }

                    foreach (UserRight userRight in user.UserRights)
                        new UserRightProvider(DbInfo).Save(userRight);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User VerifyUser(User user)
        {
            string query = $"select * from dsto_user where email='{user.Email}' and usercode='{user.Usercode}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                InitUser(user, row);
                return user;
            }
            return null;
        }

        public bool ResetPassword(User user)
        {
            string query = $"update dsto_user set password='{user.Password}' where guid='{user.Key}'";
            return (DbInfo.ExecuteNonQuery(query) > -1);
        }

        public bool DeleteUser(int oid)
        {
            string query = $"delete from dsto_user where oid='{oid}'";

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }

        public bool DeleteConfigurationUser(User user)
        {
            string query = $"delete from dsto_configurationuser where yref_user='{user.OID}' and yref_configuration='{user.ConfigurationId}'";

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }
    }
}
