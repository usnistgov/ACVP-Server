using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Kyber;

public class KyberFactory
{
    private readonly IShaFactory _shaFactory;

    public KyberFactory(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }

    public IMLKEM GetKyber(KyberParameterSet parameterSet)
    {
        var param = new KyberParameters(parameterSet);
        return new Kyber(param, _shaFactory);
    }
}
