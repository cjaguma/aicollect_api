using AiCollect.Core;
using AiCollect.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class TrainingProvider : Provider
    {
        public TrainingProvider(dloDbInfo dbInfo) : base(dbInfo)
        {

        }

        public Training GetTraining(int id)
        {
            string query = $"select * from dsto_Training where oid = '{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                System.Data.DataRow row = table.Rows[0];
                Training training = new Training(null);
                InitTraining(training, row);
                training.Trainers = new TrainerProvider(DbInfo).GetTrainers(training.Key);
                training.Trainees = new TraineeProvider(DbInfo).GetTrainees(training.Key);
                training.Topics = new TopicProvider(DbInfo).GetTopics(training.Key);
                return training;
            }
            return null;
        }

        public Reports GetReports(string configuration_id)
        {
            try
            {
               
                Reports reports = new Reports();
                string query = $"select * from dsto_training where configuration_id='{configuration_id}'";
                
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Report report = reports.Add();
                        report.Training = new Trainings().Add();
                        InitTraining(report.Training, row);
                        report.Training.Trainers = new TrainerProvider(DbInfo).GetTrainers(report.Training.Key);
                        report.Training.Trainees = new TraineeProvider(DbInfo).GetRegisteredTrainees(report.Training.Key);
                        report.Training.Topics = new TopicProvider(DbInfo).GetTopics(report.Training.Key);
                    }
                }
                
                return reports;
            }
            catch(Exception ex)
            {
                
                throw ex;
            }
        }

        public Training GetTraining(string id)
        {
            string query = $"select * from dsto_training where guid = '{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                System.Data.DataRow row = table.Rows[0];
                Training training = new Training(null);
                InitTraining(training, row);
                training.Trainers = new TrainerProvider(DbInfo).GetTrainers(training.Key);
                training.Trainees = new TraineeProvider(DbInfo).GetTrainees(training.Key);
                training.Topics = new TopicProvider(DbInfo).GetTopics(training.Key);
                return training;
            }
            return null;
        }

        private void InitTraining(Training training, DataRow row)
        {
            try
            {
                training.Key = row["guid"].ToString();
                training.CreatedBy = row["created_by"].ToString();
                training.OID = int.Parse(row["oid"].ToString());
                training.Name = row["Name"].ToString();
                training.StartDate = DateTime.Parse(row["startdate"].ToString());
                training.EndDate = DateTime.Parse(row["enddate"].ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Trainings GetMobileConfigurationTrainings(int id)
        {
            Trainings trainings = new Trainings();
            try
            {
                string query = $"select * from dsto_training where configuration_id = '{id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    System.Data.DataRow row = table.Rows[0];
                    Training training = trainings.Add();
                    InitTraining(training, row);
                    training.Trainers = new TrainerProvider(DbInfo).GetTrainers(training.Key);
                    training.Trainees = new TraineeProvider(DbInfo).GetTrainees(training.Key);
                    training.Topics = new TopicProvider(DbInfo).GetTopics(training.Key);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trainings;
        }

        public Trainings GetTrainings(int id)
        {
            Trainings trainings = new Trainings();
            try
            {
                string query = $"select * from dsto_training where configuration_id = '{id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    System.Data.DataRow row = table.Rows[0];
                    Training training = trainings.Add();
                    InitTraining(training, row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trainings;
        }

        public Training LastEntry(string key)
        {
            string query = $"select * from dsto_training where guid = '{key}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                System.Data.DataRow row = table.Rows[0];
                Training training = new Training(null);
                InitTraining(training, row);
                return training;
            }
            return null;
        }

        public override bool Save(AiCollectObject obj)
        {
            try
            {
                Training training = obj as Training;
                var query = string.Empty;

                var exists = RecordExists("dsto_training", training.Key);
                if (!exists)
                {
                    query = $"insert into dsto_training(guid,Name,created_by,startdate,enddate,configuration_id) values('{training.Key}','{training.Name}','Admin','{training.StartDate.ToString("yyyy-MM-ddThh:mm:ss")}','{training.EndDate.ToString("yyyy-MM-ddThh:mm:ss")}',{training.ConfigurationId})";
                }
                else
                {
                    //update
                    query = $"UPDATE dsto_training SET Name='{training.Name}', " +
                        $"startdate = '{training.StartDate.ToString("yyyy-MM-ddThh:mm:ss")}', " +
                        $"enddate = '{training.EndDate.ToString("yyyy-MM-ddThh:mm:ss")}', " +
                        $"deleted='{training.Deleted}' " +
                        $"WHERE guid = '{training.Key}'";
                }

                if (DbInfo.ExecuteNonQuery(query) > -1)
                {
                    foreach (var topic in training.Topics)
                    {
                        topic.TrainingId = training.Key;
                        new TopicProvider(DbInfo).Save(topic);
                    }
                    foreach (var trainee in training.Trainees)
                    {
                        trainee.TrainingId = training.Key;
                        new TraineeProvider(DbInfo).Save(trainee);
                    }
                    foreach (var trainer in training.Trainers)
                    {
                        trainer.TrainingId = training.Key;
                        new TrainerProvider(DbInfo).Save(trainer);
                    }
                }
                return true;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public bool DeleteTraining(string id)
        {

            string query = $"delete from dsto_training where guid='{id}'";
            var training = GetTraining(id);

            foreach(var trainee in training.Trainees)
            {
                new TraineeProvider(DbInfo).DeleteTrainee(trainee.Key);
            }

            foreach (var trainer in training.Trainers)
            {
                new TrainerProvider(DbInfo).DeleteTrainer(trainer.Key);
            }

            foreach (var topic in training.Topics)
            {
                new TopicProvider(DbInfo).DeleteTopic(topic.Key);
            }

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }
    }
}
