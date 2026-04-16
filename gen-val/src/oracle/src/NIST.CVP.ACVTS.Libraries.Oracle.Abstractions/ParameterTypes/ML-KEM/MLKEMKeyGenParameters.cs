using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;

public class MLKEMKeyGenParameters
{
    public MLKEMParameterSet ParameterSet { get; set; }
    public MLKEMEncapsulationKeyDisposition EncapDisposition { get; set; }
    public MLKEMDecapsulationKeyDisposition DecapDisposition { get; set; }
}
