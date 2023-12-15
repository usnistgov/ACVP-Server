using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent.TestCaseExpectations;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent
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
                    if (parameters.PublicExponentMode == PublicExponentModes.Random)
                    {
                        groups.Add(new TestGroup
                        {
                            Modulo = modulus,
                            KeyMode = format,
                            TestType = TEST_TYPE,
                            TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                            PublicExponentMode = PublicExponentModes.Random,
                            PublicExponent = null
                        });
                    }
                    else
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
            }
            
            return Task.FromResult(groups);
        }
    }    
}
