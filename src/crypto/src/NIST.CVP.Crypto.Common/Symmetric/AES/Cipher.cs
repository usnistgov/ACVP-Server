using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public struct Cipher
    {
        public const int _MAX_IV_BYTE_LENGTH = 16;

        public ModeValues Mode { get; set; }
        public BitString IV { get; set; }
        public int BlockLength { get; set; }
        public int SegmentLength { get; set; }
    }
}
