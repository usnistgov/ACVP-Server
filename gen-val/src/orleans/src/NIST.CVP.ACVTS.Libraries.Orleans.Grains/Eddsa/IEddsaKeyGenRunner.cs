using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
{
    public interface IEddsaKeyGenRunner
    {
        EddsaKeyResult GenerateKey(EddsaKeyParameters param);
    }
}
