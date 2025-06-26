using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer.TestCaseExpectations;

public class SignatureExpectationProvider : TestCaseExpectationProviderBase<SLHDSASignatureDisposition>
{
    public SignatureExpectationProvider()
    {
        var expectationReasons = new List<SLHDSASignatureDisposition>
        {
            { SLHDSASignatureDisposition.None, 2 },
            { SLHDSASignatureDisposition.ModifyMessage, 2 },
            { SLHDSASignatureDisposition.ModifySignatureR, 2 },
            { SLHDSASignatureDisposition.ModifySignatureSigFors, 2 },
            { SLHDSASignatureDisposition.ModifySignatureSigHt, 2 },
            { SLHDSASignatureDisposition.ModifySignatureTooLarge, 2 },
            { SLHDSASignatureDisposition.ModifySignatureTooSmall, 2 }
        };

        LoadExpectationReasons(expectationReasons);
    }
}
