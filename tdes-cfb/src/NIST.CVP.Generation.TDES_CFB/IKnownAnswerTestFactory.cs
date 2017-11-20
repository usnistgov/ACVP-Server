using System.Collections.Generic;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.TDES_CFB
{
    public interface IKnownAnswerTestFactory
    {
        List<TestCase> GetKATTestCases(string testType, string direction, string algo);
    }
}