using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric
{
    public class SymmetricCipherResult : IModeBlockCipherResult
    {
        public BitString Result { get; }
        public string ErrorMessage { get; }
        public SymmetricCipherResult(BitString result)
        {
            Result = result;
        }

        public SymmetricCipherResult(string errorMessage)
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
            return $"Result: {Result.ToHex()}";
        }
    }
}
