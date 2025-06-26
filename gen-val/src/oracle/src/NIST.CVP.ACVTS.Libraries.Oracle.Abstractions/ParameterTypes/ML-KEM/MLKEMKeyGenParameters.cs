using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;

public class MLKEMKeyGenParameters
{
    public KyberParameterSet ParameterSet { get; set; }
    public MLKEMEncapsulationKeyDisposition EncapDisposition { get; set; }
    public MLKEMDecapsulationKeyDisposition DecapDisposition { get; set; }
}
