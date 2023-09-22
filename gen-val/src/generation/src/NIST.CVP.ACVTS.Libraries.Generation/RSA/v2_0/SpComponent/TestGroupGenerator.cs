using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";
        
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            foreach (var format in parameters.KeyFormat)
            {
                foreach (var modulus in parameters.Modulo)
                {
                    groups.Add(new TestGroup
                    {
                        Modulo = modulus,
                        KeyMode = format,
                        TestType = TEST_TYPE,
                        TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                        PublicExponentMode = parameters.PublicExponentMode,
                        PublicExponent = parameters.PublicExponentMode == PublicExponentModes.Fixed ? parameters.PublicExponentValue : null
                    });
                }
            }

            return Task.FromResult(groups);
        }
    }    
}
