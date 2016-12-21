namespace NIST.CVP.Generation.Core.Parsers
{
    public interface IParameterParser<TParameter> 
        where TParameter : IParameters
    {
        ParseResponse<TParameter> Parse(string path);
    }
}
