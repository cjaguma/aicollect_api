namespace AiCollect.Core.Sync
{
    /// <summary>
    /// This class represents a single datalabs sync object conflict
    /// </summary>
    public class dsoConflict
    {
        public string OID { get; set; }
        public string MasterTableKey { get; set; }
        public string DeviceId { get; set; }
        public string Data { get; set; }
        public string MasterRowGuid { get; set; }
      
        public int ConflictTypeValue { get; set; }
        public int ConflictStateValue { get; set; }
    }
}