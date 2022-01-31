using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;

namespace NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0.Parsers
{
    public abstract class LegacyResponseFileParserBase : ILegacyResponseFileParser<TestVectorSet, TestGroup, TestCase>
    {
        public abstract ParseResponse<TestVectorSet> Parse(string path);

        protected abstract TestGroup CreateTestGroup(string path);
    }
}
