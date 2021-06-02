using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class dloUserRights : ObservableCollection<dloUserRight>
    {
        #region Members
        private dloDataApplication _app;
        private dloUser _user;
        private dloUserGroup _group;
        #endregion

        #region Properties
        public bool IsLoaded { get; private set; }
        public UserRightsTypes UserRightsType { get; private set; }
        internal dloDataApplication Application { get { return _app; } }
        internal dloUserGroup Group { get { return _group; } }
        internal dloUser User { get { return _user; } }
        #endregion

        #region Constructors
        internal dloUserRights(dloUser user)
        {
            _user = user;
            _app = _user.Application;
            UserRightsType = UserRightsTypes.User;
        }

        internal dloUserRights(dloUserGroup group)
        {
            _group = group;
            _app = _group.Application;
            UserRightsType = UserRightsTypes.Group;
        }
        #endregion

        #region Methods
        public dloUserRight this[string name]
        {
            get
            {
                var right = this.FirstOrDefault(t => t.ObjectName == name);
                return right;
            }
        }

        public dloUserRight GetUserRight(dloUser user, string name)
        {
            return this.FirstOrDefault(t => t.ObjectName == name && t.User.Id == user.Id);
        }

        public dloUserRight Add()
        {
            dloUserRight right = new dloUserRight(this);

            base.Add(right);

            return right;
        }

        internal void Load()
        {
            string sql;
            if (User != null)
                sql = string.Format("SELECT * FROM dsto_permissions WHERE permission_type={0} and object_id='{1}'", UserRightsType == UserRightsTypes.Group ? 2 : 1, User.Id);
            else
                sql = string.Format("SELECT * FROM dsto_permissions WHERE permission_type={0}", UserRightsType == UserRightsTypes.Group ? 2 : 1);

            DataTable table = new DataTable();

            _app.DbInfo.ExecuteQuery(sql, table);
            foreach (DataRow dr in table.Rows)
            {
                dloUserRight right = Add();
                switch (_app.Provider)
                {
                    case DataProviders.SQL:
                        right.Id = (string)dr["guid"];
                        break;
                    default:
                        right.Id = (string)dr["guid"];
                        break;
                }
                right.ObjectName = (string)dr["objectname"];
                // right.User.Id = (string)dr["object_id"];
                right.Permissions = (PermissionTypes)Enum.Parse(typeof(PermissionTypes), (string)dr["permission"]);
                right.PermissionType = (int)dr["permission_type"];
                right.EditMode = ObjectStates.None;
            }

            IsLoaded = true;
        }

        public void Refresh()
        {
            Clear();
            Load();
        }
        #endregion
    }
}
