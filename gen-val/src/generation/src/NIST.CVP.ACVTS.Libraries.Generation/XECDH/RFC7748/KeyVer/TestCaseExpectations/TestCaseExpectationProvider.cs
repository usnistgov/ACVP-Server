using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyVer.TestCaseExpectations;

public class TestCaseExpectationProvider : TestCaseExpectationProviderBase<XecdhKeyDisposition>
{
    public TestCaseExpectationProvider()
    {
        var expectationReasons = new List<XecdhKeyDisposition>
        {
            { XecdhKeyDisposition.None, 4 },
            { XecdhKeyDisposition.MsbSet, 4 },
            { XecdhKeyDisposition.TooShort, 4 },
            { XecdhKeyDisposition.TooLong, 4 }
        };

        LoadExpectationReasons(expectationReasons);
    }
}
