using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public interface IEcdsaKeyGenRunner
    {
        EcdsaKeyResult GenerateKey(EcdsaKeyParameters param);
    }
}