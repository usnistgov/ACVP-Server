using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.GenValApp.Tests.Fakes
{
    public class FakeGenerator : IGenerator
    {
        public GenerateResponse Generate(GenerateRequest generateRequest)
        {
            return new GenerateResponse();
        }
    }
}
