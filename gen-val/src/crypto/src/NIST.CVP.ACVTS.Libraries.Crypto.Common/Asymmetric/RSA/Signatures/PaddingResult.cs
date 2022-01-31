using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures
{
    public class PaddingResult
    {
        public BitString PaddedMessage { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public PaddingResult(BitString paddedMessage)
        {
            PaddedMessage = paddedMessage;
        }

        public PaddingResult(string error)
        {
            ErrorMessage = error;
        }
    }
}
