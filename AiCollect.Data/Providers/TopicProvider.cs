using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class TopicProvider : Provider
    {
        public TopicProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public Topic GetTopic(int id)
        {
            string query = $"select * from dsto_topic where oid = {id}";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                System.Data.DataRow row = table.Rows[0];
                Topic topic = new Topic(null);
                InitTopic(topic, row);
                return topic;
            }
            return null;
        }

        private void InitTopic(Topic topic, DataRow row)
        {
            topic.Key = row["guid"].ToString();
            topic.CreatedBy = row["created_by"].ToString();
            topic.OID = int.Parse(row["oid"].ToString());
            topic.Deleted = bool.Parse(row["deleted"].ToString());
            topic.Name = row["Name"].ToString();
            
        }

        public Topics GetTopics(string id,bool withTransaction = false)
        {
            Topics topics = new Topics();
            try
            {
                string query = $"select * from dsto_topic where yref_training = '{id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Topic topic = topics.Add();
                        InitTopic(topic, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return topics;
        }

        public override bool Save(AiCollectObject obj)
        {
            Topic topic = obj as Topic;
            var exists = RecordExists("dsto_topic", topic.Key);
            string query = string.Empty;
            if(!exists)
            {
                query = $"insert into dsto_topic(guid,Name,created_by,yref_training) values('{topic.Key}','{topic.Name}','Admin','{topic.TrainingId}')";
            }
            else
            {
                //update
                query = $"UPDATE dsto_topic SET Name='{topic.Name}', " +
                        $"deleted='{topic.Deleted}' " +
                        $"WHERE guid='{topic.Key}'";
            }
           
            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        public bool DeleteTopic(string key)
        {
            string query = $"delete from dsto_topic where guid='{key}'";

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }

    }
}
