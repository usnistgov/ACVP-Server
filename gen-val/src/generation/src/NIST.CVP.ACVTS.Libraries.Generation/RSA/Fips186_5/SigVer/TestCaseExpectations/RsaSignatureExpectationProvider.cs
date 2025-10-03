using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigVer.TestCaseExpectations
{
    public class RsaSignatureExpectationProvider : TestCaseExpectationProviderBase<RSASignatureModifications>
    {
        public RsaSignatureExpectationProvider()
        {
            var expectationReasons = new List<RSASignatureModifications>
            {
                RSASignatureModifications.None,
                RSASignatureModifications.Message,
                RSASignatureModifications.E,
                RSASignatureModifications.Signature,
                RSASignatureModifications.MoveIr,
                RSASignatureModifications.ModifyTrailer
            };

            LoadExpectationReasons(expectationReasons);
        }
    }
}
