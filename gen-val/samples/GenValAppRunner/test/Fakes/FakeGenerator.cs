using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests.Fakes
{
    public class FakeGenerator : IGenerator
    {
        public Task<GenerateResponse> GenerateAsync(GenerateRequest generateRequest)
        {
            return Task.FromResult(new GenerateResponse());
        }
    }
}
