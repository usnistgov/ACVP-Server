using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xecdh
{
    public interface IXecdhKeyGenRunner
    {
        XecdhKeyResult GenerateKey(XecdhKeyParameters param);
    }
}
