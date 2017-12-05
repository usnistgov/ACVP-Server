using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.TDES_CTR
{
    public interface IKnownAnswerTestFactory
    {
        List<TestCase> GetKATTestCases(string testType, string direction);
    }
}
