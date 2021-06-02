using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class DataCollectionObject : AiCollectObject
    {
        private DataCollectionObectTypes _collectionObjectType;
        private string _name;
        private string _description;
        private string _tableName;
        [Browsable(false)]
       
        public TransitionConditions TransitionConditions { get; private set; }
        public DataCollectionObject(AiCollectObject parent) : base(parent)
        {
            Init();
        }

        private void Init()
        {
            TransitionConditions = new TransitionConditions(this);
            Name = "";
            this.ObjectState = ObjectStates.None;
            this.TableName = "";
        }

        public DataCollectionObject():base()
        {
            Init();
        }

        
        [Browsable(false)]
        [DataMember]
        public DataCollectionObectTypes CollectionObjectType
        {
            get
            {
                return _collectionObjectType;
            }
            set
            {
                if (_collectionObjectType != value)
                {
                    _collectionObjectType = value;
                }
            }
        }


        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
        }


        [DataMember]
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
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        [Browsable(false)]
        public bool IsSystem { get; set; }

        [DataMember]
        [Browsable(false)]
        public string TableName { get => _tableName; set => _tableName = value; }

        [DataMember]
        [Browsable(false)]
        public new string Key
        {
            get
            {
                return base.Key;
            }
            set
            {
                base.Key = value;
            }
        }

        public virtual DatabaseQueries GenerateDatabase(DataProviders provider)
        {
            return GenerateDatabase(null, provider);
        }

        public virtual DatabaseQueries GenerateDatabase(DataCollectionObject importedForm, DataProviders provider)
        {
            return GenerateDatabase(importedForm, false, provider);
        }

        internal DatabaseQueries GenerateTableScript(string tableName, DataProviders provider)
        {
            DatabaseQueries dbQueries = new DatabaseQueries();
            string query = string.Empty;
            DatabaseQuery dbQuery = new DatabaseQuery();
            dbQuery.Name = tableName;
            dbQuery.Message = Messages.CreateTable;
            dbQuery.FriendlyMessage = "Creating table " + tableName;

            switch (provider)
            {
                case DataProviders.SQL:
                    dbQuery.SqlStatement = $@"			
                    CREATE TABLE [{tableName}](		
					";
                    break;
                case DataProviders.MYSQL:
                case DataProviders.SQLite:
                    dbQuery.SqlStatement = $@"		
                    CREATE TABLE [{tableName}] (							
					";
                    break;
                default:
                    dbQuery.SqlStatement = $@"		
                    CREATE TABLE [{tableName}] (									
					";
                    break;
            }



            string q = dbQuery.SqlStatement.Trim(',');
            q = q.Trim(',');
            StringBuilder sb = new StringBuilder();
            sb.Append(q);
            sb.Append(")");
            dbQuery.SqlStatement = sb.ToString();
            dbQuery.Name = tableName;
            dbQuery.Message = Messages.CreateTable;
            dbQueries.Add(dbQuery);
            return dbQueries;
        }

        public virtual DatabaseQueries GenerateDatabase(DataCollectionObject importedForm, bool checkForeignKeys, DataProviders provider)
        {
            if (CollectionObjectType == DataCollectionObectTypes.Section || CollectionObjectType == DataCollectionObectTypes.SubSection) return null;
            DatabaseQueries dbQueries = new DatabaseQueries();
            //  DatabaseQuery dbQuery;
            if (importedForm != null)
            {
                if (this.CompareTo(importedForm) == 0)
                {
                    //check if names are different and rename 
                    if (!this.TableName.Equals(importedForm.TableName))
                    {
                        //rename
                        //  dbQueries.Append(RenameTable(importedForm));
                    }
                }
            }
            else
            {
            }
            return dbQueries;
        }

        public DatabaseQueries DropTable()
        {
            string tableScript = "";
            DatabaseQueries dbQueries = new DatabaseQueries();
            DatabaseQuery dbQuery = new DatabaseQuery();
            dbQuery.FriendlyMessage = "Drop table " + TableName;
            tableScript += string.Format("IF OBJECT_ID('{0}') IS NOT NULL\n", TableName);
            tableScript += "BEGIN";
            tableScript += string.Format("DROP TABLE {0}(", TableName.AddSquareBrackets());
            tableScript += "\nEND";
            dbQuery.SqlStatement = tableScript;
            dbQuery.Name = TableName;
            dbQuery.Message = Messages.CreateTable;
            dbQueries.Add(dbQuery);
            return dbQueries;
        }


        public virtual DatabaseQueries CreateTable(DataProviders provider, string tableScript)
        {
            DatabaseQueries dbQueries = new DatabaseQueries();
            DatabaseQuery dbQuery = new DatabaseQuery();
            dbQuery.FriendlyMessage = "Creating table " + TableName;

            switch (provider)
            {
                case DataProviders.SQL:
                    tableScript += string.Format("IF OBJECT_ID('{0}') IS NULL\n", TableName);
                    tableScript += "BEGIN";
                    tableScript += string.Format("CREATE TABLE {0}(", TableName.AddSquareBrackets());
                    break;
                case DataProviders.MYSQL:
                case DataProviders.SQLite:
                    tableScript += string.Format("CREATE TABLE {0}(", TableName);
                    break;
                default:
                    tableScript += string.Format("CREATE TABLE {0}(", TableName);
                    break;
            }


            tableScript += ")";
            if (provider == DataProviders.SQL)
                tableScript += "\nEND";
            dbQuery.SqlStatement = tableScript;
            dbQuery.Name = TableName;
            dbQuery.Message = Messages.CreateTable;
            dbQueries.Add(dbQuery);

            StringBuilder sb = new StringBuilder();

            return dbQueries;
        }
        public int CompareTo(object obj)
        {
            DataCollectionObject a = this;
            DataCollectionObject b = (DataCollectionObject)obj;
            //bool check3 = a.Conditions.Count.Equals(b.Conditions.Count);

           
            bool check5 = true;
            bool allPass =  check5;

            if (allPass)
                return 1;
            else
                return 0;
        }

        public override void Validate()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    if (string.IsNullOrWhiteSpace(Name))
                        throw new Exception("Name cannot be empty");
                    break;
            }
        }

        public override void Cancel()
        {

        }

        public override void Update()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    Validate();
                    break;
            }
        }

    }

}
