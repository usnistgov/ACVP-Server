using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead
{
    public class AeadModeBlockCipherParameters : IAeadModeBlockCipherParameters
    {
        public BitString AdditionalAuthenticatedData { get; set; }
        public BitString Tag { get; set; }
        public int TagLength { get; }
        public BlockCipherDirections Direction { get; }
        public BitString Iv { get; set; }
        public BitString Key { get; set; }
        public BitString Payload { get; set; }
        public bool UseInverseCipherMode { get; }

        public AeadModeBlockCipherParameters(
            BlockCipherDirections direction,
            BitString iv,
            BitString key,
            BitString payload,
            BitString additionalAuthenticatedData,
            int tagLength,
            bool useInverseCipherMode = false
        )
        {
            Direction = direction;
            Iv = iv;
            Key = key;
            Payload = payload;
            AdditionalAuthenticatedData = additionalAuthenticatedData;
            TagLength = tagLength;
            UseInverseCipherMode = useInverseCipherMode;
        }

        public AeadModeBlockCipherParameters(
            BlockCipherDirections direction,
            BitString iv,
            BitString key,
            BitString payload,
            BitString additionalAuthenticatedData,
            BitString tag,
            bool useInverseCipherMode = false
        ) : this(direction, iv, key, payload, additionalAuthenticatedData, tag.BitLength, useInverseCipherMode)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            Tag = tag;
        }
    }
}
