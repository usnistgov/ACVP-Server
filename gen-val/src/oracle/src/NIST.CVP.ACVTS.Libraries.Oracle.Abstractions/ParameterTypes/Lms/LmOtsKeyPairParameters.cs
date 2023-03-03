using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms
{
    public record LmOtsKeyPairParameters(LmOtsMode LmOtsMode, byte[] I, byte[] Q, byte[] Seed);
}
