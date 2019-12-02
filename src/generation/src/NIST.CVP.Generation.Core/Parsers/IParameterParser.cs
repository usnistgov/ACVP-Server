namespace NIST.CVP.Generation.Core.Parsers
{
    /// <summary>
    /// Provides methods of parsing parameters/registration from a string.
    /// </summary>
    /// <typeparam name="TParameter">The parameters type</typeparam>
    public interface IParameterParser<TParameter> 
        where TParameter : IParameters
    {
        /// <summary>
        /// Parse parameters from the <see cref="contents" /> into <see cref="TParameter"/>
        /// </summary>
        /// <param name="contents">The contents to transform into a <see cref="TParameter"/></param>
        /// <returns></returns>
        ParseResponse<TParameter> Parse(string contents);
    }
}
