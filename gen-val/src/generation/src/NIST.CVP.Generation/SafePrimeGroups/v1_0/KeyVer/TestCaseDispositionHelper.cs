using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyVer
{
    public static class TestCaseDispositionHelper
    {
        public static List<SafePrimesKeyDisposition> PopulateValidityTestCaseOptions(TestGroup testGroup, bool isSample)
        {
            var list = new List<SafePrimesKeyDisposition>();
            var numberOfTestCases = isSample ? 25 : 10;

            var numberOfFailures = numberOfTestCases.CeilingDivide(4);
            var numberOfSuccesses = numberOfTestCases - numberOfFailures;
            
            list.Add(SafePrimesKeyDisposition.Valid, numberOfSuccesses);
            list.Add(SafePrimesKeyDisposition.Invalid, numberOfFailures);
            
            // Reorder the test case conditions randomly
            list = list.Shuffle();
            
            return list;
        }
        
        public static SafePrimesKeyDisposition GetTestCaseIntention(List<SafePrimesKeyDisposition> validityTestCaseOptions)
        {
            var nextDisposition = validityTestCaseOptions[0];
            validityTestCaseOptions.RemoveAt(0);

            return nextDisposition;
        }
    }
}