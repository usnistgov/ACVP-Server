using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.v1_0.DpComponent
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            return new ParameterValidateResponse();
        }
    }
}
