using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{

    [DataContract]
    public class EnumList : AiCollectObject
    {
        [DataMember]
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
        [DataMember]
        public new int OID
        {
            get
            {
                return base.OID;
            }
            set
            {
                base.OID = value;
            }
        }
        private string _name;
        private EnumListValues enumValues;

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [DataMember]
        public EnumListTypes EnumListType { get; set; }

        [DataMember]
        public EnumListValues EnumValues
        {
            get
            {
                return enumValues;
            }
            set
            {
                if (enumValues != value)
                {
                    enumValues = value;
                }
            }
        }

        protected string _tableName;
        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [DataMember]
        public string TableName
        {
            get
            {
                return "Enum_" + _name;
            }
            set { }

        }
        [DataMember]
        public int ConfigurationId { get; set; }

        [DataMember]
        public int QuestionId { get; set; }

        public new AiCollectObject Parent
        {
            get
            {
                return base.Parent as AiCollectObject;
            }
        }
        public EnumList(AiCollectObject parent) : base(parent)
        {
            EnumValues = new EnumListValues(this);
        }

        public EnumList() : base(null)
        {
            EnumValues = new EnumListValues();
        }

        public override void ReadJson(Newtonsoft.Json.Linq.JObject obj)
        {
            base.ReadJson(obj);
            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
                Name = ((JValue)obj["Name"]).Value.ToString();

     
            JArray listitemsObj = JArray.FromObject(obj["EnumValues"]);
            if (listitemsObj != null)
                EnumValues.ReadJson(obj);
            ObjectState = ObjectStates.None;
            SetOriginal();
        }


        public int Compare(EnumList other)
        {
            var chkName = this.Name == other.Name;

            return 1;
        }

        public override JObject ToJson()
        {
            var jobject = base.ToJson();
            jobject.Add("Name", Name);
            jobject.Add("TableName", TableName);
            JArray jArray = new JArray();
            foreach (var enumValue in EnumValues)
            {
                JObject enumValObj = new JObject();
                enumValObj.Add("Code", enumValue.Code);
                enumValObj.Add("Description", enumValue.Description);
            }
            jobject.Add("EnumValues", jArray);
            return jobject;
        }

        public override void Validate()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    if (string.IsNullOrWhiteSpace(Name))
                        throw new Exception("Name cannot be empty");
                    if (string.IsNullOrWhiteSpace(TableName))
                        throw new Exception("Name cannot be empty");
                    if (Parent is EnumLists)
                    {
                        foreach (var e in Parent as EnumLists)
                        {
                            if (e == this) continue;
                            if (e.Name == this.Name)
                                throw new Exception("Enum list with the same name already exists");
                        }
                    }
                    break;
            }
        }

        public override void Cancel()
        {

        }

        public override void Update()
        {

        }

        public override int CompareTo(AiCollectObject other)
        {
            EnumList enumList = this;
            EnumList enumList1 = other as EnumList;
            var sameName = enumList.Name.Equals(enumList1.Name);
            var sameTableName = enumList.TableName.Equals(enumList1.TableName);
            return sameName && sameTableName ? 1 : 0;
        }

        private string GenerateQuery(DataProviders provider)
        {
            StringBuilder sb = new StringBuilder();

            switch (provider)
            {
                case DataProviders.SQL:
                    sb.Append("CREATE TABLE [");
                    sb.Append(TableName);
                    sb.Append("]( [Code] VARCHAR(30) PRIMARY KEY NOT NULL, [Description] VARCHAR(MAX))");
                    break;
                case DataProviders.SQLCE:
                    sb.Append("CREATE TABLE ");
                    sb.Append(TableName);
                    sb.Append("( Code NVARCHAR(30) PRIMARY KEY NOT NULL, Description NTEXT )");
                    break;
                case DataProviders.SQLite:
                    sb.Append("CREATE TABLE ");
                    sb.Append(TableName);
                    sb.Append("( Code TEXT PRIMARY KEY NOT NULL, Description TEXT )");
                    break;
                case DataProviders.MYSQL:
                    sb.Append("CREATE TABLE ");
                    sb.Append(TableName);
                    sb.Append("( Code VARCHAR(30) PRIMARY KEY NOT NULL, Description VARCHAR(255))");
                    break;
            }
            return sb.ToString();
        }

        internal DatabaseQuery GenerateDatabaseDrop(DataProviders provider)
        {
            DatabaseQuery query = new DatabaseQuery();

            query.Name = TableName;
            query.Message = Messages.DropTable;
            query.FriendlyMessage = "Deleting data list " + Name + " table";
            switch (provider)
            {
                default:
                    query.SqlStatement = string.Format("DROP TABLE {0}", TableName);
                    break;
            }
            return query;
        }


        /// <summary>
        /// Generates data base results for this enumeration.
        /// </summary>
        /// <returns></returns>
        internal DatabaseQueries GenerateDatabase(DataProviders provider, EnumList importedList = null)
        {
            DatabaseQueries dbResults = new DatabaseQueries();
            DatabaseQuery dbResult = null;

            //if the imported list is empty we are creating a new table
            if (importedList == null)
            {
                dbResult = new DatabaseQuery();
                string sqlCreateTableQuery = GenerateQuery(provider);
                dbResult.Name = TableName;
                dbResult.Message = Messages.CreateTable;
                dbResult.FriendlyMessage = "Creating table for data list " + Name;
                dbResult.SqlStatement = sqlCreateTableQuery;
                dbResults.Add(dbResult);

                foreach (var value in this.EnumValues)
                {
                    dbResult = new DatabaseQuery();
                    dbResult.Name = TableName;

                    dbResult.Message = Messages.InsertRecord;
                    dbResult.FriendlyMessage = "Inserting values into datalist table " + Name;
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            dbResult.SqlStatement = string.Format("INSERT INTO [{0}]([Description],[Code]) VALUES (\'{1}\', \'{2}\')", TableName, value.Description.CleanSqlAndXmlSingleQuote(), value.Code);
                            break;
                        default:
                            dbResult.SqlStatement = string.Format("INSERT INTO {0}(Description,Code) VALUES (\'{1}\', \'{2}\')", TableName, value.Description.CleanSqlAndXmlSingleQuote(), value.Code);
                            break;

                    }
                    dbResults.Add(dbResult);
                }
            }
            else
            {
                //Check if we need to rename the table
                if (this.TableName != importedList.TableName)
                {
                    dbResult = new DatabaseQuery();
                    switch (provider)
                    {
                        case DataProviders.SQL:
                            dbResult.SqlStatement = string.Format("EXEC sp_rename '{0}','{1}'", importedList.TableName, TableName);
                            break;
                    }
                    dbResult.Message = Messages.RenameTable;
                    dbResult.Name = importedList.TableName;
                    dbResults.Add(dbResult);
                }

                EnumListValue foundEnumValue = null;
                //Check if the data list values have changed
                foreach (EnumListValue val in this.EnumValues)
                {
                    foundEnumValue = importedList.EnumValues.ByKey(val.Key);
                    if (foundEnumValue != null)
                    {
                        if (val.CompareTo(importedList.EnumValues.ByKey(val.Key)) == 0)
                        {
                            dbResult = new DatabaseQuery();
                            switch (provider)
                            {
                                case DataProviders.SQL:
                                    dbResult.SqlStatement = string.Format("UPDATE [{0}] SET Description = \'{1}\', Code = \'{2}\' WHERE Code = \'{3}\'", TableName, val.Description.CleanSqlAndXmlSingleQuote(), val.Code, foundEnumValue.Code);
                                    break;
                                default:
                                    dbResult.SqlStatement = string.Format("UPDATE {0} SET Description = \'{1}\', Code = \'{2}\' WHERE Code = \'{3}\'", TableName, val.Description.CleanSqlAndXmlSingleQuote(), val.Code, foundEnumValue.Code);
                                    break;
                            }
                            dbResult.Name = val.Description;
                            dbResult.Message = Messages.UpdateRecord;
                            dbResult.FriendlyMessage = "Updating data list values for " + Name;
                            dbResults.Add(dbResult);
                        }
                    }
                    else
                    {
                        dbResult = new DatabaseQuery();
                        StringBuilder builder = new StringBuilder();

                        switch (provider)
                        {
                            case DataProviders.SQL:
                                builder.AppendLine(string.Format("IF NOT EXISTS (SELECT [Description] FROM {0} WHERE [Description]='{1}' AND [Code]={2})", TableName, val.Description, val.Code));
                                builder.AppendLine("BEGIN");
                                if (TableName == importedList.TableName)
                                {
                                    builder.AppendLine(string.Format("INSERT INTO  [{0}]([Description],[Code]) VALUES(\'{1}\',{2})", importedList.TableName, val.Description, val.Code));
                                }
                                else
                                {
                                    builder.AppendLine(string.Format("INSERT INTO  [{0}]([Description],[Code]) VALUES(\'{1}\',{2})", TableName, val.Description, val.Code));
                                }
                                builder.AppendLine("END");
                                break;
                            default:
                                if (TableName == importedList.TableName)
                                    builder.AppendLine(string.Format("INSERT INTO  {0}(Description,Code) VALUES(\'{1}\',{2})", importedList.TableName, val.Description.CleanSqlAndXmlSingleQuote(), val.Code));
                                else
                                    builder.AppendLine(string.Format("INSERT INTO {0}(Description,Code) VALUES(\'{1}\',{2})", TableName, val.Description.CleanSqlAndXmlSingleQuote(), val.Code));

                                break;
                        }
                        dbResult.SqlStatement = builder.ToString();
                        dbResult.Name = val.Description;
                        dbResult.Message = Messages.InsertRecord;
                        dbResults.Add(dbResult);
                    }
                }
            }
            return dbResults;
        }

        private EnumList _original;

        internal override void SetOriginal()
        {
            _original = Copy() as EnumList;
        }

    }

}
