using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.Fakes;
using NUnit.Framework;
using FakeTestVectorSet = NIST.CVP.Tests.Core.Fakes.FakeTestVectorSet;

namespace NIST.CVP.Generation.DRBG.Tests
{
    // @@@ TODO Consider writing fake implementations for some/all of the dependencies for valid/invalid invokes.
    // Mocking getting too complex.
    [TestFixture]
    public class GeneratorTests
    {

        private const string _WORKING_PATH = @"C:\temp";

        [OneTimeSetUp]
        public void Setup()
        {
            if (!Directory.Exists(_WORKING_PATH))
            {
                Directory.CreateDirectory(_WORKING_PATH);
            }
        }

        [Test]
        public void GenerateShouldReturnErrorResponseWhenParametersNotParsedSuccessfully()
        {
            string errorMessage = "Invalid Parameters";
            var mocks = new MockedSystemDependencies();
            mocks.MockIParameterParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(new Generation.Core.ParseResponse<FakeParameters>(errorMessage));
            var subject = GetSystem(mocks);

            var result = subject.Generate(It.IsAny<string>());

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void GenerateShouldReturnErrorResponseWhenParametersNotValidatedSuccessfully()
        {
            string errorMessage = "Invalid Parameter Validation";
            var mocks = new MockedSystemDependencies();
            mocks.MockIParameterParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(new Generation.Core.ParseResponse<FakeParameters>(new FakeParameters()));
            mocks.MockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<FakeParameters>()))
                .Returns(new Generation.Core.ParameterValidateResponse(errorMessage));
            var subject = GetSystem(mocks);

            var result = subject.Generate(It.IsAny<string>());

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void GenerateShouldReturnErrorResponseWhenInvalidTestCaseResponse()
        {
            string errorMessage = "Invalid Test Case Response";
            var mocks = new MockedSystemDependencies();
            mocks.MockIParameterParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(new ParseResponse<FakeParameters>(new FakeParameters()));
            mocks.MockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<FakeParameters>()))
                .Returns(new ParameterValidateResponse());
            mocks.MockITestVectorFactory
                .Setup(s => s.BuildTestVectorSet(It.IsAny<FakeParameters>()))
                .Returns(new FakeTestVectorSet());
            mocks.MockITestCaseGeneratorFactoryFactory
                .Setup(s => s.BuildTestCases(It.IsAny<FakeTestVectorSet>()))
                .Returns(new GenerateResponse(errorMessage));

            var subject = GetSystem(mocks);

            var result = subject.Generate(It.IsAny<string>());

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void GenerateShouldReturnSuccessWithValidCalls()
        {
            var mocks = new MockedSystemDependencies();
            mocks.MockIParameterParser
                .Setup(s => s.Parse(It.IsAny<string>()))
                .Returns(new ParseResponse<FakeParameters>(new FakeParameters()));
            mocks.MockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<FakeParameters>()))
                .Returns(new ParameterValidateResponse());
            mocks.MockITestVectorFactory
                .Setup(s => s.BuildTestVectorSet(It.IsAny<FakeParameters>()))
                .Returns(new FakeTestVectorSet()
                {
                    Algorithm = "DRBG",
                    TestGroups = new List<ITestGroup>()
                    {
                        new FakeTestGroup()
                    }
                });
            mocks.MockITestCaseGeneratorFactoryFactory
                .Setup(s => s.BuildTestCases(It.IsAny<FakeTestVectorSet>()))
                .Returns(new GenerateResponse());

            var subject = GetSystem(mocks);

            GenerateResponse result = null;
            Guid fileNameRoot = Guid.NewGuid();

            try
            {
                result = subject.Generate($"{_WORKING_PATH}\\{fileNameRoot.ToString()}.json");
            }
            finally
            {
                // Find and delete files as a result of the test
                List<string> files = new List<string>();
                files = Directory.GetFiles(_WORKING_PATH, $"{fileNameRoot}*").ToList();

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

        private Crypto.AES.Generator<FakeParameters,FakeTestVectorSet> GetSystem(ITestVectorFactory<FakeParameters> testVectorFactory, IParameterParser<FakeParameters> parameterParser, IParameterValidator<FakeParameters> parameterValidator, ITestCaseGeneratorFactoryFactory<FakeTestVectorSet> testCaseGeneratorFactoryFactory)
        {
            return new Crypto.AES.Generator<FakeParameters, FakeTestVectorSet>(testVectorFactory, parameterParser, parameterValidator, testCaseGeneratorFactoryFactory);
        }

        private Crypto.AES.Generator<FakeParameters, FakeTestVectorSet> GetSystem(MockedSystemDependencies mocks)
        {
            return GetSystem(
                mocks.MockITestVectorFactory.Object,
                mocks.MockIParameterParser.Object,
                mocks.MockIParameterValidator.Object,
                mocks.MockITestCaseGeneratorFactoryFactory.Object
            );
        }

        private class MockedSystemDependencies
        {
            public Mock<ITestVectorFactory<FakeParameters>> MockITestVectorFactory { get; set; } = new Mock<ITestVectorFactory<FakeParameters>>();
            public Mock<ITestCaseGeneratorFactoryFactory<FakeTestVectorSet>> MockITestCaseGeneratorFactoryFactory { get; set; } = new Mock<ITestCaseGeneratorFactoryFactory<FakeTestVectorSet>>();
            public Mock<IParameterParser<FakeParameters>> MockIParameterParser { get; set; } = new Mock<IParameterParser<FakeParameters>>();
            public Mock<IParameterValidator<FakeParameters>> MockIParameterValidator { get; set; } = new Mock<IParameterValidator<FakeParameters>>();
        }
    }
}
