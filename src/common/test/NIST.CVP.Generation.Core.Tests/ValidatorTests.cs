using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Moq;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

// TODO fix, things are weird with the refactor, no more projections within the TVS

namespace NIST.CVP.Generation.Core.Tests
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

    //                var tests = new List<ITestCase>();
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
    //            mocks.MockIDynamicParser.Object, 
    //            mocks.MockIResultValidator.Object, 
    //            mocks.MockITestCaseValidatorFactory.Object, 
    //            mocks.MockITestReconstitutor.Object
    //        );
    //        string localTestPath = GetUniqueTestPath(_testPath);

    //        mocks.MockIDynamicParser
    //            .Setup(s => s.Parse(It.IsAny<string>()))
    //            .Returns(new ParseResponse<object>(new object()));

    //        subject
    //            .Validate(
    //            $"{localTestPath}{_testVectorFileNames[0]}",
    //            $"{localTestPath}{_testVectorFileNames[1]}");

    //        var expectedFile = $@"{localTestPath}\validation.json";
    //        Assert.IsTrue(File.Exists(expectedFile));
    //    }

    //    [Test]
    //    [TestCase(0)]
    //    [TestCase(1)]
    //    public void ShouldHandleFailedFileParse(int failFileIndex)
    //    {
    //        var mocks = new MockedSystemDependencies();
    //        var subject = new FakeSuccessValidator(
    //            mocks.MockIDynamicParser.Object,
    //            mocks.MockIResultValidator.Object,
    //            mocks.MockITestCaseValidatorFactory.Object,
    //            mocks.MockITestReconstitutor.Object
    //        );
    //        string localTestPath = GetUniqueTestPath(_testPath);

    //        var failFile = $"{localTestPath}{_testVectorFileNames[failFileIndex]}";

    //        mocks.MockIDynamicParser
    //            .Setup(s => s.Parse(It.IsAny<string>()))
    //            .Returns(new ParseResponse<object>(new object()));
    //        mocks.MockIDynamicParser
    //            .Setup(s => s.Parse(failFile))
    //            .Returns(new ParseResponse<object>(failFile));

    //        var result = subject
    //            .Validate(
    //                $"{localTestPath}{_testVectorFileNames[0]}",
    //                $"{localTestPath}{_testVectorFileNames[1]}"
    //            );

    //        Assert.AreEqual(failFile, result.ErrorMessage);
    //    }

    //    [Test]
    //    public void ShouldHandleFailedFileSaveGracefully()
    //    {
    //        var mocks = new MockedSystemDependencies();
    //        var subject = new FakeSuccessValidator(
    //            mocks.MockIDynamicParser.Object,
    //            mocks.MockIResultValidator.Object,
    //            mocks.MockITestCaseValidatorFactory.Object,
    //            mocks.MockITestReconstitutor.Object
    //        );
    //        string localTestPath = GetUniqueTestPath(_testPath);

    //        string validationFileLocation = $"{localTestPath}\\validation.json";
    //        string expectedMessage = $"Could not create {validationFileLocation}";

    //        mocks.MockIDynamicParser
    //            .Setup(s => s.Parse(It.IsAny<string>()))
    //            .Returns(new ParseResponse<object>(new object()));

    //        using (FileStream fs = File.Create(validationFileLocation))
    //        {
    //            var result = subject
    //                .Validate(
    //                    $"{localTestPath}{_testVectorFileNames[0]}",
    //                    $"{localTestPath}{_testVectorFileNames[1]}"
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
    //        var result = subject.Validate($"C:\\{Guid.NewGuid()}\\result.json", $"C:\\{Guid.NewGuid()}\\answer.json");
    //        Assert.IsNotNull(result);
    //        Assert.IsFalse(result.Success);
    //    }

    //    [Test]
    //    [TestCase("")]
    //    [TestCase(null)]
    //    public void ShouldReturnErrorForNullOrEmptyPath(string path)
    //    {
    //        var subject = GetSubject(true, false);
    //        var result = subject.Validate(path, path);
    //        Assert.IsNotNull(result);
    //        Assert.IsFalse(result.Success);
    //    }

    //    [Test]
    //    public void ShouldValidateGoodResults()
    //    {
    //        var subject = GetSubject(false, false);
    //        var testPath = GetTestPath();

    //        var result = subject.Validate(Path.Combine(testPath, "result.json"), Path.Combine(testPath, "answer.json"));
    //        Assert.IsNotNull(result);
    //        Assert.IsTrue(result.Success);
    //    }

    //    [Test]
    //    public void ShouldValidateCleanlyIfFailedValidation()
    //    {
    //        var subject = GetSubject(false, true);
    //        var testPath = GetTestPath();

    //        var result = subject.Validate(Path.Combine(testPath, "result.json"), Path.Combine(testPath, "answer.json"));
    //        Assert.IsNotNull(result);
    //        Assert.IsTrue(result.Success);
    //    }

    //    private Validator<FakeTestVectorSet, FakeTestGroup, FakeTestCase> GetSubject(bool parserFail, bool validationFail)
    //    {
    //        var mocks = new MockedSystemDependencies();
    //        if (parserFail)
    //        {
    //            mocks.MockIDynamicParser
    //            .Setup(s => s.Parse(It.IsAny<string>()))
    //            .Returns(new ParseResponse<dynamic>("Parser failed."));
    //        }
    //        else
    //        {
    //          var vectorSet = GetTestTestVectorSet();
    //          mocks.MockIDynamicParser
    //            .Setup(s => s.Parse(It.Is<string>( p=> p.Contains("answer.json") ||  p.Contains("prompt.json"))))
    //            .Returns(new ParseResponse<dynamic>((dynamic)vectorSet.ToDynamic()));
    //          mocks.MockIDynamicParser
    //            .Setup(s => s.Parse(It.Is<string>(p => p.Contains("result.json"))))
    //            .Returns(new ParseResponse<dynamic>(GetTestResults()));
    //        }

    //        if (validationFail)
    //        {
    //            mocks.MockIResultValidator
    //                .Setup(s => s.ValidateResults(It.IsAny<List<ITestCaseValidator<FakeTestCase>>>(), It.IsAny<List<FakeTestGroup>>()))
    //                .Returns(new TestVectorValidation { Validations = new List<TestCaseValidation> {new TestCaseValidation { Result = Disposition.Failed } } });
    //        }
    //        else
    //        {
    //            mocks.MockIResultValidator
    //               .Setup(s => s.ValidateResults(It.IsAny<List<ITestCaseValidator<FakeTestCase>>>(), It.IsAny<List<FakeTestGroup>>()))
    //               .Returns(new TestVectorValidation { Validations = new List<TestCaseValidation> { new TestCaseValidation { Result = Disposition.Passed } } });
    //        }

    //        return new Validator<FakeTestVectorSet, FakeTestGroup, FakeTestCase>(mocks.MockIDynamicParser.Object, mocks.MockIResultValidator.Object, mocks.MockITestCaseValidatorFactory.Object, mocks.MockITestReconstitutor.Object);
    //    }

    //    private dynamic GetTestResults(int groups = 2)
    //    {
    //        var vectorSet = GetTestTestVectorSet(groups);
    //        dynamic updateObject = new ExpandoObject();
    //        ((IDictionary<string, object>)updateObject).Add("testResults", vectorSet.ResultProjection);
    //        return updateObject;
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
    //        public Mock<IResultValidator<FakeTestGroup, FakeTestCase>> MockIResultValidator { get; } = new Mock<IResultValidator<FakeTestGroup, FakeTestCase>>();
    //        public Mock<IDynamicParser> MockIDynamicParser { get; } = new Mock<IDynamicParser>();
    //        public Mock<ITestCaseValidatorFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase>> MockITestCaseValidatorFactory { get; } = new Mock<ITestCaseValidatorFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase>>();
    //        public Mock<ITestReconstitutor<FakeTestVectorSet, FakeTestGroup, FakeTestCase>> MockITestReconstitutor { get; } = new Mock<ITestReconstitutor<FakeTestVectorSet, FakeTestGroup, FakeTestCase>>();
    //    }
    //}
}
