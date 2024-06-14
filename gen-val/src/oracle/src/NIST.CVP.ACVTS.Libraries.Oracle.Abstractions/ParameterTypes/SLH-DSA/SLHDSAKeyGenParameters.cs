using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;

public class SLHDSAKeyGenParameters : IParameters
{
    public SlhdsaParameterSet SlhdsaParameterSet { get; set; }
}
