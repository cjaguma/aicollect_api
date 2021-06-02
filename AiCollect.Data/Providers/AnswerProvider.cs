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
    public class AnswerProvider : Provider
    {
        private Questionaire questionaire;
        private FieldInspection fieldInspection;
        private Certification certification;
        public AnswerProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public Answers GetAnswers(Question question, string response_id, int occurance = 0)
        {
            Answers answers = new Answers(null);
            try
            {
                var isBinary = question.DataType.In(DataTypes.Audio, DataTypes.Image, DataTypes.Video, DataTypes.Binary);
                string query = $"select * from dsto_answer where yref_question='{question.Key}' and occurance='{occurance}' and (yref_questionaire='{response_id}' or yref_certification='{response_id}' or yref_fieldInspection='{response_id}') and deleted=false order by oid asc";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Answer answer = new Answer(null);
                        answer.Key = row["guid"].ToString();
                        answer.OID = int.Parse(row["oid"].ToString());
                        if (!isBinary)
                            answer.AnswerText = row["answertext"].ToString();
                        answer.Deleted = bool.Parse(row["deleted"].ToString());
                        answer.questionKey = row["yref_question"].ToString();
                        answer.Occurance = int.Parse(row["occurance"].ToString());
                        answer.QuestionaireKey = row["yref_questionaire"].ToString();
                        answer.FieldKey = row["yref_fieldInspection"].ToString();
                        answer.CertificationKey = row["yref_certification"].ToString();
                        answers.Add(answer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return answers;
        }

        public override bool Save(AiCollectObject obj)
        {
            try
            {
                Answer answer = obj as Answer;
                var exists = RecordExists("dsto_answer", answer.Key);
                string query = string.Empty;
                if (!exists)
                {
                    if (questionaire != null)
                        query = $"insert into dsto_answer(guid,created_by,answertext,yref_question,yref_questionaire,occurance) " +
                                $"values('{answer.Key}','Admin','{answer.AnswerText}','{answer.questionKey}','{questionaire.Key}','{answer.Occurance}')";
                    else if (certification != null)
                        query = $"insert into dsto_answer(guid,created_by,answertext,yref_question,yref_certification,occurance) " +
                               $"values('{answer.Key}','Admin','{answer.AnswerText}','{answer.questionKey}','{certification.Key}','{answer.Occurance}')";
                    else if (fieldInspection != null)
                        query = $"insert into dsto_answer(guid,created_by,answertext,yref_question,yref_fieldInspection,occurance) " +
                               $"values('{answer.Key}','Admin','{answer.AnswerText}','{answer.questionKey}','{fieldInspection.Key}','{answer.Occurance}')";
                }
                else
                {
                    //update
                    query = $"UPDATE dsto_answer SET " +
                        $"answertext = '{answer.AnswerText}', " +
                        $"Deleted = '{answer.Deleted}', " +
                        $"occurance = '{answer.Occurance}' " +
                        $"WHERE guid = '{answer.Key}'";
                }

                return DbInfo.ExecuteNonQuery(query) > -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void Sync(Certification certification, Question question, string dependency_template = null)
        {
            this.certification = certification;
            foreach (Answer answer in question.Answers)
            {
                answer.questionKey = question.Key;
                Save(answer);
            }
        }

        internal void Sync(Questionaire questionaire, Question question)
        {
            this.questionaire = questionaire;
            foreach (Answer answer in question.Answers)
            {
                answer.questionKey = question.Key;
                Save(answer);
            }
        }

        internal void Sync(FieldInspection fieldInspection, Question question)
        {
            this.fieldInspection = fieldInspection;
            foreach (Answer answer in question.Answers)
            {
                answer.questionKey = question.Key;
                Save(answer);
            }
        }
    }
}
