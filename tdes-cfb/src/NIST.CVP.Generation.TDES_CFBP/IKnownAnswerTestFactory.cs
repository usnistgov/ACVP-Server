using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public interface IKnownAnswerTestFactory
    {
        List<TestCase> GetKATTestCases(string testType, string direction, string algo);
    }
}