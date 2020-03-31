using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.GenValApp.Tests.Fakes
{
    public class FakeGenerator : IGenerator
    {
        public Task<GenerateResponse> GenerateAsync(GenerateRequest generateRequest)
        {
            return Task.FromResult(new GenerateResponse());
        }
    }
}
