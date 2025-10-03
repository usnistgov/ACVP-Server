using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;

public class DecapsulationExpectationProvider : TestCaseExpectationProviderBase<MLKEMDecapsulationDisposition>
{
    public DecapsulationExpectationProvider()
    {
        var expectationReasons = new List<MLKEMDecapsulationDisposition>
        {
            { MLKEMDecapsulationDisposition.None, 5 },
            { MLKEMDecapsulationDisposition.ModifyCiphertext, 5 }
        };

        LoadExpectationReasons(expectationReasons);
    }
}
