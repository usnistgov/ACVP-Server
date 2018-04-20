using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
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
        public string[] PQGen { get; set; }
        public string[] GGen { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public string[] HashAlg { get; set; }
    }
}
