using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
{
    public class TestCaseGeneratorNull<TTestGroup, TTestCase, TKeyPair> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>
        where TKeyPair : IDsaKeyPair
    {
        public int NumberOfTestCasesToGenerate => 0;
        public Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup @group, bool isSample, int caseNo = -1)
        {
            return Task.FromResult(new TestCaseGenerateResponse<TTestGroup, TTestCase>("This is the null generator -- nothing is generated"));
        }
    }
}