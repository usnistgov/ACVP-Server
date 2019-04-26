using System;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.ParameterChecker.Tests.Fakes
{
    public class FakeParameterChecker : IParameterChecker
    {
        public ParameterCheckResponse CheckParameters(string requestFilePath)
        {
            if (requestFilePath.Contains("bad") || requestFilePath.Contains("not"))
            {
                return new ParameterCheckResponse("fail");
            }

            return new ParameterCheckResponse();
        }
    }
}
