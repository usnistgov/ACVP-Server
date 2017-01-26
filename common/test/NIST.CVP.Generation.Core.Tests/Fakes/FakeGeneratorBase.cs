using System.Collections.Generic;
using NIST.CVP.Generation.Core.Resolvers;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeGeneratorBase : GeneratorBase
    {
        public FakeGeneratorBase() {  }

        // Need an accessor for the protected method inside of GeneratorBase
        public GenerateResponse SaveOutputsTester(string requestFilePath, ITestVectorSet testVector)
        {
            return SaveOutputs(requestFilePath, testVector);
        }
    }
}