using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.IKEv2
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
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
