﻿using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using System;
using NIST.CVP.Common.Helpers;

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
        public int PMod8 { get; set; }
        public int QMod8 { get; set; }

        public override bool Equals(object other)
        {
            if (other is RsaKeyParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            // We want keys for 186-2 and 186-4 to be essentially equivalent
            var standard = EnumHelpers.GetEnumDescriptionFromEnum(Standard);
            if (Standard == Fips186Standard.Fips186_2 || Standard == Fips186Standard.Fips186_4)
            {
                standard = "shared standard";
            }
            
            var code1 = HashCode.Combine(KeyMode, PublicExponentMode, Modulus, PrimeTest, standard);
            var code2 = HashCode.Combine(HashAlg?.DigestSize, HashAlg?.Mode);
            var code3 = HashCode.Combine(PMod8, QMod8);
            return HashCode.Combine(code1, code2, code3);
        }
    }
}
