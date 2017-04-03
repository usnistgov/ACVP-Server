using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA3
{
    public struct HashFunction
    {
        public int DigestSize { get; set; }
        public int Capacity { get; set; }
        public bool XOF { get; set; }
    }
}
