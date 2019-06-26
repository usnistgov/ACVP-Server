using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators
{
    public class GGenerateResult
    {
        public BitString G { get; }
        public BitString H { get; }
        public string ErrorMessage { get; }

        public GGenerateResult(BitString g, BitString h)
        {
            G = g;
            H = h;
        }

        public GGenerateResult(BitString g)
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
