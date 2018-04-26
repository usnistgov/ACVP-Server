using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.GenValApp.Tests.Fakes
{
    public class FakeGenerator : IGenerator
    {
        public GenerateResponse Generate(string requestFilePath)
        {
            if (requestFilePath.Contains("bad"))
            {
                return new GenerateResponse("fail");
            }

            return new GenerateResponse();
        }
    }
}
