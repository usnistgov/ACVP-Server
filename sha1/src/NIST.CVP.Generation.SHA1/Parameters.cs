using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }
        public int[] MessageLen { get; set; }
        public int[] DigestLen { get; set; }
        public bool IncludeNull { get; set; }
        public bool BitOriented { get; set; }
    }
}
