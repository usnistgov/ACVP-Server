using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core.Async
{
    public interface ITestCaseGeneratorWithPrep<TTestGroup, TTestCase> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        GenerateResponse PrepareGenerator(TTestGroup group, bool isSample);
    }
}