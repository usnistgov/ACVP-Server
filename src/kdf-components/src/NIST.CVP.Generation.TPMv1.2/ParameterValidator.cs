using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TPMv1._2
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            return new ParameterValidateResponse();
        }
    }
}
