using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyVer.TestCaseExpectations
{
    public class KeyExpectationProvider : TestCaseExpectationProviderBase<SafePrimesKeyDisposition>
    {
        public KeyExpectationProvider(bool isSample)
        {
            var list = new List<SafePrimesKeyDisposition>();
            var numberOfTestCases = isSample ? 25 : 10;

            var numberOfFailures = numberOfTestCases.CeilingDivide(4);
            var numberOfSuccesses = numberOfTestCases - numberOfFailures;

            list.Add(SafePrimesKeyDisposition.Valid, numberOfSuccesses);
            list.Add(SafePrimesKeyDisposition.Invalid, numberOfFailures);

            LoadExpectationReasons(list);
        }
    }
}
