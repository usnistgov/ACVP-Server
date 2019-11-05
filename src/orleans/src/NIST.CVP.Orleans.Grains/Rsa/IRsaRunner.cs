using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public interface IRsaRunner
    {
        RsaPrimeResult GeneratePrimes(RsaKeyParameters param, IEntropyProvider entropyProvider);
        RsaKeyResult CompleteKey(RsaKeyResult param, PrivateKeyModes keyMode);
    }
}