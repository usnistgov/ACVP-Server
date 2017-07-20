using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KeyWrap
{
    public class KeyWrapResult
    {
        public BitString ResultingBitString { get; private set; }
        public string ErrorMessage { get; private set; }
        public KeyWrapResult(BitString resultingBitString)
        {
            ResultingBitString = resultingBitString;
        }

        public KeyWrapResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public override string ToString()
        {
            if (!Success)
            {
                return ErrorMessage;
            }
            return $"Resulting BitString: {ResultingBitString.ToHex()}";
        }
    }
}