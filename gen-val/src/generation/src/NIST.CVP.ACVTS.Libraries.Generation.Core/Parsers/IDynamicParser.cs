namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers
{
    public interface IDynamicParser
    {
        ParseResponse<object> Parse(string path);
    }
}
