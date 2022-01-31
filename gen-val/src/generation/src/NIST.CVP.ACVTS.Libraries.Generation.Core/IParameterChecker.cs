using System;
namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    public interface IParameterChecker
    {
        ParameterCheckResponse CheckParameters(ParameterCheckRequest request);
    }
}
