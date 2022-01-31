using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators
{
    public interface IPQGeneratorValidator
    {
        PQGenerateResult Generate(int L, int N, int seedLen);
        PQValidateResult Validate(BigInteger p, BigInteger q, DomainSeed seed, Counter count);

        // Needed for firehose tests mainly
        void AddEntropy(BitString entropy);
    }
}
