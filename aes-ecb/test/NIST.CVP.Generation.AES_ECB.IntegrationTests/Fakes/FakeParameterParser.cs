using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_ECB.Parsers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.IntegrationTests.Fakes
{
    internal class FakeExceptionParameterParser : IParameterParser
    {
        public ParseResponse<Parameters> Parse(string path)
        {
            throw new Exception();
        }
    }

    internal class FakeFailureParamterParser : IParameterParser
    {
        public ParseResponse<Parameters> Parse(string path)
        {
            return new ParseResponse<Parameters>("Error");
        }
    }
}
