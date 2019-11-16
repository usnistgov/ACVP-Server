using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NIST.CVP.Common.Enums;
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

        [Test]
        public void GenerateShouldReturnErrorResponseWhenParametersNotParsedSuccessfully()
        {
            string errorMessage = "Invalid Parameters";
            _mockIParameterParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(() => new ParseResponse<FakeParameters>(errorMessage));

            var result = _subject.Generate(new GenerateRequest(string.Empty));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
            Assert.AreEqual(StatusCode.ParameterError, result.StatusCode);
        }

        [Test]
        public void GenerateShouldReturnErrorResponseWhenParametersNotValidatedSuccessfully()
        {
            var errorMessage = new List<string>() { "Invalid Parameter Validation" };
            _mockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<FakeParameters>()))
                .Returns(() => new ParameterValidateResponse(errorMessage));

            var result = _subject.Generate(new GenerateRequest(string.Empty));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage.First(), result.ErrorMessage);
            Assert.AreEqual(StatusCode.ParameterValidationError, result.StatusCode);
        }

        [Test]
        public void GenerateShouldReturnErrorResponseWhenInvalidTestCaseResponse()
        {
            string errorMessage = "Invalid Test Case Response";
            _mockITestCaseGeneratorFactoryFactory
                .Setup(s => s.BuildTestCases(It.IsAny<FakeTestVectorSet>()))
                .Returns(() => new GenerateResponse(errorMessage, StatusCode.TestCaseGeneratorError));

            var result = _subject.Generate(new GenerateRequest(string.Empty));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
            Assert.AreEqual(StatusCode.TestCaseGeneratorError, result.StatusCode);
        }

        [Test]
        public void GenerateShouldReturnSuccessWithValidCalls()
        {
            GenerateResponse result = null;
            var fileNameRoot = Guid.NewGuid();
            result = _subject.Generate(new GenerateRequest(string.Empty));

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(StatusCode.Success, result.StatusCode);
        }
    }
}
