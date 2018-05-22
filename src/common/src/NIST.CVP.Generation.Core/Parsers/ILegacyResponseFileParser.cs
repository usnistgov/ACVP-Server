namespace NIST.CVP.Generation.Core.Parsers
{
    public interface ILegacyResponseFileParser<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        ParseResponse<TTestVectorSet> Parse(string path);
    }
}