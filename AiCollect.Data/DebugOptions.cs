using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AiCollect.Data
{
    /// <summary>
    /// Specifies options used to debug a dloDataApplication
    /// </summary>
    public class DebugOptions
    {
        
        private bool _purgeDatabase;
        /// <summary>
        /// Gets or Sets a bool value that indicates whether the  database should be deleted
        /// </summary>
        public bool PurgeDatabase
        {
            get { return _purgeDatabase; }
            set
            {
                if (value != _purgeDatabase)
                {
                    _purgeDatabase = value;
                //    OnPropertyChanged("PurgeDatabase");
                }
            }
        }

        private bool _writeSql;
        /// <summary>
        /// Gets or Sets a value indicating whether the SQL used in generation should be written to the disk
        /// </summary>
        public bool WriteSql
        {
            get { return _writeSql; }
            set
            {
                if (value != _writeSql)
                {
                    _writeSql = value;
                //    OnPropertyChanged("WriteSql");
                }
            }
        }

        private bool _writeLogs;
        /// <summary>
        /// Gets or Sets a value indicating whether log information should be written to the disk
        /// </summary>
        public bool WriteLogs
        {
            get { return _writeLogs; }
            set 
            {
                if (value != _writeLogs)
                {
                    _writeLogs = value;
                  //  OnPropertyChanged("WriteLogs");
                }
            }
        }

        private bool _showLogin;

        public bool ShowLogin
        {
            get { return _showLogin; }
            set 
            {
                if (value != _showLogin)
                {
                    _showLogin = value;
                   // OnPropertyChanged("ShowLogin");
                }
            }
        }

        private bool _hidePrompt;

        public bool HidePrompt
        {
            get { return _hidePrompt; }
            set
            {
                if (value != _hidePrompt)
                {
                    _hidePrompt = value;
                  //  OnPropertyChanged("HidePrompt");
                }
            }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set 
            {
                if (value != _username)
                {
                    _username = value;
                 //   OnPropertyChanged("Username");

                }
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set 
            {
                if (value != _password)
                {
                    _password = value;
                  //  OnPropertyChanged("Password");
                }
            }
        }

        public DebugOptions()
        {
            ShowLogin = true;
            Username = "admin";
            Password = "admin";
        }

        /// <summary>
        /// Persists settings to local disk
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DebugOptions));
            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, this);
            } 

        }


        public static DebugOptions Load(string filename)
        {
            DebugOptions options = null;
            if (File.Exists(filename))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(DebugOptions));
                TextReader reader = new StreamReader(filename);
                object obj = deserializer.Deserialize(reader);
                options = (DebugOptions)obj;
                reader.Close();
            }
            else
            {
                options = new DebugOptions();
            }
            return options;
        }
    }
}
