using AiCollect.Core.Data;

namespace AiCollect.Core.Data
{
    public class TableSize : DatabaseObjectSize
    {
        /// <summary>
        /// Number of rows existing in the table. If the object specified is a Service Broker queue,
        /// this column indicates the number of messages in the queue.
        /// </summary>
        public string Rows { get; set; }
    }
}