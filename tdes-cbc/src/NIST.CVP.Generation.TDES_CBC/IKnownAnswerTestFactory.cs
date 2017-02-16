using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CBC
{
    public interface IKnownAnswerTestFactory
    {
        List<TestCase> GetKATTestCases(string testType);
    }
}