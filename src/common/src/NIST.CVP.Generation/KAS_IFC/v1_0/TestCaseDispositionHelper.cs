using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.Helpers;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public static class TestCaseDispositionHelper
    {
        public static List<KasIfcValTestDisposition> PopulateValidityTestCaseOptions(TestGroup testGroup, bool includeFailureTests)
        {
            var list = new List<KasIfcValTestDisposition>();
            var numberOfTestCases = includeFailureTests ? 25 : 10;

            if (!includeFailureTests)
            {
                list.Add(KasIfcValTestDisposition.Success, numberOfTestCases);
                return list;
            }

            int numberOfScenariosPerType = 2;
            
            list.Add(KasIfcValTestDisposition.SuccessLeadingZeroNibbleZ, numberOfScenariosPerType);
            list.Add(KasIfcValTestDisposition.FailChangedZ, numberOfScenariosPerType);
            list.Add(KasIfcValTestDisposition.FailChangedDkm, numberOfScenariosPerType);

            // When a Key confirmation scheme, can introduce errors in additional locations
            if (KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(testGroup.Scheme))
            {
                list.Add(KasIfcValTestDisposition.FailChangedMacData, numberOfScenariosPerType);
                list.Add(KasIfcValTestDisposition.FailChangedTag, numberOfScenariosPerType);
                list.Add(KasIfcValTestDisposition.FailKeyConfirmationBits, numberOfScenariosPerType);
            }

            // The remaining scenarios should be successful.
            int numberOfSuccessTests = numberOfTestCases - list.Count;

            list.Add(KasIfcValTestDisposition.Success, numberOfSuccessTests);

            // Reorder the test case conditions randomly
            list = list.Shuffle();
            
            return list;
        }
        
        public static KasIfcValTestDisposition GetTestCaseIntention(List<KasIfcValTestDisposition> validityTestCaseOptions)
        {
            var nextDisposition = validityTestCaseOptions[0];
            validityTestCaseOptions.RemoveAt(0);

            return nextDisposition;
        }
    }
}