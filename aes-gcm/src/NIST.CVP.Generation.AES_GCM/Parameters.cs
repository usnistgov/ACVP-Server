using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Direction { get; set; }
        public int[] KeyLen { get; set; }
        public int[] PtLen { get; set; }
        public int[] ivLen { get; set; }
        public string ivGen { get; set; }
        public string ivGenMode { get; set; }
        public int[] aadLen { get; set; }
        public int[] TagLen { get; set; }
    }
}
