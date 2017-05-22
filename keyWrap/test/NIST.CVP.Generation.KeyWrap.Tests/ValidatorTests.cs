using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using FakeTestCase = NIST.CVP.Tests.Core.Fakes.FakeTestCase;
using FakeTestVectorSet = NIST.CVP.Tests.Core.Fakes.FakeTestVectorSet;

namespace NIST.CVP.Generation.KeyWrap.Tests
{
    [TestFixture, UnitTest]
    public class ValidatorTests
    {
        private const string _WORKING_PATH = @"C:\temp";
        private TestDataMother _tdm = new TestDataMother();

        [OneTimeSetUp]
        public void Setup()
        {
            if (!Directory.Exists(_WORKING_PATH))
            {
                Directory.CreateDirectory(_WORKING_PATH);
            }
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = GetSubject(true, false);
            var result = subject.Validate($"C:\\{Guid.NewGuid()}\\result.json", $"C:\\{Guid.NewGuid()}\\answer.json", $"C:\\{Guid.NewGuid()}\\prompt.json");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = GetSubject(true, false);
            var result = subject.Validate(path, path, path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldValidateGoodResults()
        {
            var subject = GetSubject(false, false);
            var testPath = GetTestPath();

            var result = subject.Validate(Path.Combine(testPath, "result.json"), Path.Combine(testPath, "answer.json"), Path.Combine(testPath, "prompt.json"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldValidateCleanlyIfFailedValidation()
        {
            var subject = GetSubject(false, true);
            var testPath = GetTestPath();

            var result = subject.Validate(Path.Combine(testPath, "result.json"), Path.Combine(testPath, "answer.json"), Path.Combine(testPath, "prompt.json"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        private Validator<FakeTestVectorSet, FakeTestCase> GetSubject(bool parserFail, bool validationFail)
        {
            var mocks = new MockedSystemDependencies();
            if (parserFail)
            {
                mocks.MockIDynamicParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(new ParseResponse<dynamic>("Parser failed."));
            }
            else
            {
              var vectorSet = GetTestTestVectorSet();
              mocks.MockIDynamicParser
                .Setup(s => s.Parse(It.Is<string>( p=> p.Contains("answer.json") ||  p.Contains("prompt.json"))))
                .Returns(new ParseResponse<dynamic>((dynamic)vectorSet.ToDynamic()));
              mocks.MockIDynamicParser
                .Setup(s => s.Parse(It.Is<string>(p => p.Contains("result.json"))))
                .Returns(new ParseResponse<dynamic>(GetTestResults()));
            }

            if (validationFail)
            {
                mocks.MockIResultValidator
                    .Setup(s => s.ValidateResults(It.IsAny<List<ITestCaseValidator<FakeTestCase>>>(), It.IsAny<List<FakeTestCase>>()))
                    .Returns(new TestVectorValidation { Validations = new List<TestCaseValidation> {new TestCaseValidation { Result = "failed"} } });
            }
            else
            {
                mocks.MockIResultValidator
                   .Setup(s => s.ValidateResults(It.IsAny<List<ITestCaseValidator<FakeTestCase>>>(), It.IsAny<List<FakeTestCase>>()))
                   .Returns(new TestVectorValidation { Validations = new List<TestCaseValidation> { new TestCaseValidation { Result = "passed" } } });
            }

            return new Validator<FakeTestVectorSet, FakeTestCase>(mocks.MockIDynamicParser.Object, mocks.MockIResultValidator.Object, mocks.MockITestCaseValidatorFactory.Object, mocks.MockITestReconstitutor.Object);
        }

        private dynamic GetTestResults(int groups = 2)
        {
            var vectorSet = GetTestTestVectorSet(groups);
            dynamic updateObject = new ExpandoObject();
            ((IDictionary<string, object>)updateObject).Add("testResults", vectorSet.ResultProjection);
            return updateObject;
        }

        private FakeTestVectorSet GetTestTestVectorSet(int groups = 2)
        {
            var vectorSet = new FakeTestVectorSet { Algorithm = "DRBG" };
            var testGroups = _tdm.GetTestGroups(groups);
            vectorSet.TestGroups = testGroups.Select(g => (ITestGroup)g).ToList();
            return vectorSet;
        }

        private string GetTestPath()
        {
            var guid = Guid.NewGuid();
           
            var testDir = Path.Combine(_WORKING_PATH, guid.ToString());
            Directory.CreateDirectory(testDir);
            return testDir;
        }

        private class MockedSystemDependencies
        {
            public Mock<IResultValidator<FakeTestCase>> MockIResultValidator { get; set; } = new Mock<IResultValidator<FakeTestCase>>();
            public Mock<IDynamicParser> MockIDynamicParser { get; set; } = new Mock<IDynamicParser>();
            public Mock<ITestCaseValidatorFactory<FakeTestVectorSet, FakeTestCase>> MockITestCaseValidatorFactory { get; set; } = new Mock<ITestCaseValidatorFactory<FakeTestVectorSet, FakeTestCase>>();
            public Mock<ITestReconstitutor<FakeTestVectorSet, FakeTestCase>> MockITestReconstitutor { get; set; } = new Mock<ITestReconstitutor<FakeTestVectorSet, FakeTestCase>>();

        }
    }
}
