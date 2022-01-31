using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC
{
    public class MacResult : ICryptoResult
    {
        public BitString Mac { get; }
        public string ErrorMessage { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public MacResult(BitString mac)
        {
            Mac = mac;
        }

        public MacResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
