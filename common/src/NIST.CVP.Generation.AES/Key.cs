using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES
{
    public struct Key
    {
        public DirectionValues Direction { get; set; }
        public byte[] Bytes { get; set; }
        public int BlockLength { get; set; }
        public int Length { get { return Bytes.Length; } }
        public RijndaelKeySchedule KeySchedule { get; set; }
    }
}
