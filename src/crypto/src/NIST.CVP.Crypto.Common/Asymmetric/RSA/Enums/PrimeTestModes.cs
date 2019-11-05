﻿using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrimeTestModes
    {
        Invalid,

        [EnumMember(Value = "2pow100")]
        TwoPow100ErrorBound,

        [EnumMember(Value = "2powSecStr")]
        TwoPowSecurityStrengthErrorBound
    }
}
