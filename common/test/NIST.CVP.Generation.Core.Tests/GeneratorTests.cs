using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using FakeParameters = NIST.CVP.Generation.Core.Tests.Fakes.FakeParameters;
using FakeTestVectorSet = NIST.CVP.Generation.Core.Tests.Fakes.FakeTestVectorSet;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture, UnitTest]
    public class GeneratorTests
    {
        private string _testPath;

        private Mock<ITestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>> _mockITestVectorFactory;
        private Mock<ITestCaseGeneratorFactoryFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase>> _mockITestCaseGeneratorFactoryFactory;
        private Mock<IParameterParser<FakeParameters>> _mockIParameterParser;
        private Mock<IParameterValidator<FakeParameters>> _mockIParameterValidator;
        private Mock<IVectorSetSerializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase>> _mockIVectorSetSerializer;

        private Generator<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase> _subject;

        [SetUp]
        public void Setup()
        {
            _mockITestVectorFactory = new Mock<ITestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>>();
            _mockITestCaseGeneratorFactoryFactory = new Mock<ITestCaseGeneratorFactoryFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase>>();
            _mockIParameterParser = new Mock<IParameterParser<FakeParameters>>();
            _mockIParameterValidator = new Mock<IParameterValidator<FakeParameters>>();
            _mockIVectorSetSerializer = new Mock<IVectorSetSerializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase>>();

            _mockIParameterParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(() => new ParseResponse<FakeParameters>(new FakeParameters()));
            _mockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<FakeParameters>()))
                .Returns(() => new ParameterValidateResponse());
            _mockITestVectorFactory
                .Setup(s => s.BuildTestVectorSet(It.IsAny<FakeParameters>()))
                .Returns(() => new FakeTestVectorSet()
                {
                    Algorithm = "AES",
                    TestGroups = new List<FakeTestGroup>()
                    {
                        new FakeTestGroup()
                    }
                });
            _mockITestCaseGeneratorFactoryFactory
                .Setup(s => s.BuildTestCases(It.IsAny<FakeTestVectorSet>()))
                .Returns(() => new GenerateResponse());
            _mockIVectorSetSerializer
                .Setup(s => s.Serialize(It.IsAny<FakeTestVectorSet>(), Projection.Server))
                .Returns(() => "");

            _subject = new Generator<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(
                _mockITestVectorFactory.Object,
                _mockIParameterParser.Object,
                _mockIParameterValidator.Object,
                _mockITestCaseGeneratorFactoryFactory.Object,
                _mockIVectorSetSerializer.Object
            );
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\GeneratorTests\");
            Directory.CreateDirectory(_testPath);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        public void GenerateShouldReturnErrorResponseWhenParametersNotParsedSuccessfully()
        {
            string errorMessage = "Invalid Parameters";
            _mockIParameterParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(() => new ParseResponse<FakeParameters>(errorMessage));

            var result = _subject.Generate(string.Empty);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void GenerateShouldReturnErrorResponseWhenParametersNotValidatedSuccessfully()
        {
            string errorMessage = "Invalid Parameter Validation";
            _mockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<FakeParameters>()))
                .Returns(() => new ParameterValidateResponse(errorMessage));

            var result = _subject.Generate(string.Empty);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void GenerateShouldReturnErrorResponseWhenInvalidTestCaseResponse()
        {
            string errorMessage = "Invalid Test Case Response";
            _mockITestCaseGeneratorFactoryFactory
                .Setup(s => s.BuildTestCases(It.IsAny<FakeTestVectorSet>()))
                .Returns(() => new GenerateResponse(errorMessage));

            var result = _subject.Generate(string.Empty);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void GenerateShouldReturnSuccessWithValidCalls()
        {
            GenerateResponse result = null;
            Guid fileNameRoot = Guid.NewGuid();

            try
            {
                result = _subject.Generate($"{_testPath}\\{fileNameRoot.ToString()}.json");
            }
            finally
            {
                // Find and delete files as a result of the test
                List<string> files = new List<string>();
                files = Directory.GetFiles(_testPath, $"{fileNameRoot}*").ToList();

                if (files.Count <= 4)
                {
                    foreach(var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldProperlySaveOutputsForEachResolverWithValidFiles()
        {
            var subject = new FakeGenerator(
                _mockITestVectorFactory.Object, 
                _mockIParameterParser.Object, 
                _mockIParameterValidator.Object, 
                _mockITestCaseGeneratorFactoryFactory.Object, 
                _mockIVectorSetSerializer.Object
            );
            var testVectorSet = new FakeTestVectorSet();
            var result = subject.SaveOutputsTester(_testPath, testVectorSet);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldNotSaveOutputsForEachResolverWithInvalidPath()
        {
            var subject = new FakeGenerator(
                _mockITestVectorFactory.Object,
                _mockIParameterParser.Object,
                _mockIParameterValidator.Object,
                _mockITestCaseGeneratorFactoryFactory.Object,
                _mockIVectorSetSerializer.Object
            );
            var testVectorSet = new FakeTestVectorSet();
            var jsonPath = Path.Combine(_testPath, "fakePath/");
            var result = subject.SaveOutputsTester(jsonPath, testVectorSet);
            Assert.IsFalse(result.Success);
        }
    }
}
