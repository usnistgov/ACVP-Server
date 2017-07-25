using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public string[] SigGenModes { get; set; }
        public int[] Moduli { get; set; }
        public string[] HashAlgs { get; set; }
        public string SaltMode { get; set; }
        public string Salt { get; set; } = "";
        public int[] SaltLen { get; set; }
        public string N { get; set; }
        public string PubExp { get; set; }              // E
        public string PrivExp { get; set; }             // D
    }
}
