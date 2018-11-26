using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class DsaDomainParametersParameters : IParameters
    {
        public HashFunction HashAlg { get; set; }
        public PrimeGenMode PQGenMode { get; set; }
        public GeneratorGenMode GGenMode { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public string Disposition { get; set; }

        public override bool Equals(object other)
        {
            if (other is DsaDomainParametersParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        /// <summary>
        /// Note, purposefully ignoring disposition for equal comparison, as that only matters after the fact.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(HashAlg, PQGenMode, GGenMode, L, N);
    }
}
