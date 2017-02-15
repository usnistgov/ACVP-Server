using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_ECB
{
    public interface IKnownAnswerTestFactory
    {
        List<TestCase> GetKATTestCases(string testType);
    }
}