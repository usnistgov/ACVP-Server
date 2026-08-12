using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer.TestCaseExpectations;

public class SignatureExpectationProvider : TestCaseExpectationProviderBase<SLHDSASignatureDisposition>
{
    public SignatureExpectationProvider() : this(2)
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
        var expectationReasons = new List<SLHDSASignatureDisposition>
        {
            { SLHDSASignatureDisposition.None, validCount },
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
