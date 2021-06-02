using System;

namespace AiCollect.Core.Data
{
    /// <summary>
    /// Class to hold size of database object
    /// SQL Server : EXEC sp_spaceused N'Purchasing.Vendor'
    /// </summary>
    public abstract class DatabaseObjectSize
    {
        /// <summary>
        /// Name of object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Size of database
        /// Note: database_size will always be larger than the sum of reserved + unallocated space because
        /// it includes the size of log files, but reserved and unallocated_space consider only data pages.
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Total amount of space used by data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Total amount of space used by indexes
        /// </summary>
        public string IndexSize { get; set; }

        /// <summary>
        /// Total amount of space reserved for objects in the database, but not yet used.
        /// </summary>
        public string Unused { get; set; }

        /// <summary>
        /// Total amount of space allocated by objects
        /// </summary>
        public string Reserved { get; set; }

        /// <summary>
        /// Timestamp of last update
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}