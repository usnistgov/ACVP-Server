using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public interface IPQGeneratorValidator
    {
        PQGenerateResult Generate(int L, int N, int seedLen);
        PQValidateResult Validate(BigInteger p, BigInteger q, DomainSeed seed, Counter count);

        // Needed for firehose tests mainly
        void AddEntropy(BitString entropy);
    }
}
