using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public class HssSignatureResult
    {
        public BitString Signature { get; private set; }
        public string ErrorMessage { get; private set; }

        public HssSignatureResult(BitString signature)
        {
            Signature = signature;
        }

        public HssSignatureResult(string errorMessage)
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

            return $"Signature: {Signature.ToHex()}";
        }
    }
}
