using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public interface IRsaRunner
    {
        RsaPrimeResult GeneratePrimes(RsaKeyParameters param, IEntropyProvider entropyProvider);
        RsaKeyResult CompleteKey(RsaKeyResult param, PrivateKeyModes keyMode);
    }
}
