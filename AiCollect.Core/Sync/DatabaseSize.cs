namespace AiCollect.Core.Data
{
    public class DatabaseSize : DatabaseObjectSize
    {
        /// <summary>
        /// Space in the database that has not been reserved for database objects
        /// </summary>
        public string Unallocated { get; set; }
    }
}