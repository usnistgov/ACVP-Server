using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;

public class MLKEMDecapsulationParameters
{
    public MLKEMParameterSet ParameterSet { get; set; }
    public BitString EncapsulationKey { get; set; }
    public MLKEMDecapsulationDisposition Disposition { get; set; }
    public BitString DecapsulationKey { get; set; }
}
