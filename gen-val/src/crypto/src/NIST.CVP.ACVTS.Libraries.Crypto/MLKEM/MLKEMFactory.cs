using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;

namespace NIST.CVP.ACVTS.Libraries.Crypto.MLKEM;

public class MLKEMFactory
{
    private readonly IShaFactory _shaFactory;

    public MLKEMFactory(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }

    public IMLKEM GetMlkem(MLKEMParameterSet parameterSet)
    {
        var param = new MLKEMParameters(parameterSet);
        return new MLKEM(param, _shaFactory);
    }
}
