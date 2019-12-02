using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.DRBG.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }       // "ctrDRBG/hashDRBG/hmacDRBG"
        public string Mode { get; set; } = "";      // empty
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        
        public bool[] PredResistanceEnabled { get; set; }
        public bool ReseedImplemented { get; set; }

        public Capability[] Capabilities { get; set; }
    }

    public class Capability
    {
        // TODO I have no idea what to name this property
        public string Mode { get; set; }

        public bool DerFuncEnabled { get; set; }
        public MathDomain EntropyInputLen { get; set; }
        public MathDomain NonceLen { get; set; }
        public MathDomain PersoStringLen { get; set; }
        public MathDomain AdditionalInputLen { get; set; }
        public int ReturnedBitsLen { get; set; }
    }
}
