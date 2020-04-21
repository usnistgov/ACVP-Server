namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
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