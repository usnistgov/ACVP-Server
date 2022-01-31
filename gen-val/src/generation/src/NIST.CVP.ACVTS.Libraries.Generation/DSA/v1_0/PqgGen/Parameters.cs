using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public Capability[] Capabilities { get; set; }
    }

    public class Capability
    {
        public string[] PQGen { get; set; }
        public string[] GGen { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public string[] HashAlg { get; set; }
    }
}
