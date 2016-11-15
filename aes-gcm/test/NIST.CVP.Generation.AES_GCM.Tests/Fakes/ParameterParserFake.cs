using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM.Tests.Fakes
{
    public class ParameterParserFake : IParameterParser
    {
        public ParseResponse<Parameters> Parse(string path)
        {
            var parms = new Parameters
            {
                aadLen = new []{16},
                Algorithm = "AES-GCM",
                Mode = new []{"encrypt"},
                ivGenMode = "8.2.1",
                ivGen = "internal",
                PtLen = new []{96},
                ivLen = new []{96},
                TagLen = new []{32},
                KeyLen = new []{128}

            };
            return  new ParseResponse<Parameters>(parms);
        }
    }
}
