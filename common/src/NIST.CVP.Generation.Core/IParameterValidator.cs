using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    public interface IParameterValidator<in TParameters>
        where TParameters : IParameters
    {
        ParameterValidateResponse Validate(TParameters parameters);
    }
}
