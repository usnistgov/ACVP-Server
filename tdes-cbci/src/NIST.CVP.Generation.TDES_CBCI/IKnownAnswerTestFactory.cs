using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CBCI
{
    public interface IKnownAnswerTestFactory
    {
        List<TestCase> GetKATTestCases(string testType, string direction);
    }
}
