namespace NIST.CVP.Pools.Models
{
    public class PoolProperties
    {
        public ParameterHolder PoolType { get; set; }
        public string PoolName { get; set; }
        public int MaxCapacity { get; set; }
        public int MinCapacity { get; set; }
        public decimal RecycleRatePerHundred { get; set; }
    }
}
