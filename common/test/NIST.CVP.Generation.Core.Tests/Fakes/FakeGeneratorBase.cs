using System.Collections.Generic;
using NIST.CVP.Generation.Core.Resolvers;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeGeneratorBase : GeneratorBase
    {
        public new List<JsonOutputDetail> JsonOutputs;

        public FakeGeneratorBase(string resolverType)
        {
            switch (resolverType)
            {
                case "answer":
                    JsonOutputs.Add(new JsonOutputDetail { OutputFileName = "answer.json", Resolver = new AnswerResolver()});
                    break;
                case "prompt":
                    JsonOutputs.Add(new JsonOutputDetail { OutputFileName = "prompt.json", Resolver = new PromptResolver()});
                    break;
                case "result":
                    JsonOutputs.Add(new JsonOutputDetail {OutputFileName = "testResults.json", Resolver = new ResultResolver()});
                    break;
                default:
                    JsonOutputs = null;
                    break;
            }
        }

        // Need an accessor for the protected method inside of GeneratorBase
        public GenerateResponse SaveOutputsTester(string requestFilePath, ITestVectorSet testVector)
        {
            return SaveOutputs(requestFilePath, testVector);
        }
    }
}