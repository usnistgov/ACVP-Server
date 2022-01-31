using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class DsaDomainParametersParameters : IParameters
    {
        public HashFunction HashAlg { get; set; }
        public PrimeGenMode PQGenMode { get; set; }
        public GeneratorGenMode GGenMode { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public string Disposition { get; set; } = "none";

        public override bool Equals(object other)
        {
            if (other is DsaDomainParametersParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(HashAlg, PQGenMode, GGenMode, L, N, Disposition);
    }
}
