using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class TrainerProvider : Provider
    {
        public TrainerProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }
        public Trainer GetTrainer(int id)
        {
            string query = $"select * from dsto_trainer where oid = '{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                System.Data.DataRow row = table.Rows[0];
                Trainer trainer = new Trainer(null);
                InitTrainer(trainer, row);
                return trainer;
            }
            return null;
        }

        private void InitTrainer(Trainer topic, DataRow row)
        {
            topic.Key = row["guid"].ToString();
            topic.CreatedBy = row["created_by"].ToString();
            topic.OID = int.Parse(row["oid"].ToString());
            topic.Name = row["Name"].ToString();
            
        }

        public Trainers GetTrainers(string id)
        {
            Trainers trainers = new Trainers();
            try
            {
                string query = $"select * from dsto_trainer where yref_training = '{id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Trainer trainer = trainers.Add();
                        InitTrainer(trainer, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trainers;
        }

        public override bool Save(AiCollectObject obj)
        {
            Trainer trainer = obj as Trainer;

            var exists = RecordExists("dsto_trainer", trainer.Key);
            string query = string.Empty;
            if(!exists)
            {
                query = $"insert into dsto_trainer(guid,Name,created_by,yref_training) values('{trainer.Key}','{trainer.Name}','Admin','{trainer.TrainingId}')";
            }
            else
            {
                //update
                query = $"UPDATE dsto_trainer SET Name='{trainer.Name}', " +
                        $"Deleted='{trainer.Deleted}' " +
                        $"WHERE guid = '{trainer.Key}'";
            }

           
            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        public bool DeleteTrainer(string key)
        {
            string query = $"delete from dsto_trainer where guid='{key}'";

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }
    }
}
