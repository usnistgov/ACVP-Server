using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyVer.TestCaseExpectations
{
    public class KeyExpectationProvider : TestCaseExpectationProviderBase<EcdsaKeyDisposition>
    {
        public KeyExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<EcdsaKeyDisposition>();

            int countForEachCase = (isSample ? 1 : 4);

            expectationReasons.Add(EcdsaKeyDisposition.None, countForEachCase);
            expectationReasons.Add(EcdsaKeyDisposition.OutOfRange, countForEachCase);
            expectationReasons.Add(EcdsaKeyDisposition.NotOnCurve, countForEachCase);

            LoadExpectationReasons(expectationReasons);
        }
    }
}
