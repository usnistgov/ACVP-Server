namespace NIST.CVP.Pools.Models
{
    public class PoolInformation
    {
        public string PoolName { get; set; }
        public bool PoolExists { get; set; } = true;
        public long FillLevel { get; set; }
    }
}
