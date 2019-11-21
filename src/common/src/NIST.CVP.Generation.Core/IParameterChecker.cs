using System;
namespace NIST.CVP.Generation.Core
{
    public interface IParameterChecker
    {
        ParameterCheckResponse CheckParameters(ParameterCheckRequest request);
    }
}
