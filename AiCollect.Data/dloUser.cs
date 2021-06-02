using AiCollect.Core;
using CryptSharp;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class dloUser
    {
        #region Members
        private dloDataApplication _app;
        private dloUsers _users;
        #endregion

        #region Properties



        public event UserHandler UserAdded;
        public event UserHandler UserRemoved;

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

        public bool IsChanged { get; private set; }


        private string _id;
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
                    if (value < _editMode || _editMode == ObjectStates.None)
                    {
                        _editMode = value;
                    }
                }
            }
        }

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    IsChanged = true;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    IsChanged = true;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
        private string _firstname;
        public string FirstName
        {
            get
            {
                return _firstname;
            }
            set
            {
                if (_firstname != value)
                {
                    _firstname = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
        private string _lastname;
        public string LastName
        {
            get
            {
                return _lastname;
            }
            set
            {
                if (_lastname != value)
                {
                    _lastname = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
        private string _email;

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
        private string _mobile;

        public string Mobile
        {
            get
            {
                return _mobile;
            }
            set
            {
                if (_mobile != value)
                {
                    _mobile = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
        public string FullName { get { return FirstName + " " + LastName; } }
        private int _status;

        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    EditMode = ObjectStates.Modified;
                }
            }

        }
        private bool _isAdmin;

        public bool IsAdmin
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
        private Image _photo;

        public Image Photo
        {
            get
            {
                return _photo;
            }
            set
            {
                if (_photo != value)
                {
                    _photo = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }

        public string GroupId { get; set; }
        public DateTime DateCreated { get; set; }
        private dloUserGroup _group;
        public dloUserGroup Group
        {
            get
            {
                return _group;
            }
            set
            {
                if (_group != value)
                {
                    _group = value;
                    EditMode = ObjectStates.Modified;
                }
            }
        }
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
        private ObjectStates _editMode;
      

        public dloDataApplication Application { get { return _app; } }
        #endregion

        #region Constructors
        public dloUser() { }
        internal dloUser(dloUsers users)
        {
            Id = Guid.NewGuid().ToString();
            _users = users;
            _app = _users.Application;
            _rights = new dloUserRights(this);

            EditMode = ObjectStates.None;
        }

        #endregion
        public bool BelongsToGroup(dloUserGroup group)
        {
            var result = Group.Id == group.Id;
            return result;
        }

        public bool HasPermission(PermissionTypes type, string name)
        {
            if (IsAdmin)
                return true;
            //set the default to be denied
            bool canProceed = false;
            dloUserRight right = null;
            //Check group permissions
            if (Group != null)
            {
                right = Group.Rights[name];
                if (right != null)
                {
                    canProceed = (type & right.Permissions) == type;
                }
            }

            //check user permissions
            right = Rights[name];
            // right = Rights.GetUserRight(this, name);
            if (right != null)
            {
                canProceed = (type & right.Permissions) == type;
            }

            return canProceed;
        }

        private void Validate(bool chk = false)
        {
            //if (string.IsNullOrWhiteSpace(Username))
            //{
            //    throw new UserNameCannotBeEmptyException(this);
            //}
            //if ((!chk && EditMode == ObjectStates.Modified) || EditMode == ObjectStates.Added)
            //{
            //    if (string.IsNullOrWhiteSpace(Password))
            //    {
            //        throw new PasswordCannotBeEmptyException(this);
            //    }
            //}
            //foreach (var user in _users)
            //{
            //    if (!user.Equals(this))
            //    {
            //        if (user.Username.Equals(Username))
            //        {
            //            throw new UserNamesMustBeUnique();
            //        }
            //    }
            //}
        }

        public void FetchPassword()
        {
            if (EditMode == ObjectStates.None)
            {
                DbCommand cmd = Application.DbInfo.Connection.CreateCommand();
                var query = string.Format("SELECT password FROM dsto_users WHERE guid = '{0}'", Id);
                cmd.CommandText = query;
                var pw = Application.DbInfo.ExecuteScalar(cmd);
                if (pw != null)
                    Password = pw.ToString();
            }
        }

        private bool CheckPassword()
        {
            string pwd = string.Empty;
            bool result = false;
            if (EditMode == ObjectStates.Modified)
            {
                DbCommand cmd = Application.DbInfo.Connection.CreateCommand();
                var query = string.Format("SELECT password FROM dsto_users WHERE guid = '{0}'", Id);
                cmd.CommandText = query;
                var pw = Application.DbInfo.ExecuteScalar(cmd);
                if (pw != null)
                    pwd = pw.ToString();
            }
            //var hash = Crypter.GenerateSHA1Hash(Password);
            //if (!string.IsNullOrWhiteSpace(pwd))
            //    result = pwd.Equals(hash);
            return result;
        }

        public void Save()
        {
            var added = EditMode == ObjectStates.Added;
            var modified = EditMode == ObjectStates.Modified;
            var any = added || modified;
            if (any)
            {
                bool chk = false;
                chk = CheckPassword();
                Validate(chk);

                string sql = "";

                DbCommand cmd = Application.DbInfo.Connection.CreateCommand();

                if (added)
                {
                    sql = "INSERT INTO dsto_users (guid,username,password,firstname,lastname,email,mobile,photo,IsAdmin,YREF_Group,Deleted) VALUES(@id,@username,@password,@firstname,@lastname,@email,@mobile,@photo,@isadmin,@group,@deleted)";
                }
                else if (modified)
                {

                    if (!chk)
                    {
                        sql =
                            @"UPDATE dsto_users 
                            SET username=@username,
                            password=@password,
                            firstname=@firstname,
                            lastname=@lastname,
                            email=@email,
                            mobile=@mobile,
                            photo=@photo,
                            IsAdmin=@isadmin,
                            YREF_Group=@group
                            WHERE guid=@id";
                    }
                    else
                    {
                        sql =
                            @"UPDATE dsto_users 
                            SET username=@username,
                         
                            firstname=@firstname,
                            lastname=@lastname,
                            email=@email,
                            mobile=@mobile,
                            photo=@photo,
                            IsAdmin=@isadmin,
                            YREF_Group=@group
                            WHERE guid=@id";
                    }
                }

                cmd.CommandText = sql;

                DbParameter idPar = cmd.CreateParameter();
                idPar.ParameterName = "@id";
                idPar.Value = Id;
                cmd.Parameters.Add(idPar);

                DbParameter usernamePar = cmd.CreateParameter();
                usernamePar.ParameterName = "@username";
                usernamePar.Value = Username;
                cmd.Parameters.Add(usernamePar);
                if (!chk)
                {
                    DbParameter passwordPar = cmd.CreateParameter();
                    passwordPar.ParameterName = "@password";
                    //passwordPar.Value = Crypter.GenerateSHA1Hash(Password);
                    cmd.Parameters.Add(passwordPar);
                }
                DbParameter firstnamePar = cmd.CreateParameter();
                firstnamePar.ParameterName = "@firstname";
                if (string.IsNullOrWhiteSpace(FirstName))
                    firstnamePar.Value = DBNull.Value;
                else
                    firstnamePar.Value = FirstName;
                cmd.Parameters.Add(firstnamePar);

                DbParameter lastNamePar = cmd.CreateParameter();
                lastNamePar.ParameterName = "@lastname";
                if (string.IsNullOrWhiteSpace(LastName))
                    lastNamePar.Value = DBNull.Value;
                else
                    lastNamePar.Value = LastName;
                cmd.Parameters.Add(lastNamePar);

                DbParameter emailPar = cmd.CreateParameter();
                emailPar.ParameterName = "@email";
                if (string.IsNullOrWhiteSpace(Email))
                    emailPar.Value = DBNull.Value;
                else
                    emailPar.Value = Email;
                cmd.Parameters.Add(emailPar);

                DbParameter mobilePar = cmd.CreateParameter();
                mobilePar.ParameterName = "@mobile";
                if (string.IsNullOrWhiteSpace(Mobile))
                    mobilePar.Value = DBNull.Value;
                else
                    mobilePar.Value = Mobile;
                cmd.Parameters.Add(mobilePar);

                DbParameter photoPar = cmd.CreateParameter();
                photoPar.ParameterName = "@photo";
                switch (_app.Provider)
                {
                    case DataProviders.SQL:
                        photoPar.DbType = System.Data.DbType.Binary;
                        break;
                }
                if (Photo == null)
                    photoPar.Value = DBNull.Value;
                else
                    photoPar.Value = Photo.ToByteArray();
                cmd.Parameters.Add(photoPar);

                DbParameter isAdminPar = cmd.CreateParameter();
                isAdminPar.ParameterName = "@isadmin";
                isAdminPar.Value = IsAdmin;
                isAdminPar.DbType = System.Data.DbType.Boolean;
                cmd.Parameters.Add(isAdminPar);

                DbParameter groupPar = cmd.CreateParameter();
                groupPar.ParameterName = "@group";
                groupPar.Value = Group.Id;
                cmd.Parameters.Add(groupPar);

                DbParameter deletedPar = cmd.CreateParameter();
                deletedPar.ParameterName = "@deleted";
                deletedPar.Value = 0;
                cmd.Parameters.Add(deletedPar);
                int res = Application.DbInfo.ExecuteCECommand(cmd);
                if (res == -2)
                    throw new Exception("Failed to register user");
                //else
                //{
                //    EditMode = ObjectStates.None;
                //    OnUserAdded(this, new EventArgs());
                //}

            }
        }

        public bool ChangePassword(string oldPassword, string newPassword, bool reset = false)
        {
            //Check if the old password is correct
            if (!reset && !CheckOldSuppliedPassword(oldPassword))
                throw new Exception("Wrong old password supplied!");

            //continue to update the password
            string sql = "UPDATE dsto_users SET Password=@password WHERE guid=@id";
            DbCommand cmd = Application.DbInfo.Connection.CreateCommand();
            cmd.CommandText = sql;

            //add command parameters
            DbParameter idPar = cmd.CreateParameter();
            idPar.ParameterName = "@id";
            idPar.Value = Id;
            cmd.Parameters.Add(idPar);

            DbParameter passwordPar = cmd.CreateParameter();
            passwordPar.ParameterName = "@password";
            //passwordPar.Value = Crypter.GenerateSHA1Hash(newPassword);

            cmd.Parameters.Add(passwordPar);

            if (Application.DbInfo.ExecuteCECommand(cmd) != -2)
                return true;

            return false;
        }

        private bool CheckOldSuppliedPassword(string oldPassword)
        {

            string sql = "SELECT Password FROM dsto_users WHERE guid=@userid";
            DbCommand cmd = Application.DbInfo.Connection.CreateCommand();
            cmd.CommandText = sql;

            //add command parameters
            DbParameter idPar = cmd.CreateParameter();
            idPar.ParameterName = "@userid";
            idPar.Value = Id;
            cmd.Parameters.Add(idPar);

            //string hashedoldpassword = Crypter.GenerateSHA1Hash(oldPassword);

            //object dbhashedpassword = Application.DbInfo.ExecuteScalar(cmd);
            //if (dbhashedpassword != null || !dbhashedpassword.Equals(DBNull.Value))
            //    if (hashedoldpassword.Equals(dbhashedpassword as string))
            //        return true;

            return false;
        }


        private bool Delete()
        {

            DbCommand cmd = Application.DbInfo.Connection.CreateCommand();
            var query = string.Format("DELETE FROM dsto_users WHERE guid = '{0}'", Id);
            cmd.CommandText = query;
            var result = Application.DbInfo.ExecuteQuery(query);
            if (result > 0)
            {
                return true;
            }
            return false;

        }

        public bool Cancel()
        {
            return _users.RemoveUser(this);
        }
        public void Remove()
        {
            EditMode = ObjectStates.Removed;
        }

        public void Update()
        {
            if (EditMode == ObjectStates.Removed)
            {
                var removed = Delete();
                if (removed)
                    OnUserRemoved(this, new EventArgs());
            }
            else
            {
                if (EditMode != ObjectStates.None)
                {
                    Save();
                    if (EditMode == ObjectStates.Added)
                        OnUserAdded(this, new EventArgs());
                    EditMode = ObjectStates.None;
                }
            }
        }

        public void Reset()
        {
            _rights = new dloUserRights(this);
            GroupId = "";
            _group = null;
            Id = "";
            Username = "";
            Password = "";
            Email = "";
            FirstName = "";
            LastName = "";
            Status = -1;
        }
    }
    public class UserNamesMustBeUnique : Exception
    {
        public UserNamesMustBeUnique()
            : base("Username already exists")
        {

        }
    }

}
