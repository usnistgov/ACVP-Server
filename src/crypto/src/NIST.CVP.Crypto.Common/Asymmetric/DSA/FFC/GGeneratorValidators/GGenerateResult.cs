using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators
{
    public class GGenerateResult
    {
        public BigInteger G { get; }
        public BigInteger H { get; }
        public string ErrorMessage { get; }

        public GGenerateResult(BigInteger g, BigInteger h)
        {
            G = g;
            H = h;
        }

        public GGenerateResult(BigInteger g)
        {
            G = g;
        }

        public GGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
