using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;

public class MLDSAKeyGenParameters
{
    public DilithiumParameterSet ParameterSet { get; set; }
}
