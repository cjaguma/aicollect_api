using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class Questionaires : AiCollectObject, ICollection<Questionaire>, IEnumerable<Questionaire>,IComparable<Questionaires>
    {

        private List<Questionaire> _questionaires;

        public Questionaires(AiCollectObject parent) : base(parent)
        {
            _questionaires = new List<Questionaire>();
        }

        public Questionaires() : base()
        {
            _questionaires = new List<Questionaire>();
        }

        public int Count
        {
            get
            {
                return _questionaires.Count();
            }
        }

        public Questionaire this[int index]
        {
            get
            {
                return _questionaires[index];
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Questionaire Add()
        {
            Questionaire obj = (Questionaire)ObjectFactory.Create(DataCollectionObectTypes.Questionaire, this);
            obj.ObjectState = ObjectStates.Added;
            _questionaires.Add(obj);
            return obj;
        }

        public override void Cancel()
        {

        }

        public IEnumerator<Questionaire> GetEnumerator()
        {
            foreach (var ai in _questionaires)
                yield return ai;
        }

        public override void Update()
        {

        }

        public override void Validate()
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal DataCollectionObject ByKey(string key)
        {
            return _questionaires.FirstOrDefault(c => c.Key == key);
        }

        public void Add(Questionaire item)
        {
            if(item.CollectionObjectType == DataCollectionObectTypes.Questionaire)
            {
                Questionaire questionaire = item as Questionaire;
            }
            _questionaires.Add(item);
        }

        public void Clear()
        {
            _questionaires.Clear();
        }

        public bool Contains(Questionaire item)
        {
            return _questionaires.Contains(item);
        }

        public void CopyTo(Questionaire[] array, int arrayIndex)
        {

        }

        public bool Remove(Questionaire item)
        {
            _questionaires.Remove(item);
            return true;
        }

        internal DatabaseQueries GenerateSectionsTable(DataProviders provider)
        {
            Section section = null;
            foreach (Questionaire qn in _questionaires.Where(c => c is Questionaire).Cast<Questionaire>())
            {
                var hasSections = qn.Sections.Count() > 0;
                if (!hasSections) continue;
                section = qn.Sections[0];
                break;
            }
            if (section == null) return null;
            DatabaseQueries databaseQueries = section.GenerateTableScript("dsto_sections", provider);
            return databaseQueries;
        }

        internal DatabaseQueries GenerateSubSectionsTable(DataProviders provider)
        {
            SubSection subsection = null;
            bool found = false;
            foreach (Questionaire qn in _questionaires.Where(c => c is Questionaire).Cast<Questionaire>())
            {            
                foreach (Section section in qn.Sections)
                {
                    var hasSubSections = section.SubSections.Count > 0;
                    if (!hasSubSections) continue;

                    subsection = section.SubSections[0];
                    found = true;
                    break;
                }
                if (found)
                    break;
            }

            if (subsection == null) return null;

            DatabaseQueries databaseQueries = subsection.GenerateTableScript("dsto_subsections", provider);

            return databaseQueries;

        }

        internal DatabaseQueries GenerateSubSectionsXQuestionTable(DataProviders provider)
        {
            DatabaseQueries databaseQueries = new DatabaseQueries();

            DatabaseQuery dbQuery = new DatabaseQuery();
            dbQuery.Name = "dsto_subsectionXquestion";
            dbQuery.Message = Messages.CreateTable;
            var query = "create table dsto_subsectionXquestion\n";
            query += "(\n";
            query += "oid int identity(1,1) primary key,";
            query += "guid uniqueidentifier default newid(),";
            query += "created_on datetime getdate(),";
            query += "created_by varchar(50),";
            query += "lastupdated_by varchar(50),";
            query += "lastupdated_on datetime,";
            query += "yref_question uniqueidentifier references [dsto_questions]([guid]),";
            query += "yref_subsection uniqueidentifier references [dsto_subsections]([guid])";
            dbQuery.SqlStatement = query;
            return databaseQueries;
        }

        internal DatabaseQueries GenerateSectionsXQuestionTable(DataProviders provider)
        {
            DatabaseQueries databaseQueries = new DatabaseQueries();
            DatabaseQuery dbQuery = new DatabaseQuery();
            dbQuery.Name = "dsto_sectionXquestion";
            dbQuery.Message = Messages.CreateTable;
            var query = "create table dsto_sectionXquestion\n";
            query += "(\n";
            query += "oid int identity(1,1) primary key,";
            query += "guid uniqueidentifier default newid(),";
            query += "created_on datetime getdate(),";
            query += "created_by varchar(50),";
            query += "lastupdated_by varchar(50),";
            query += "lastupdated_on datetime,";
            query += "yref_question uniqueidentifier references [dsto_questions]([guid]),";
            query += "yref_section uniqueidentifier references [dsto_sections]([guid])";
            dbQuery.SqlStatement = query;
            return databaseQueries;
        }

        public DatabaseQueries GenerateDatabase(DataProviders provider, Questionaires importedDataObjects = null)
        {

            DatabaseQueries queries = new DatabaseQueries();

            if (importedDataObjects == null)
            {
                foreach (Questionaire dataObject in _questionaires)
                {
                    queries.Append(dataObject.GenerateDatabase(provider));
                }
            }
            else
            {
                //first drop 
                DataCollectionObject configObj = null;

                foreach (Questionaire form in importedDataObjects)
                {
                    configObj = ByKey(form.Key);
                    if (configObj == null)
                    {
                        //drop it
                        queries.Append(form.DropTable());
                    }
                }


                ////first check the cached config and add a data object thats missing 
                foreach (DataCollectionObject form in _questionaires)
                {
                    var importedDataObject = importedDataObjects.ByKey(form.Key);
                    if (importedDataObject == null)
                    {
                        queries.Append(form.GenerateDatabase(provider));
                    }
                }
            
            }
            return queries;
        }

        public DatabaseQueries CreateForeignKeyConstraints(DataProviders provider)
        {
            DatabaseQueries dbQueries = new DatabaseQueries();
            foreach (DataCollectionObject obj in _questionaires)
            {
                // dbQueries.Append(obj.AddForeignKeyConstraints(provider));
            }
            return dbQueries;
        }


        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);

            _questionaires.Clear();
            JArray collectionObjs = null;
            if (obj["Questionaires"]!=null)
            {
                collectionObjs = JArray.FromObject(obj["Questionaires"]);
            }
            else if(obj["CollectionObjects"] != null)
                 collectionObjs = JArray.FromObject(obj["CollectionObjects"]);

            if (collectionObjs != null)
            {
                foreach (var cobj in collectionObjs)
                {
                    DataCollectionObectTypes dataCollectionObectType = DataCollectionObectTypes.None;
                    if (cobj["CollectionObjectType"] != null && ((JValue)cobj["CollectionObjectType"]).Value != null)
                    {
                        dataCollectionObectType = (DataCollectionObectTypes)Enum.Parse(typeof(DataCollectionObectTypes), ((JValue)cobj["CollectionObjectType"]).Value.ToString());
                    }
                    var item = Add();
                    item.ReadJson((JObject)cobj);
                }
            }
        }

        public int CompareTo(Questionaires other)
        {
            var serverQuestionaires = other;
            //compare questionaire
            foreach (var qn in _questionaires)
            {
                var inQuestionaire = serverQuestionaires.ByKey(qn.Key);
                var exists = inQuestionaire != null;
                if (exists)
                {
                    //compare questionaire
                    var different = inQuestionaire.CompareTo(qn) == 0;
                    if (different)
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
    }
}
