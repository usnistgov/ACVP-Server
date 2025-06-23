using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer.TestCaseExpectations;

public class SignatureExpectationProvider : TestCaseExpectationProviderBase<LmsSignatureDisposition>
{
    public SignatureExpectationProvider()
    {
        var expectationReasons = new List<LmsSignatureDisposition>
        {
            LmsSignatureDisposition.None,
            LmsSignatureDisposition.ModifySignature,
            LmsSignatureDisposition.ModifyMessage,
            LmsSignatureDisposition.ModifyHeader
        };
        
        LoadExpectationReasons(expectationReasons);
    }
}
