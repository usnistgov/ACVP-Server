namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Describes the validation process for a set of parameters.
    /// </summary>
    /// <typeparam name="TParameters">The parameters to validate</typeparam>
    public interface IParameterValidator<in TParameters>
        where TParameters : IParameters
    {
        /// <summary>
        /// Checks a set of parameters for validity - are given values/lengths within expectations?
        /// </summary>
        /// <param name="parameters">The parameters to validate</param>
        /// <returns></returns>
        ParameterValidateResponse Validate(TParameters parameters);
    }
}
