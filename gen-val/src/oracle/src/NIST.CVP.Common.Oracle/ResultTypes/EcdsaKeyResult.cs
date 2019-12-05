using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class EcdsaKeyResult : IResult
    {
        public EccKeyPair Key { get; set; }
    }
}
