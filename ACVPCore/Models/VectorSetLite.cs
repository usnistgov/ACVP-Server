namespace ACVPCore.Models
{
    public class VectorSetLite
    {
        public long Id { get; set; }
        public string GeneratorVersion { get; set; }
        public long AlgorithmId { get; set; }
        public string Algorithm { get; set; }
        public VectorSetStatus Status { get; set; }
    }
}