using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.IKEv1
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

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
