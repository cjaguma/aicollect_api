using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace AiCollect.Core
{
    public class DatabaseQueries : IEnumerable<DatabaseQuery>
    {
        #region Members
        private List<DatabaseQuery> _queries;
        #endregion

        #region Events
        public event DatabaseQueryExcecutingHandler Executing;
        public event DatabaseQueryExecutionFailedHandler ExecutionFailed;
        public event DatabaseQueryExecutionSucceededHandler ExecutionSucceeded;
        #endregion

        #region Properties
        public int Count { get { return _queries.Count; } }
        public bool HasErrors
        {
            get
            {
                int countFailed = _queries.Where(g => g.Result == -1).Count();
                if (countFailed == 0)
                    return false;
                else
                    return true;
            }
        }

        #endregion

        public DatabaseQueries()
        {
            _queries = new List<DatabaseQuery>();
        }

      

        public IEnumerator<DatabaseQuery> GetEnumerator()
        {
            foreach (DatabaseQuery query in _queries)
            {
                yield return query;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Append(DatabaseQueries queries)
        {
            foreach (DatabaseQuery result in queries)
            {
                _queries.Add(result);
            }
        }

        public void Add(DatabaseQuery query)
        {
            _queries.Add(query);
        }

        public void ExportToFile(Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DatabaseQuery query in _queries)
            {
                sb.AppendLine(string.Format("{0}\n GO \n", query.SqlStatement));
            }
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(sb.ToString());
                writer.Flush();
            }
        }

        public void OnExecuting(DatabaseQuery query)
        {
            if (Executing != null)
                Executing(query);
        }

        public void OnExecutionSucceeded(DatabaseQuery query)
        {
            if (ExecutionSucceeded != null)
                ExecutionSucceeded(query);
        }

        public void OnExecutionFailed(DatabaseQuery query)
        {
            if (ExecutionFailed != null)
                ExecutionFailed(query);
        }
    }

    public delegate void DatabaseQueryExcecutingHandler(DatabaseQuery query);
    public delegate void DatabaseQueryExecutionSucceededHandler(DatabaseQuery query);
    public delegate void DatabaseQueryExecutionFailedHandler(DatabaseQuery query);
}