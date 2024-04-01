using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;

public class DilithiumFactory : IDilithiumFactory
{
    private readonly IShaFactory _shaFactory;
    private readonly IEntropyProvider _entropyProvider;
    
    public DilithiumFactory(IShaFactory shaFactory, IEntropyProvider entropyProvider)
    {
        _shaFactory = shaFactory;
        _entropyProvider = entropyProvider;
    }
    
    public IMLDSA GetDilithium(DilithiumParameterSet parameterSet)
    {
        return new Dilithium(new DilithiumParameters(parameterSet), _shaFactory, _entropyProvider);
    }
}
