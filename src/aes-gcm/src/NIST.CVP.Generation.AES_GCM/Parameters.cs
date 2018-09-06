using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_GCM
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public string[] Direction { get; set; }
        public int[] KeyLen { get; set; }
        public MathDomain PtLen { get; set; }
        public MathDomain ivLen { get; set; }
        public string ivGen { get; set; }
        public string ivGenMode { get; set; }
        public MathDomain aadLen { get; set; }
        public MathDomain TagLen { get; set; }
    }
}
