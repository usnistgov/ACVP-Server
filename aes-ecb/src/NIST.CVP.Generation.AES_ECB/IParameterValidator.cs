using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public interface IParameterValidator
    {
        ParameterValidateResponse Validate(Parameters parameters);
    }
}
