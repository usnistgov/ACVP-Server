using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;

public class MLKEMDecapsulationResult : ICryptoResult
{
    public BitString SharedKey { get; set; }
    public bool ImplicitRejection { get; set; }
}
