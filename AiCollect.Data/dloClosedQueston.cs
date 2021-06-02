using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCollect.Core;
using System.Data.Common;
using System.Reflection;

namespace AiCollect.Data
{
    public class dloClosedQueston : ClosedQuestion
    {

        private dloDataApplication _application;

        internal dloClosedQueston(dloDataApplication application) : base(null)
        {
            _application = application;

        }

        public dloClosedQueston(AiCollectObject parent) : base(null)
        {

        }

        public DbCommand CreateInsertCommand(DataProviders provider)
        {
            DbCommand cmd = _application.DbInfo.Connection.CreateCommand(); ;
            cmd.CommandText = "INSERT INTO dsto_question(";
            var count = this.GetType().GetProperties().Count();
            int index = 0;
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                cmd.CommandText = $"{property.Name}";
                if (index < count - 1)
                    cmd.CommandText += ",";
                index += 1;
            }
            cmd.CommandText += ")";
            cmd.CommandText = "guid,created_by,question_text) values(";

            index = 0;
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                cmd.CommandText = $"@{property.Name}";
                if (index < count - 1)
                    cmd.CommandText += ",";
                index += 1;
            }
            cmd.CommandText += ")";

            AddParameters(cmd);

            return cmd;
        }

        private void AddParameters(DbCommand command)
        {
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                DbParameter dbParameter = command.CreateParameter();
                dbParameter.ParameterName = $"@{property.Name}";
                dbParameter.Value = property.GetValue(this);
                command.Parameters.Add(dbParameter);
            }
        }



    }
}
