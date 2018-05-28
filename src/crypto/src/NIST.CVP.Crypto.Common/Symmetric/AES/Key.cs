using System;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    [Obsolete("Removing with new abstraction")]
    public struct Key
    {
        public DirectionValues Direction { get; set; }
        public bool UseInverseCipher { get; set; }
        public byte[] Bytes { get; set; }
        public int BlockLength { get; set; }
        public int Length => Bytes.Length;
        public IRijndaelKeySchedule KeySchedule { get; set; }
    }
}
