using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class EcdsaKeyResult : IResult
    {
        public EccKeyPair Key { get; set; }
    }
}
