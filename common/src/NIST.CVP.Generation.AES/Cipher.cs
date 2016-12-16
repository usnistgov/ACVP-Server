using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES
{
    public struct Cipher
    {
        public ModeValues Mode { get; set; }
        public byte[] IV { get; set; }
        public int BlockLength { get; set; }
        public int SegmentLength { get; set; }
    }
}
