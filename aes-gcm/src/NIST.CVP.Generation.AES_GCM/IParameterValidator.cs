using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public interface IParameterValidator
    {
        ParameterValidateResponse Validate(Parameters parameters);
    }
}
