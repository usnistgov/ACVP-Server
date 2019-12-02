using System;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.ParameterChecker.Tests.Fakes
{
    public class FakeParameterChecker : IParameterChecker
    {
        public ParameterCheckResponse CheckParameters(ParameterCheckRequest request)
        {
            return new ParameterCheckResponse();
        }
    }
}
