using AiCollect.Data.Providers;
using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class QuestionProvider : Provider
    {
        public QuestionProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        private void InitQuestion(DataRow row, Question question, string response_id = null, int occurance = 0)
        {
            try
            {
                question.Key = row["guid"].ToString();
                question.OID = int.Parse(row["oid"].ToString());
                question.QuestionText = row["questiontext"].ToString();
                question.QuestionType = (row["question_type"] != DBNull.Value) ? (QuestionTypes)Enum.Parse(typeof(QuestionTypes), row["question_type"].ToString()) : QuestionTypes.None;
                question.DataType = (row["data_type"] != DBNull.Value) ? (DataTypes)Enum.Parse(typeof(DataTypes), row["data_type"].ToString()) : DataTypes.None;
                question.SectionKey = row["yref_questionaire"].ToString();
                question.Deleted = bool.Parse(row["deleted"].ToString());
                question.SubSectionKey = row["yref_subsection"].ToString();
                question.Required = bool.Parse(row["required"].ToString());

                if (!string.IsNullOrEmpty(response_id))
                    question.Answers = new AnswerProvider(DbInfo).GetAnswers(question, response_id, occurance);

                question.Dependencies = new DependencyProvider(DbInfo).GetDependencyByQuestionId(question, question.Answers, response_id);
                if (question.Dependencies.ToList().Count > DbInfo.MaxDependencies)
                    DbInfo.MaxDependencies = question.Dependencies.ToList().Count;

                question.Conditions = new SkipConditionProvider(DbInfo).GetConditions(question);

                if (question is ClosedQuestion)
                    (question as ClosedQuestion).EnumList = new EnumListProvider(DbInfo).GetEnumList(question.OID, EnumListTypes.Question);

                if (question is MultipleChoiceQuestion)
                    (question as MultipleChoiceQuestion).EnumList = new EnumListProvider(DbInfo).GetEnumList(question.OID, EnumListTypes.Question);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal Question GetQuestion(string target, bool withTransaction = false)
        {
            try
            {
                Questions _questions = new Questions();
                string query = $"select * from dsto_questions where guid='{target}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                        Question question = _questions.Add((QuestionTypes)Enum.Parse(typeof(QuestionTypes), table.Rows[0]["question_type"].ToString()));
                        InitQuestion(table.Rows[0], question,null,0);
                    return question;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Collects questions 
        /// Response id is meant for referencing which questionaire/field inspection/certifications submitted from 
        /// the mobile app synchronization
        /// </summary>
        /// <param name="section"></param>
        /// <param name="response_id"></param>
        /// <returns></returns>
        internal Questions GetQuestions(Section section, string response_id = null)
        {
            try
            {
                Questions _questions = new Questions();
                string query = $"select * from dsto_questions where yref_section='{section.Key}' and deleted=false order by oid asc";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Question question = _questions.Add((QuestionTypes)Enum.Parse(typeof(QuestionTypes), row["question_type"].ToString()));
                        InitQuestion(row, question, response_id,0);
                    }
                    return _questions;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string QuestionaireIndentification(string _sectionKey, string response_id,bool withTransaction=false)
        {
            string query = $"select * from dsto_questions where questiontext='name' and yref_section='{_sectionKey}' order by oid asc";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count == 1)
            {
                Question question = new Questions().Add((QuestionTypes)Enum.Parse(typeof(QuestionTypes), table.Rows[0]["question_type"].ToString()));
                InitQuestion(table.Rows[0], question, response_id);

                if (question.Answers.Count > 0)
                    if (question.Answers.FirstOrDefault() != null)
                        return question.Answers.FirstOrDefault().AnswerText;
            }

            return null;
        }

        internal Questions GetQuestions(SubSection subSection, string response_id = null, int occurance = 0)
        {
            try
            {
                Questions _questions = new Questions();
                string query = $"select * from dsto_questions where yref_subsection='{subSection.Key}' and deleted=false order by oid asc";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Question question = _questions.Add((QuestionTypes)Enum.Parse(typeof(QuestionTypes), row["question_type"].ToString()));
                        InitQuestion(row, question, response_id, occurance);
                    }
                    return _questions;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Save(AiCollectObject obj)
         {
            try
            {
                Question question = obj as Question;

                var exists = RecordExists("dsto_questions", question.Key);
                string query = string.Empty;
                if (!exists)
                {
                    if(string.IsNullOrEmpty(question.SubSectionKey))
                        query = $"insert into dsto_questions(guid,created_by,questionText,question_type,data_type,yref_section,required) " +
                                $"values('{question.Key}','Admin','{question.QuestionText}','{(int)question.QuestionType}','{(int)question.DataType}','{question.SectionKey}', '{question.Required}')";
                    else
                        query = $"insert into dsto_questions(guid,created_by,questionText,question_type,data_type,yref_subsection,required) " +
                            $"values('{question.Key}','Admin','{question.QuestionText}','{(int)question.QuestionType}','{(int)question.DataType}','{question.SubSectionKey}', '{question.Required}')";
                }
                else
                {
                    query = $"UPDATE dsto_questions SET " +
                            $"questionText='{question.QuestionText}', " +
                            $"question_type='{(int)question.QuestionType}', " +
                            $"Required='{question.Required}', " +
                            $"data_type='{(int)question.DataType}', " +
                            $"deleted='{question.Deleted}' " +
                            $"WHERE guid='{question.Key}'";
                }

                if (DbInfo.ExecuteNonQuery(query) > -1)
                {
                    query = $"select * from dsto_questions where guid='{question.Key}'";
                    var table = DbInfo.ExecuteSelectQuery(query);
                    if (table.Rows.Count > 0)
                    {
                        DataRow row = table.Rows[0];
                        Question qn = new Questions().Add((QuestionTypes)Enum.Parse(typeof(QuestionTypes), row["question_type"].ToString()));
                        InitQuestion(row, qn);
                        if (question is ClosedQuestion)
                        {
                            (question as ClosedQuestion).EnumList.QuestionId = qn.OID;
                            (question as ClosedQuestion).EnumList.EnumListType = EnumListTypes.Question;
                            new EnumListProvider(DbInfo).Save((question as ClosedQuestion).EnumList);
                        }

                        if (question is MultipleChoiceQuestion)
                        {
                            (question as MultipleChoiceQuestion).EnumList.QuestionId = qn.OID;
                            (question as MultipleChoiceQuestion).EnumList.EnumListType = EnumListTypes.Question;
                            new EnumListProvider(DbInfo).Save((question as MultipleChoiceQuestion).EnumList);
                        }

                        foreach (var condition in question.Conditions)
                        {
                            condition.QuestionKey = qn.Key;
                            new SkipConditionProvider(DbInfo).Save(condition);
                        }

                        foreach(var dependency in question.Dependencies)
                        {
                            dependency.QuestionKey = question.Key;
                            new DependencyProvider(DbInfo).Save(dependency);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool Delete(string reference, params string[] guids)
        {
            string filter = string.Empty;
            int i = 0;
            foreach (var s in guids)
            {
                filter += $"'{s}'";
                if (i < guids.Length - 1)
                    filter += ",";
                i++;
            }
            string query = $"delete from dsto_question where guid IN ({filter}) AND (yref_questionaire = '{reference}' or yref_field_inspection = '{reference}' or yref_certification = '{reference}')";
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > 0;
        }

        public bool DeleteQuestion(string key)
        {
            string query = $"delete from dsto_question where guid='{key}'";

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }

        public bool DeleteByReference(string reference)
        {
            string query = $"delete from dsto_question where yref_questionaire = '{reference}' or yref_section = '{reference}' or yref_subsection = '{reference}'";
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > 0;
        }
    }
}
