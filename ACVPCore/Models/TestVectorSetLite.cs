namespace ACVPCore.Models
{
    public class TestVectorSetLite
    {
        public long Id { get; set; }
        public string GeneratorVersion { get; set; }
        public long AlgorithmId { get; set; }
        public string Algorithm { get; set; }
        public VectorSetStatus Status { get; set; }
    }
}