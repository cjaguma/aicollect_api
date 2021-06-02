using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class dloUserGroup
    {
        #region Members
        private dloUserGroups _groups;
        private dloDataApplication _app;
        private string _id;
        private string _name;
        private string _description;
        
        #endregion
        #region Properties
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {

                if (_name != value)
                {
                    _name = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
        private ObjectStates _editMode;
        public ObjectStates EditMode
        {
            get
            {
                return _editMode;
            }
            internal set
            {
                if (_editMode != value)
                {
                    if(value<_editMode || _editMode == ObjectStates.None)
                    {
                        _editMode = value;
                    }
                }
            }
        }
        internal dloDataApplication Application { get { return _app; } }
        private dloUserRights _rights;
        public dloUserRights Rights
        {
            get
            {
                if (!_rights.IsLoaded)
                    _rights.Load();
                return _rights;
            }
        }

        internal bool IsLoaded { get; set; }
        #endregion

        #region Constructors
        internal dloUserGroup(dloUserGroups groups)
        {
            Id = Guid.NewGuid().ToString();
            _groups = groups;
            _app = _groups.Application;
            _rights = new dloUserRights(this);
        }
        #endregion
        private void Validate()
        {
            if(string.IsNullOrWhiteSpace(Name))
            {
                throw new GroupNameCannotBeEmpty();
            }
            foreach(dloUserGroup grp in _groups)
            {
                if(grp!=this)
                {
                    if(Name.Equals(grp.Name))
                    {
                        throw new GroupNamesMustBeUnique();
                    }
                }
            }
        }
        public void Save()
        {
            Validate();
            string sql = "";
            DbCommand cmd = Application.DbInfo.Connection.CreateCommand();
            if (EditMode == ObjectStates.Added)
            {
                sql = "INSERT INTO dsto_groups(guid,Name,Description,Deleted) VALUES(@id,@name,@description,@deleted)";
            }
            else if (EditMode == ObjectStates.Modified)
            {
                sql = "UPDATE dsto_groups SET Name=@name,Description=@description WHERE guid=@id";
            }
            cmd.CommandText = sql;

            DbParameter idPar = cmd.CreateParameter();
            idPar.ParameterName = "@id";
            idPar.Value = Id;
            cmd.Parameters.Add(idPar);

            DbParameter namePar = cmd.CreateParameter();
            namePar.ParameterName = "@name";
            if (!string.IsNullOrWhiteSpace(Name))
                namePar.Value = Name;
            else
                namePar.Value = DBNull.Value;
            cmd.Parameters.Add(namePar);

            DbParameter descPar = cmd.CreateParameter();
            descPar.ParameterName = "@description";
            if (!string.IsNullOrWhiteSpace(Description))
                descPar.Value = Description;
            else
                descPar.Value = DBNull.Value;

            cmd.Parameters.Add(descPar);

            DbParameter delPar = cmd.CreateParameter();
            delPar.ParameterName = "@deleted";
            delPar.Value = 0;
            cmd.Parameters.Add(delPar);
            //int res = _app.DbInfo.ExecuteNonQuery(sql);
            int res = Application.DbInfo.ExecuteCECommand(cmd);
            EditMode = ObjectStates.None;
        }

        public void Delete()
        {
            string sql = string.Format("UPDATE dsto_groups SET deleted=1 WHERE guid='{0}'", Id);
            int res = _app.DbInfo.ExecuteNonQuery(sql);
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public class GroupNameCannotBeEmpty:Exception
    {
        public GroupNameCannotBeEmpty():base("Name cannot be empty")
        {

        }
    }
    public class GroupNamesMustBeUnique:Exception
    {
        public GroupNamesMustBeUnique():base("Group names must be unique")
        {

        }
    }
}
