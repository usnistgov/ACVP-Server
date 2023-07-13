using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

public class EcdsaAlterKeyParameters : EcdsaKeyParameters
{
    public EccKeyPair Key { get; set; }
}
