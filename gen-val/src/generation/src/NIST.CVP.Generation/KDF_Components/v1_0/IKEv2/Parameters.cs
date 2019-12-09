using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF_Components.v1_0.IKEv2
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public Capabilities[] Capabilities { get; set; }
    }

    public class Capabilities
    {
        public string[] HashAlg { get; set; }
        public MathDomain InitiatorNonceLength { get; set; }
        public MathDomain ResponderNonceLength { get; set; }
        public MathDomain DiffieHellmanSharedSecretLength { get; set; }
        public MathDomain DerivedKeyingMaterialLength { get; set; }
    }
}
