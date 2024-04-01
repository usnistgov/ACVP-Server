using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;

public class MLKEMKeyGenParameters
{
    public KyberParameterSet ParameterSet { get; set; }
}
