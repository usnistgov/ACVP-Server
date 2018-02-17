namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeGenerator : Generator<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>
    {
        public FakeGenerator() : base (null, null, null, null) {  }

        // Need an accessor for the protected method inside of GeneratorBase
        public GenerateResponse SaveOutputsTester(string requestFilePath, ITestVectorSet testVector)
        {
            return SaveOutputs(requestFilePath, testVector);
        }
    }
}