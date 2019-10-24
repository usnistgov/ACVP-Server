using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class RsaKeyParameters : IParameters
    {
        public PrimeGenModes KeyMode { get; set; }
        public PublicExponentModes PublicExponentMode { get; set; }
        public BitString PublicExponent { get; set; }
        public int Modulus { get; set; }
        public HashFunction HashAlg{ get; set; }
        public PrimeTestModes PrimeTest { get; set; }
        public BitString Seed { get; set; }
        public int[] BitLens { get; set; }
        public Fips186Standard Standard { get; set; }

        public PrivateKeyModes KeyFormat { get; set; }

        public override bool Equals(object other)
        {
            if (other is RsaKeyParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(KeyMode, PublicExponentMode, Modulus, HashAlg?.DigestSize, HashAlg?.Mode, PrimeTest);
    }
}
