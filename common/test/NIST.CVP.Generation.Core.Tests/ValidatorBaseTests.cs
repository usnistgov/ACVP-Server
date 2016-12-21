using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture]
    public class ValidatorBaseTests
    {
        Mock<IDynamicParser> _mockDynamicParser;
        string _testPath;
        string[] _testVectorFileNames = new string[]
        {
                @"\testResults.json",
                @"\answer.json",
                @"\prompt.json"
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\validatorBaseTests\");
        }

        [SetUp]
        public void Setup()
        {
            _mockDynamicParser = new Mock<IDynamicParser>();
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        public void ShouldParseSuccessfullyAndCreateValidationFile()
        {
            var subject = new FakeSuccessValidatorBase(_mockDynamicParser.Object);
            string localTestPath = GetUniqueTestPath(_testPath);

            _mockDynamicParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(new ParseResponse<object>(new object()));

            subject
                .Validate(
                $"{localTestPath}{_testVectorFileNames[0]}", 
                $"{localTestPath}{_testVectorFileNames[1]}", 
                $"{localTestPath}{_testVectorFileNames[2]}");

            var expectedFile = $@"{localTestPath}\validation.json";
            Assert.IsTrue(File.Exists(expectedFile));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ShouldHandleFailedFileParse(int failFileIndex)
        {
            var subject = new FakeSuccessValidatorBase(_mockDynamicParser.Object);
            string localTestPath = GetUniqueTestPath(_testPath);

            var failFile = $"{localTestPath}{_testVectorFileNames[failFileIndex]}";

            _mockDynamicParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(new ParseResponse<object>(new object()));
            _mockDynamicParser
                .Setup(s => s.Parse(failFile))
                .Returns(new ParseResponse<object>(failFile));

            var result = subject
                .Validate(
                    $"{localTestPath}{_testVectorFileNames[0]}",
                    $"{localTestPath}{_testVectorFileNames[1]}",
                    $"{localTestPath}{_testVectorFileNames[2]}"
                );

            Assert.AreEqual(failFile, result.ErrorMessage);
        }

        [Test]
        public void ShouldHandleFailedFileSaveGracefully()
        {
            var subject = new FakeSuccessValidatorBase(_mockDynamicParser.Object);
            string localTestPath = GetUniqueTestPath(_testPath);

            string validationFileLocation = $"{localTestPath}\\validation.json";
            string expectedMessage = $"Could not create {validationFileLocation}";

            _mockDynamicParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(new ParseResponse<object>(new object()));

            using (FileStream fs = File.Create(validationFileLocation))
            {
                var result = subject
                    .Validate(
                        $"{localTestPath}{_testVectorFileNames[0]}",
                        $"{localTestPath}{_testVectorFileNames[1]}",
                        $"{localTestPath}{_testVectorFileNames[2]}"
                    );

                Assert.AreEqual(expectedMessage, result.ErrorMessage);
            }
        }

        private string GetUniqueTestPath(string testPath)
        {
            var directoryToCreate = Path.Combine(testPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(directoryToCreate);

            return directoryToCreate;
        }
    }
}
