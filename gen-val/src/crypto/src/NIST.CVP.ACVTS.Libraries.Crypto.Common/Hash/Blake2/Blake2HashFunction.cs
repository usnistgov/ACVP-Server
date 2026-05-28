using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2
{
    public class Blake2HashFunction
    {
        public Blake2HashFunction(Blake2Variant variant, int digestLength)
        {
            if (digestLength <= 0 || digestLength % 8 != 0)
            {
                throw new ArgumentException("Digest length must be a positive multiple of 8 bits.", nameof(digestLength));
            }

            Variant = variant;
            DigestLength = digestLength;
        }

        public Blake2Variant Variant { get; }
        public int DigestLength { get; }
    }
}
