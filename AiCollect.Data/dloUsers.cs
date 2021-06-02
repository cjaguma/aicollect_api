using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public delegate void UserHandler(object sender, EventArgs e);
    public class dloUsers : ObservableCollection<dloUser>
    {
        #region Members
        private dloDataApplication _app;

        #endregion
        #region Properties
        public bool IsLoaded { get; private set; }
        internal dloDataApplication Application { get { return _app; } }
        #endregion

        #region Constructors
        internal dloUsers(dloDataApplication app)
        {
            _app = app;
        }
        #endregion

        public event UserHandler UserAdded;
        public event UserHandler UserRemoved;
        public dloUser Add()
        {
            dloUser u = new dloUser(this);
            u.EditMode = ObjectStates.Added;
            u.UserAdded += U_UserAdded;
            u.UserRemoved += U_UserRemoved;
            base.Add(u);
            return u;
        }

        private void U_UserRemoved(object sender, EventArgs e)
        {
            RemoveUser(sender as dloUser);
            OnUserRemoved(sender, e);        
        }

        private void U_UserAdded(object sender, EventArgs e)
        {
            OnUserAdded(sender, e);
        }

        public bool RemoveUser(dloUser user)
        {
            bool isRemoved = base.Remove(user);           
            return isRemoved;
        }

        internal void Load()
        {

            string sql = "SELECT * FROM dsto_users";
            DataTable table = new System.Data.DataTable();
            _app.DbInfo.ExecuteQuery(sql, table);

            foreach (DataRow dr in table.Rows)
            {
                dloUser user = Add();
                switch (_app.Provider)
                {
                    case DataProviders.SQL:
                        user.Id = ((Guid)dr["guid"]).ToString();
                        break;
                    default:
                        user.Id = (string)dr["guid"];
                        break;
                }
                user.Username = (string)dr["username"];
                if (dr["firstname"] != null && dr["firstname"] != DBNull.Value)
                    user.FirstName = (string)dr["firstname"];
                if (dr["lastname"] != null && dr["lastname"] != DBNull.Value)
                    user.LastName = (string)dr["lastname"];
                if (dr["mobile"] != null && dr["mobile"] != DBNull.Value)
                    user.Mobile = (string)dr["mobile"];
                if (dr["email"] != null && dr["email"] != DBNull.Value)
                    user.Email = (string)dr["email"];

               // if (dr["Password"] != null && dr["Password"] != DBNull.Value)
                 //   user.Password = (string)dr["Password"];
                //user.Status = (int)dr["status"];

                if (dr["photo"] != null && dr["photo"] != DBNull.Value)
                    user.Photo = ((byte[])dr["photo"]).ToImage();
                //Load the user group
                if (dr["YREF_Group"] != null && dr["YREF_Group"] != DBNull.Value)
                {
                    string groupId="";

                    switch (_app.Provider)
                    {
                        case DataProviders.SQL:
                            groupId = ((Guid)dr["YREF_Group"]).ToString();
                            break;
                        default:
                            groupId = (string)dr["YREF_Group"];
                            break;
                    }

                    /*dloUserGroup group = _app.Groups.SingleOrDefault(x => x.Id == groupId);
                    if (group != null)
                        user.Group = group;*/
                }
                user.EditMode = ObjectStates.None;
            }
            IsLoaded = true;
        }

        public void Refresh()
        {
            Clear();
            Load();
        }

        private void OnUserAdded(object sender, EventArgs e)
        {
            if (UserAdded != null)
            {
                UserAdded(sender, e);
            }
        }

        private void OnUserRemoved(object sender, EventArgs e)
        {
            if (UserRemoved != null)
            {
                UserRemoved(sender, e);
            }
        }
    }
}
