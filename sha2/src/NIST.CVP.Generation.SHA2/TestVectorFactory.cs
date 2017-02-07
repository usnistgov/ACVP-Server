using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;

namespace NIST.CVP.Generation.SHA2
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public readonly List<string> TestTypes = new List<string>
        {
            "short",
            "long",
            "montecarlo"
        };

        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);
            return new TestVectorSet {TestGroups = groups, Algorithm = "SHA", IsSample = parameters.IsSample};
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var mode in parameters.Mode)
            {
                foreach (var size in parameters.DigestSize)
                {
                    AddTestGroups(TestTypes, mode, size, parameters.IncludeNull, parameters.BitOriented, testGroups);                    
                }
            }

            return testGroups;
        }

        private void AddTestGroups(List<string> testTypesToRun, string function, string digestSize, bool includeNull, bool bitOriented,
            List<ITestGroup> testGroups)
        {
            ModeValues mode;
            switch (function)
            {
                case "SHA1":
                    mode = ModeValues.SHA1;
                    break;
                case "SHA2":
                default:
                    mode = ModeValues.SHA2;
                    break;
            }

            DigestSizes digest;
            switch (digestSize)
            {
                case "224":
                    digest = DigestSizes.d224;
                    break;
                case "256":
                    digest = DigestSizes.d256;
                    break;
                case "384":
                    digest = DigestSizes.d384;
                    break;
                case "512":
                    digest = DigestSizes.d512;
                    break;
                case "512t224":
                    digest = DigestSizes.d512t224;
                    break;
                case "512t256":
                    digest = DigestSizes.d512t256;
                    break;
                case "160":
                default:
                    digest = DigestSizes.d160;
                    break;
            }

            foreach (var testType in testTypesToRun)
            {
                var testGroup = new TestGroup {Function = mode, DigestSize = digest, TestType = testType, IncludeNull = includeNull, BitOriented = bitOriented};
                testGroups.Add(testGroup);
            }
        }
    }
}
