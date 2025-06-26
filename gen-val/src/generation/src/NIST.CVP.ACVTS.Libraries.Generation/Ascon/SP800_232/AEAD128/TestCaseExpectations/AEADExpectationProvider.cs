using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128.TestCaseExpectations;

public class AEADExpectationProvider : TestCaseExpectationProviderBase<AsconAEADDisposition>
{
    public AEADExpectationProvider()
    {
        var expectationReasons = new List<AsconAEADDisposition>
        {
            {AsconAEADDisposition.None, 30},
            {AsconAEADDisposition.ModifyTag, 30},
        };

        LoadExpectationReasons(expectationReasons);
    }
}
