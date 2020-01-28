using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;

namespace NIST.CVP.Orleans.Grains.Aead
{
    public interface IAeadRunner
    {
        AeadResult DoSimpleAead(IAeadModeBlockCipher cipher, AeadResult fullParam, AeadParameters param);
    }
}