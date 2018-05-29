using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.GenValApp.Tests.Fakes
{
    public class FakeValidator : IValidator
    {
        public ValidateResponse Validate(string resultPath, string answerPath, bool showExpected = false)
        {
            if (resultPath.Contains("bad"))
            {
                return new ValidateResponse("fail");
            }

            if (answerPath.Contains("bad"))
            {
                return new ValidateResponse("fail");
            }

            return new ValidateResponse();
        }
    }
}
