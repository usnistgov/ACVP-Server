using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.AES_ECB.IntegrationTests.Fakes
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
