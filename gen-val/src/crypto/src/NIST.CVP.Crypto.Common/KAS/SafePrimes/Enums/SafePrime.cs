using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums
{
    /// <summary>
    /// Safe prime group names as per:
    /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar3.pdf
    /// </summary>
    public enum SafePrime
    {
        None,
        [EnumMember(Value = "MODP-2048")]
        Modp2048,
        [EnumMember(Value = "MODP-3072")]
        Modp3072,
        [EnumMember(Value = "MODP-4096")]
        Modp4096,
        [EnumMember(Value = "MODP-6144")]
        Modp6144,
        [EnumMember(Value = "MODP-8192")]
        Modp8192,
        [EnumMember(Value = "ffdhe2048")]
        Ffdhe2048,
        [EnumMember(Value = "ffdhe3072")]
        Ffdhe3072,
        [EnumMember(Value = "ffdhe4096")]
        Ffdhe4096,
        [EnumMember(Value = "ffdhe6144")]
        Ffdhe6144,
        [EnumMember(Value = "ffdhe8192")]
        Ffdhe8192,
    }
}