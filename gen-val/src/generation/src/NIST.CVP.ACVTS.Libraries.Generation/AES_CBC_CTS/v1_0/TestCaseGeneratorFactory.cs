using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0
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
            var testType = testGroup.TestType.ToLower();
            var katType = testGroup.InternalTestType.ToLower();
            var isPartialBlockGroup = testGroup.IsPartialBlockGroup;

            switch (isPartialBlockGroup)
            {
                case false:
                    {
                        switch (katType)
                        {
                            case "gfsbox":
                            case "keysbox":
                            case "vartxt":
                            case "varkey":
                                return new TestCaseGeneratorKnownAnswerSingleBlock(testGroup.KeyLength, katType);
                        }

                        switch (testType)
                        {
                            case "aft":
                                return new TestCaseGeneratorMmtFullBlock(_oracle);
                        }

                        break;
                    }
                case true:
                    {
                        switch (katType)
                        {
                            case "gfsbox":
                            case "keysbox":
                            case "vartxt":
                            case "varkey":
                                return new TestCaseGeneratorKnownAnswerPartialBlock(_oracle, testGroup.KeyLength, katType);
                        }

                        switch (testType)
                        {
                            case "aft":
                                return new TestCaseGeneratorMmtPartialBlock(_oracle);
                            case "mct":
                                return new TestCaseGeneratorMct(_oracle);
                        }
                    }

                    break;
            }


            return new TestCaseGeneratorNull();
        }
    }
}
