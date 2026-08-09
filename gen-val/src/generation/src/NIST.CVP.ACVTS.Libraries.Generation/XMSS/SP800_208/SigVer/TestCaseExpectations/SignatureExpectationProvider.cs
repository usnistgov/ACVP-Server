using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer.TestCaseExpectations;

public class SignatureExpectationProvider : TestCaseExpectationProviderBase<XmssSignatureDisposition>
{
    public SignatureExpectationProvider()
    {
        var expectationReasons = new List<XmssSignatureDisposition>
        {
            XmssSignatureDisposition.None,
            XmssSignatureDisposition.ModifySignature,
            XmssSignatureDisposition.ModifyMessage,
            XmssSignatureDisposition.ModifyIndex
        };

        LoadExpectationReasons(expectationReasons);
    }
}
