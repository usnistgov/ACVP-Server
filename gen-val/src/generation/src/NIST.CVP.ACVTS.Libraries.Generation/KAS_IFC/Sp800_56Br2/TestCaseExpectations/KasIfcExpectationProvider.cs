using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2.TestCaseExpectations
{
    public class KasIfcExpectationProvider : TestCaseExpectationProviderBase<KasIfcValTestDisposition>
    {
        public KasIfcExpectationProvider(TestGroup testGroup, bool includeFailureTests)
        {
            var list = new List<KasIfcValTestDisposition>();
            var numberOfTestCases = includeFailureTests ? 25 : 10;

            if (!includeFailureTests)
            {
                list.Add(KasIfcValTestDisposition.Success, numberOfTestCases);
                LoadExpectationReasons(list);
                return;
            }

            int numberOfScenariosPerType = 2;

            /*
			    This test type is *quite* expensive as it has to generate keys in the 10s of times to happen upon a 
			    situation where the shared secret is a leading zero nibble.
			    
			    Avoid this test type for modulo 4096 and above
			*/
            if (testGroup.Modulo < 4096)
            {
                list.Add(KasIfcValTestDisposition.SuccessLeadingZeroNibbleZ, numberOfScenariosPerType);
            }

            //list.Add(KasIfcValTestDisposition.FailChangedZ, numberOfScenariosPerType);
            list.Add(KasIfcValTestDisposition.FailChangedDkm, numberOfScenariosPerType);

            // When a Key confirmation scheme, can introduce errors in additional locations
            if (KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(testGroup.Scheme))
            {
                list.Add(KasIfcValTestDisposition.FailChangedMacData, numberOfScenariosPerType);
                list.Add(KasIfcValTestDisposition.FailChangedTag, numberOfScenariosPerType);
            }

            // The remaining scenarios should be successful.
            int numberOfSuccessTests = numberOfTestCases - list.Count;

            list.Add(KasIfcValTestDisposition.Success, numberOfSuccessTests);

            LoadExpectationReasons(list);
        }
    }
}
