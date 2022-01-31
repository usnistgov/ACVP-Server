using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly string _testType = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = (from keyAgreementRole in parameters.KasRole
                          from keyConfirmationRole in parameters.KeyConfirmationMethod.KeyConfirmationRoles
                          from keyConfirmationDirection in parameters.KeyConfirmationMethod.KeyConfirmationDirections
                          from mac in parameters.KeyConfirmationMethod.MacMethods.GetRegisteredMacMethods()
                          select new TestGroup()
                          {
                              KasRole = keyAgreementRole,
                              KeyLen = mac.KeyLen,
                              MacLen = mac.MacLen,
                              TestType = _testType,
                              KeyConfirmationDirection = keyConfirmationDirection,
                              KeyConfirmationRole = keyConfirmationRole,
                              KeyAgreementMacType = mac.MacType,
                              IsSample = parameters.IsSample,
                          }).ToList();

            return Task.FromResult(groups);
        }
    }
}
