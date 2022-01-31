using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class EddsaKeyResult
    {
        public EdKeyPair Key { get; set; }
    }
}
