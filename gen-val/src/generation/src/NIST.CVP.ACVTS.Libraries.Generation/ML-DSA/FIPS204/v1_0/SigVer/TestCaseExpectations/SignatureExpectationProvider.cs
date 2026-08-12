using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.v1_0.SigVer.TestCaseExpectations;

public class SignatureExpectationProvider : TestCaseExpectationProviderBase<MLDSASignatureDisposition>
{
    public SignatureExpectationProvider() : this(3)
    {
    }

    /// <summary>
    /// validCount controls how many valid (None) cases the group carries. Pre-hash groups pass the
    /// number of registered hash functions so every one of them can be paired with a valid case; only
    /// a valid case can test that the verifier used the right pre-hash OID and digest, since a negative
    /// case is expected to fail whichever pre-hash function it names.
    /// </summary>
    public SignatureExpectationProvider(int validCount)
    {
        var expectationReasons = new List<MLDSASignatureDisposition>
        {
            { MLDSASignatureDisposition.None, validCount },
            { MLDSASignatureDisposition.ModifyMessage, 3 },
            { MLDSASignatureDisposition.ModifySignature, 3 },
            { MLDSASignatureDisposition.ModifyHint, 3 },
            { MLDSASignatureDisposition.ModifyZ, 3 }
        };

        LoadExpectationReasons(expectationReasons);
    }
}
