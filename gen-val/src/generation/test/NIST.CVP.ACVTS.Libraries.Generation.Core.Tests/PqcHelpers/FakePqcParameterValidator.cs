using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.PqcHelpers;

public class FakePqcParameterValidator : PqcParameterValidator, IParameterValidator<FakeParameters>
{
    public ParameterValidateResponse Validate(FakeParameters parameters)
    {
        var errors = new List<string>();

        ValidateSignatureInterfacesAndPreHash(parameters, errors);

        foreach (var capability in parameters.Capabilities)
        {
            ValidateCapability(capability, parameters, errors);
        }

        return errors.Any() ? new ParameterValidateResponse(errors) : new ParameterValidateResponse();
    }
}
