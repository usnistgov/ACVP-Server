using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer.TestCaseExpectations;

public class SignatureExpectationProvider : TestCaseExpectationProviderBase<MLDSASignatureDisposition>
{
    public SignatureExpectationProvider()
    {
        var expectationReasons = new List<MLDSASignatureDisposition>
        {
            { MLDSASignatureDisposition.None, 3 },
            { MLDSASignatureDisposition.ModifyMessage, 3 },
            { MLDSASignatureDisposition.ModifySignature, 3 },
            { MLDSASignatureDisposition.ModifyHint, 3 },
            { MLDSASignatureDisposition.ModifyZ, 3 }
        };

        LoadExpectationReasons(expectationReasons);
    }
}
