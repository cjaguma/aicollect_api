using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class dloUserRight
    {
        #region Members
        private dloUserRights _rights;
        private dloDataApplication _application;
        #endregion
        #region Properties
        public string Id { get; set; }
        public string ObjectName { get; set; }
        private PermissionTypes _permissions;
        public int PermissionType { get; set; }
        public PermissionTypes Permissions
        { 
            get
            {
                return _permissions;
            }
            set
            {
                if(_permissions!=value)
                {
                    _permissions = value;
                    EditMode = ObjectStates.Modified;
                    //OnPropertyChanged("Permissions");
                }
            }
        }
        public ObjectStates EditMode { get; internal set; }
        public dloUser User { get { return _rights.User; } }
        public dloUserGroup Group { get { return _rights.Group; } }
        public UserRightsTypes UserRightType { get { return _rights.UserRightsType; } }
        #endregion
        internal dloUserRight(dloUserRights rights)
        {
            Id = Guid.NewGuid().ToString();
            _rights = rights;
            _application = _rights.Application;
            EditMode = ObjectStates.Added;
        }

        public void Save()
        {
            string sql = "";

            string object_id = _rights.UserRightsType == UserRightsTypes.Group ? _rights.Group.Id : _rights.User.Id;
            int perm_type=_rights.UserRightsType==UserRightsTypes.Group?2:1;
            if(EditMode==ObjectStates.Added)
            {
                sql = string.Format("INSERT INTO dsto_permissions(guid,object_id,objectname,permission,permission_type) VALUES('{0}','{1}','{2}','{3}',{4})",Id,object_id,ObjectName,Permissions.ToString(),perm_type);
            }
            else if(EditMode ==ObjectStates.Modified || EditMode==ObjectStates.None)
            {
                sql = string.Format("UPDATE dsto_permissions SET permission='{3}' WHERE guid='{0}' AND object_id='{1}' AND objectname='{2}' AND permission_type={4}", Id,object_id, ObjectName,Permissions.ToString(), perm_type);
            }

            int res =_application.DbInfo.ExecuteQuery(sql);
            if (res == -2)
                throw new Exception("Failed to save permission");

            EditMode = ObjectStates.None;
        }

    }

    public enum UserRightsTypes
    {
        User,
        Group
    }
}
