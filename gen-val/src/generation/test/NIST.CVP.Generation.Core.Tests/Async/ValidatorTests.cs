using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

// TODO fix, things are weird with the refactor, no more projections within the TVS

namespace NIST.CVP.Generation.Core.Tests.Async
{
    //[TestFixture, UnitTest]
    //public class ValidatorTests
    //{
    //    private string _testPath;
    //    private TestDataMother _tdm = new TestDataMother();

    //    private class TestDataMother
    //    {
    //        public List<FakeTestGroup> GetTestGroups(int groups = 1, string direction = "encrypt", bool failureTest = false)
    //        {
    //            var testGroups = new List<FakeTestGroup>();
    //            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
    //            {

    //                var tests = new List<FakeTestCase>();
    //                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
    //                {
    //                    tests.Add(new FakeTestCase());
    //                }

    //                testGroups.Add(
    //                    new FakeTestGroup()
    //                );
    //            }
    //            return testGroups;
    //        }
    //    }

    //    private readonly string[] _testVectorFileNames = new string[]
    //    {
    //            @"\testResults.json",
    //            @"\answer.json"
    //    };

    //    [OneTimeSetUp]
    //    public void OneTimeSetUp()
    //    {
    //        _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\validatorBaseTests\");
    //    }

    //    [OneTimeTearDown]
    //    public void Teardown()
    //    {
    //        Directory.Delete(_testPath, true);
    //    }

    //    [Test]
    //    public void ShouldParseSuccessfullyAndCreateValidationFile()
    //    {
    //        var mocks = new MockedSystemDependencies();
    //        var subject = new FakeSuccessValidator(
    //            mocks.MockIResultValidator.Object,
    //            mocks.MockITestCaseValidatorFactory.Object,
    //            mocks.MockIVectorSetDeserializer.Object
    //        );
    //        string localTestPath = GetUniqueTestPath(_testPath);

    //        subject
    //            .Validate(
    //                $"{localTestPath}{_testVectorFileNames[0]}",
    //                $"{localTestPath}{_testVectorFileNames[1]}",
    //                true
    //            );

    //        var expectedFile = $@"{localTestPath}\validation.json";
    //        Assert.IsTrue(File.Exists(expectedFile));
    //    }

    //    [Test]
    //    public void ShouldHandleFailedFileSaveGracefully()
    //    {
    //        var mocks = new MockedSystemDependencies();
    //        var subject = new FakeSuccessValidator(
    //            mocks.MockIResultValidator.Object,
    //            mocks.MockITestCaseValidatorFactory.Object,
    //            mocks.MockIVectorSetDeserializer.Object
    //        );
    //        string localTestPath = GetUniqueTestPath(_testPath);

    //        string validationFileLocation = $"{localTestPath}\\validation.json";
    //        string expectedMessage = $"Could not create {validationFileLocation}";

    //        mocks.MockIVectorSetDeserializer
    //            .Setup(s => s.Deserialize(It.IsAny<string>()))
    //            .Returns(new FakeTestVectorSet());

    //        using (FileStream fs = File.Create(validationFileLocation))
    //        {
    //            var result = subject
    //                .Validate(
    //                    $"{localTestPath}{_testVectorFileNames[0]}",
    //                    $"{localTestPath}{_testVectorFileNames[1]}",
    //                    true
    //                );

    //            Assert.AreEqual(expectedMessage, result.ErrorMessage);
    //        }
    //    }

    //    private string GetUniqueTestPath(string testPath)
    //    {
    //        var directoryToCreate = Path.Combine(testPath, Guid.NewGuid().ToString());
    //        Directory.CreateDirectory(directoryToCreate);

    //        return directoryToCreate;
    //    }

    //    [Test]
    //    public void ShouldReturnErrorForNonExistentPath()
    //    {
    //        var subject = GetSubject(true, false);
    //        var result = subject.Validate($"C:\\{Guid.NewGuid()}\\result.json", $"C:\\{Guid.NewGuid()}\\answer.json", true);
    //        Assert.IsNotNull(result);
    //        Assert.IsFalse(result.Success);
    //    }

    //    [Test]
    //    [TestCase("")]
    //    [TestCase(null)]
    //    public void ShouldReturnErrorForNullOrEmptyPath(string path)
    //    {
    //        var subject = GetSubject(true, false);
    //        var result = subject.Validate(path, path, true);
    //        Assert.IsNotNull(result);
    //        Assert.IsFalse(result.Success);
    //    }

    //    [Test]
    //    public void ShouldValidateGoodResults()
    //    {
    //        var subject = GetSubject(false, false);
    //        var testPath = GetTestPath();

    //        var result = subject.Validate(
    //            Path.Combine(testPath, "result.json"), 
    //            Path.Combine(testPath, "answer.json"),
    //            true
    //        );
    //        Assert.IsNotNull(result);
    //        Assert.IsTrue(result.Success);
    //    }

    //    [Test]
    //    public void ShouldValidateCleanlyIfFailedValidation()
    //    {
    //        var subject = GetSubject(false, true);
    //        var testPath = GetTestPath();

    //        var result = subject.Validate(
    //            Path.Combine(testPath, "result.json"), 
    //            Path.Combine(testPath, "answer.json"),
    //            true
    //        );
    //        Assert.IsNotNull(result);
    //        Assert.IsTrue(result.Success);
    //    }

    //    private ValidatorAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase> GetSubject(bool parserFail, bool validationFail)
    //    {
    //        var mocks = new MockedSystemDependencies();
    //        if (parserFail)
    //        {
    //            mocks.MockIVectorSetDeserializer
    //                .Setup(s => s.Deserialize(It.IsAny<string>()))
    //                .Throws(new Exception());
    //        }
    //        else
    //        {
    //            var vectorSet = GetTestTestVectorSet();
    //            mocks.MockIVectorSetDeserializer
    //              .Setup(s => s.Deserialize(It.Is<string>(p => p.Contains("answer.json") || p.Contains("prompt.json"))))
    //              .Returns(vectorSet);
    //            mocks.MockIVectorSetDeserializer
    //              .Setup(s => s.Deserialize(It.Is<string>(p => p.Contains("result.json"))))
    //              .Returns(vectorSet);
    //        }

    //        if (validationFail)
    //        {
    //            mocks.MockIResultValidator
    //                .Setup(s => s.ValidateResults(It.IsAny<IEnumerable<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>>(), It.IsAny<IEnumerable<FakeTestGroup>>(), It.IsAny<bool>()))
    //                .Returns(new TestVectorValidation { Validations = new List<TestCaseValidation> { new TestCaseValidation { Result = Disposition.Failed } } });
    //        }
    //        else
    //        {
    //            mocks.MockIResultValidator
    //                .Setup(s => s.ValidateResults(It.IsAny<IEnumerable<ITestCaseValidatorAsync<FakeTestGroup, FakeTestCase>>>(), It.IsAny<IEnumerable<FakeTestGroup>>(), It.IsAny<bool>()))
    //                .Returns(new TestVectorValidation { Validations = new List<TestCaseValidation> {new TestCaseValidation {Result = Disposition.Passed} } });
    //        }

    //        return new ValidatorAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>(mocks.MockIResultValidator.Object, mocks.MockITestCaseValidatorFactory.Object, mocks.MockIVectorSetDeserializer.Object);
    //    }

    //    private FakeTestVectorSet GetTestTestVectorSet(int groups = 2)
    //    {
    //        var vectorSet = new FakeTestVectorSet { Algorithm = "AES" };
    //        var testGroups = _tdm.GetTestGroups(groups);
    //        vectorSet.TestGroups = testGroups.Select(g => g).ToList();
    //        return vectorSet;
    //    }

    //    private string GetTestPath()
    //    {
    //        var guid = Guid.NewGuid();

    //        var testDir = Path.Combine(_testPath, guid.ToString());
    //        Directory.CreateDirectory(testDir);
    //        return testDir;
    //    }

    //    private class MockedSystemDependencies
    //    {
    //        public Mock<IResultValidatorAsync<FakeTestGroup, FakeTestCase>> MockIResultValidator { get; } = new Mock<IResultValidatorAsync<FakeTestGroup, FakeTestCase>>();
    //        public Mock<ITestCaseValidatorFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>> MockITestCaseValidatorFactory { get; } = new Mock<ITestCaseValidatorFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>>();
    //        public Mock<IVectorSetDeserializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase>> MockIVectorSetDeserializer { get; } = new Mock<IVectorSetDeserializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase>>();
    //    }
    //}
}
