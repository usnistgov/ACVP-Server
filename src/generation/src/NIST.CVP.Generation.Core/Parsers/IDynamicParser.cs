namespace NIST.CVP.Generation.Core.Parsers
{
    public interface IDynamicParser
    {
        ParseResponse<object> Parse(string path);
    }
}