using System;
using System.Runtime.Serialization;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums
{
    public enum AnsiX942Oids
    {
        None,

        [EnumMember(Value = "TDES")]
        TDES,

        [EnumMember(Value = "AES-128-KW")]
        AES_128_KW,

        [EnumMember(Value = "AES-192-KW")]
        AES_192_KW,

        [EnumMember(Value = "AES-256-KW")]
        AES_256_KW
    }

    public static class AnsiX942OidHelper
    {
        public static BitString GetOidFromEnum(AnsiX942Oids oid)
        {
            return oid switch
            {
                AnsiX942Oids.TDES => new BitString("060B2A864886F70D0109100306"),
                AnsiX942Oids.AES_128_KW => new BitString("0609608648016503040105"),
                AnsiX942Oids.AES_192_KW => new BitString("0609608648016503040119"),
                AnsiX942Oids.AES_256_KW => new BitString("060960864801650304012D"),
                _ => throw new ArgumentOutOfRangeException(nameof(oid), oid, "OID not available")
            };
        }
    }
}
