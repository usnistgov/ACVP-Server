namespace NIST.CVP.Generation.Core.Parsers
{
    public interface ILegacyResponseFileParser<TTestVectorSet>
        where TTestVectorSet : ITestVectorSet
    {
        ParseResponse<TTestVectorSet> Parse(string path);
    }
}