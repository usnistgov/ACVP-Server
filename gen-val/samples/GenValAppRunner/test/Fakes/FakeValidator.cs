using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests.Fakes
{
    public class FakeValidator : IValidator
    {
        public Task<ValidateResponse> ValidateAsync(ValidateRequest validateRequest)
        {
            return Task.FromResult(new ValidateResponse(string.Empty, StatusCode.Success));
        }
    }
}
