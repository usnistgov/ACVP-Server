using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_ECB.Parsers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.Tests.Fakes
{
    public class ParameterParserFake : IParameterParser
    {
        public ParseResponse<Parameters> Parse(string path)
        {
            var parms = new Parameters
            {
                Algorithm = "AES-ECB",
                Mode = new []{"encrypt"},
                PtLen = new []{96},
                KeyLen = new []{128}
            };
            return  new ParseResponse<Parameters>(parms);
        }
    }
}
