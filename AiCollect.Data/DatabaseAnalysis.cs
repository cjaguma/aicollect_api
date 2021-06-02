using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class DatabaseAnalysis
    {
        #region Members
        private dloDataApplication _application;
        #endregion
        #region Properties
       // public TableStatisticsCollection TableStatistics { get; private set; }
        #endregion

        internal DatabaseAnalysis (dloDataApplication application)
        {
            _application = application;
            ///TableStatistics = new TableStatisticsCollection();
        }

        public void Analyse(string tables)
        {
            string sql = "";
            switch(_application.Provider)
            {
                case DataProviders.SQL:
                    sql = string.Format(@"
                
                                    SELECT t.NAME AS TableName,
                                        p.rows AS RowCounts,
                                        SUM(a.total_pages) * 8 AS TotalSpaceKB, 
                                        SUM(a.used_pages) * 8 AS UsedSpaceKB, 
                                        (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB
                                    FROM 
                                        sys.tables t
                                    INNER JOIN      
                                        sys.indexes i ON t.OBJECT_ID = i.object_id
                                    INNER JOIN 
                                        sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                                    INNER JOIN 
                                        sys.allocation_units a ON p.partition_id = a.container_id
                                    WHERE 
                                        t.NAME in ({0}) 
                                        AND t.is_ms_shipped = 0
                                        AND i.OBJECT_ID > 255 
                                    GROUP BY 
                                        t.Name, p.Rows
                                    ORDER BY 
                                        t.Name
                                        ", tables

                    );
                    break;
                default:
                    throw new Exception("Specified data provider not yet supported");
            }

            DataTable dt = new DataTable();
            _application.DbInfo.ExecuteQuery(sql, dt);

            if(dt.Rows.Count>0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    //TableStatistics ts = new TableStatistics();
                    //ts.TableName = (string)dr["TableName"];

                    //int rows = 0;
                    //int.TryParse(dr["RowCounts"].ToString(), out rows);
                    //ts.Rows = rows;

                    //double totalspace = 0;
                    //double.TryParse(dr["TotalSpaceKB"].ToString(), out totalspace);
                    //ts.TotalSpace=totalspace;

                    //double usedspace = 0;
                    //double.TryParse(dr["UsedSpaceKB"].ToString(), out usedspace);
                    //ts.UsedSpace = usedspace;

                    //double unusedspace = 0;
                    //double.TryParse(dr["UnUsedSpaceKB"].ToString(), out unusedspace);
                    //ts.UnusedSpace= unusedspace;

                    //TableStatistics.Add(ts);
                }
            }
        }
    }
}
