using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class FakeExceptionParameterParser<TParameters> : IParameterParser<TParameters>
            where TParameters : IParameters
    {
        public ParseResponse<TParameters> Parse(string path)
        {
            throw new Exception();
        }
    }

    public class FakeFailureParameterParser<TParameters> : IParameterParser<TParameters>
        where TParameters : IParameters
    {
        public ParseResponse<TParameters> Parse(string path)
        {
            return new ParseResponse<TParameters>("Error");
        }
    }
}
