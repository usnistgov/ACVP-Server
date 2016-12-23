using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES
{
    public struct Cipher
    {
        public const int _MAX_IV_BYTE_LENGTH = 16;

        public ModeValues Mode { get; set; }
        private byte[] _iv;
        public byte[] IV {
            get { return _iv; }
            set
            {
                if (value.Length > _MAX_IV_BYTE_LENGTH)
                {
                    throw new ArgumentOutOfRangeException($"{value} exceeds max length of {_MAX_IV_BYTE_LENGTH}");
                }

                _iv = value;
            }
        }
        public int BlockLength { get; set; }
        public int SegmentLength { get; set; }
    }
}
