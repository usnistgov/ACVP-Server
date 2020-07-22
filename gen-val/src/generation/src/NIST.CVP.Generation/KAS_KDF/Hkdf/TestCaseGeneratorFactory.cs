using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS_KDF.Hkdf
{
	public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
	{
		private readonly IOracle _oracle;

		public TestCaseGeneratorFactory(IOracle oracle)
		{
			_oracle = oracle;
		}
		
		public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
		{
			switch (testGroup.TestType.ToLower())
			{
				case "aft":
					return new TestCaseGeneratorAft(_oracle);
				case "val":
					var testCaseExpectationProvider = new TestCaseExpectationProvider(testGroup.IsSample);
					return new TestCaseGeneratorVal(_oracle, testCaseExpectationProvider, testCaseExpectationProvider.ExpectationCount);
				default:
					return new TestCaseGeneratorNull();
			}
		}
	}
}