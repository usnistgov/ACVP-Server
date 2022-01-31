using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash
{
    public class HashResult
    {
        public BitString Digest
        {
            get
            {
                if (_digest != null) return _digest;
                if (_digestBytes == null) return null;
                _digest = new BitString(_digestBytes);
                return _digest;
            }

            private set => _digest = value;
        }

        public string ErrorMessage { get; private set; }
        private BitString _digest;
        private readonly byte[] _digestBytes;

        public HashResult(BitString digest)
        {
            Digest = digest;
        }

        public HashResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public HashResult(byte[] digest)
        {
            _digestBytes = digest;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

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
