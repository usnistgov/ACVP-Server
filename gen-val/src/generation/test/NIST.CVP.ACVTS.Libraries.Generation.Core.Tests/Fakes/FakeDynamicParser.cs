using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes
{
    public class FakeExceptionDynamicParser : IDynamicParser
    {
        public ParseResponse<object> Parse(string path)
        {
            throw new Exception();
        }
    }

    public class FakeFailureDynamicParser : IDynamicParser
    {
        public ParseResponse<object> Parse(string path)
        {
            return new ParseResponse<object>("Fail");
        }
    }
}
