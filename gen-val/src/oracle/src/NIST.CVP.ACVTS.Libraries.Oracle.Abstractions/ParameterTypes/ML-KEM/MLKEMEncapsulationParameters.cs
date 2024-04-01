using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;

public class MLKEMEncapsulationParameters
{
    public KyberParameterSet ParameterSet { get; set; }
    public BitString EncapsulationKey { get; set; }
}
