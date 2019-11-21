using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF_Components.v1_0.TPMv1_2
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            return new ParameterValidateResponse();
        }
    }
}
