namespace NIST.CVP.Generation.Core.Parsers
{
    /// <summary>
    /// Provides methods of parsing parameters/registraiton from a file
    /// </summary>
    /// <typeparam name="TParameter">The parameters type</typeparam>
    public interface IParameterParser<TParameter> 
        where TParameter : IParameters
    {
        /// <summary>
        /// Parse parameters from the file at <see cref="path"/> into <see cref="TParameter"/>
        /// </summary>
        /// <param name="path">The file to parse</param>
        /// <returns></returns>
        ParseResponse<TParameter> Parse(string path);
    }
}
