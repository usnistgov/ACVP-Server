using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.GenValApp.Tests.Fakes
{
    public class FakeValidator : IValidator
    {
        public ValidateResponse Validate(ValidateRequest validateRequest)
        {
            return new ValidateResponse(string.Empty, StatusCode.Success);
        }
    }
}
