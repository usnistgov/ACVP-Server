namespace NIST.CVP.Pools
{
    public class PoolProperties
    {
        public ParameterHolder PoolType { get; set; }
        public string FilePath { get; set; }
        public int MaxCapacity { get; set; }
        public int MonitorFrequency { get; set; }
    }
}
