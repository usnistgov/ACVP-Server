using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.GenValApp.Tests.Fakes
{
    public class FakeValidator : IValidator
    {
        public Task<ValidateResponse> ValidateAsync(ValidateRequest validateRequest)
        {
            return Task.FromResult(new ValidateResponse(string.Empty, StatusCode.Success));
        }
    }
}
