using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrimeGenModes
    {
        Invalid,

        [EnumMember(Value = "provable")]
        RandomProvablePrimes,

        [EnumMember(Value = "probable")]
        RandomProbablePrimes,

        [EnumMember(Value = "provableWithProvableAux")]
        RandomProvablePrimesWithAuxiliaryProvablePrimes,

        [EnumMember(Value = "probableWithProvableAux")]
        RandomProbablePrimesWithAuxiliaryProvablePrimes,

        [EnumMember(Value = "probableWithProbableAux")]
        RandomProbablePrimesWithAuxiliaryProbablePrimes
    }
}
