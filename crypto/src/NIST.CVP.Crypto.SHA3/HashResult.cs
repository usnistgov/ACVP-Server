using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA3
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
