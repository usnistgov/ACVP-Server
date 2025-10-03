using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;

public class EncapsulationKeyExpectationProvider : TestCaseExpectationProviderBase<MLKEMEncapsulationKeyDisposition>
{
    public EncapsulationKeyExpectationProvider()
    {
        var expectationReasons = new List<MLKEMEncapsulationKeyDisposition>
        {
            { MLKEMEncapsulationKeyDisposition.None, 5 },
            { MLKEMEncapsulationKeyDisposition.ValuesTooLarge, 5 }
        };

        LoadExpectationReasons(expectationReasons);
    }
}
