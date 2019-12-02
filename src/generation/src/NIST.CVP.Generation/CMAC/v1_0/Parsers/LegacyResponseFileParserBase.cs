using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.CMAC.v1_0.Parsers
{
    public abstract class LegacyResponseFileParserBase : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
    {
        public abstract ParseResponse<TestVectorSet> Parse(string path);

        protected abstract TestGroup CreateTestGroup(string path);
    }
}
