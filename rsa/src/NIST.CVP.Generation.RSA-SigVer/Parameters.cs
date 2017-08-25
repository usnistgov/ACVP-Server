using NIST.CVP.Generation.Core;
using NIST.CVP.Crypto.RSA;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public string[] SigVerModes { get; set; }
        public SigCapability[] Capabilities { get; set; }
        public string PubExpMode { get; set; }
        public string FixedPubExpValue { get; set; } = "";
    }

    public class SigCapability
    {
        public int Modulo { get; set; }
        public HashPair[] HashPairs { get; set; }
    }

    public class HashPair
    {
        public string HashAlg { get; set; }
        public int SaltLen { get; set; } = 0;
    }
}
