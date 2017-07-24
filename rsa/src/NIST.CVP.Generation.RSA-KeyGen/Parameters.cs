using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public bool InfoGeneratedByServer { get; set; } = true;
        public string PubExpMode { get; set; }
        public string FixedPubExp { get; set; } = "";
        public string[] KeyGenModes { get; set; }
        public int[] Moduli { get; set; }
        public string[] HashAlgs { get; set; }
        public string[] PrimeTests { get; set; }
    }
}
