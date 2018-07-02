using System;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public RsaKeyResult GetRsaKey() => throw new NotImplementedException();
        public VerifyResult<RsaKeyResult> GetRsaKeyVerify() => throw new NotImplementedException();
        public RsaSignatureResult GetRsaSignature() => throw new NotImplementedException();
        public VerifyResult<RsaSignatureResult> GetRsaVerify() => throw new NotImplementedException();
    }
}
