using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;

public class DecapsulationKeyExpectationProvider : TestCaseExpectationProviderBase<MLKEMDecapsulationKeyDisposition>
{
    public DecapsulationKeyExpectationProvider()
    {
        var expectationReasons = new List<MLKEMDecapsulationKeyDisposition>
        {
            { MLKEMDecapsulationKeyDisposition.None, 5 },
            { MLKEMDecapsulationKeyDisposition.ModifyH, 5}
        };
        
        LoadExpectationReasons(expectationReasons);
    }
}
