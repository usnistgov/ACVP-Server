using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash
{
    public class HashResult
    {
        public BitString Digest { get; private set; }
        public string ErrorMessage { get; private set; }

        public HashResult(BitString digest)
        {
            Digest = digest;
        }

        public HashResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }

        public BigInteger ToBigInteger()
        {
            if (!Success)
            {
                return 0;
            }

            return Digest.ToPositiveBigInteger();
        }

        public override string ToString()
        {
            if (!Success)
            {
                return ErrorMessage;
            }

            return $"Digest: {Digest.ToHex()}";
        }
    }
}
