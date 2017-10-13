namespace NIST.CVP.Generation.Core
{
    public interface IDeferredTestCaseResolver<in TTestGroup, in TTestCase, out TCryptoResult>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
        where TCryptoResult : ICryptoResult
    {
        TCryptoResult CompleteDeferredCrypto(TTestGroup testGroup, TTestCase serverTestCase, TTestCase iutTestCase);
    }
}