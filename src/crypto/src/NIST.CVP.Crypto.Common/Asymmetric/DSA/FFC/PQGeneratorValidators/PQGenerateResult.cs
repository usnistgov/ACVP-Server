using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators
{
    public class PQGenerateResult
    {
        public BitString P { get; }
        public BitString Q { get; }
        public DomainSeed Seed { get; }
        public Counter Count { get; }
        public string ErrorMessage { get; }

        public PQGenerateResult(BitString p, BitString q, DomainSeed seed, Counter count)
        {
            P = p;
            Q = q;
            Seed = seed;
            Count = count;
        }

        public PQGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
