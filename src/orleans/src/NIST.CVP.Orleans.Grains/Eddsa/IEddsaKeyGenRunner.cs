using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Orleans.Grains.Eddsa
{
    public interface IEddsaKeyGenRunner
    {
        EddsaKeyResult GenerateKey(EddsaKeyParameters param);
    }
}