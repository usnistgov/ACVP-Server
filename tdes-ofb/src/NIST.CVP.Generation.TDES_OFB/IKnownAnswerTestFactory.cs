using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_OFB
{
    public interface IKnownAnswerTestFactory
    {
        List<TestCase> GetKATTestCases(string testType, string direction);
    }
}