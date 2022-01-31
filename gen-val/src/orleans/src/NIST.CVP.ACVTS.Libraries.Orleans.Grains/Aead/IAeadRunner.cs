using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aead
{
    public interface IAeadRunner
    {
        AeadResult DoSimpleAead(IAeadModeBlockCipher cipher, AeadResult fullParam, AeadParameters param);
    }
}
