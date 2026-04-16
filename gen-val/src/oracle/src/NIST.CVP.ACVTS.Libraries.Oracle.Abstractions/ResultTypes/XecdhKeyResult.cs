using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class XecdhKeyResult
    {
        public XecdhKeyPair Key { get; set; }
    }
}
