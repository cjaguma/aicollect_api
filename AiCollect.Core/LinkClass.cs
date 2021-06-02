using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class LinkClass : DataCollectionObject
    {

        private string _answer;
        public Question Question { get; set; }
        public EnumList AnswerList { get; set; }
        public string Answer 
        {
            get
            {
                return _answer;
            }
            set
            {
                if(_answer!=value)
                {
                    _answer = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        public LinkClass(AiCollectObject parent) : base(parent)
        {
            Name = Question.Name + "X" + AnswerList.Name;
        }

        public override DatabaseQueries GenerateDatabase(DataCollectionObject importedForm, bool checkForeignKeys, DataProviders provider)
        {
            return base.GenerateDatabase(importedForm, checkForeignKeys, provider);
        }

        internal DatabaseQueries AddForeignKeyConstraints()
        {

            DatabaseQueries queries = new DatabaseQueries();

            //string questionQuery = string.Empty;

            //string answerQuery = string.Empty;

            ////ConnectionSettings settings = Configuration.DataStores.Default.Settings;

            //DataProviders provider = settings.Provider;

            ////question key
            //switch (provider)
            //{
            //    case DataProviders.SQL:
            //        questionQuery = $"ADD ALTER {TableName} ADD CONSTRAINT FK_X_questions_table FOREIGN KEY REFERENCES questions_table(oid)";
            //        answerQuery = $"ADD ALTER {TableName} ADD CONSTRAINT FK_X_questions_table FOREIGN KEY REFERENCES {AnswerList.TableName}(oid)";
            //        break;
            //    case DataProviders.SQLite:
            //        questionQuery = $"ADD ALTER {TableName} ADD CONSTRAINT FK_X_questions_table FOREIGN KEY REFERENCES questions_table(oid)";
            //        answerQuery = $"ADD ALTER {TableName} ADD CONSTRAINT FK_X_questions_table FOREIGN KEY REFERENCES {AnswerList.TableName}(oid)";
            //        break;
            //    case DataProviders.MYSQL:
            //        questionQuery = $"ADD ALTER {TableName} ADD CONSTRAINT FK_X_questions_table FOREIGN KEY REFERENCES questions_table(oid)";
            //        answerQuery = $"ADD ALTER {TableName} ADD CONSTRAINT FK_X_questions_table FOREIGN KEY REFERENCES {AnswerList.TableName}(oid)";
            //        break;
            //    default:
            //        break;
            //}

            //for(int i=1;i<=2;i++)
            //{
            //    //DatabaseQuery query = new DatabaseQuery();
            //    //query.SqlStatement 
            //}
            
            return queries;
        }


    }
}
