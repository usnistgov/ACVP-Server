using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1
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
        public string AuthenticationMethod { get; set; }
        public MathDomain InitiatorNonceLength { get; set; }
        public MathDomain ResponderNonceLength { get; set; }
        public MathDomain PreSharedKeyLength { get; set; }
        public MathDomain DiffieHellmanSharedSecretLength { get; set; }
        public string[] HashAlg { get; set; }
    }
}
