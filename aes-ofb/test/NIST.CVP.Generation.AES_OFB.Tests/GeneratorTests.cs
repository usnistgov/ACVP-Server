using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_OFB.Tests
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
                .Returns(new Core.ParseResponse<Parameters>(errorMessage));
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
                .Returns(new Core.ParseResponse<Parameters>(new Parameters()));
            mocks.MockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<Parameters>()))
                .Returns(new Core.ParameterValidateResponse(errorMessage));
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
                .Returns(new ParseResponse<Parameters>(new Parameters()
                {
                    Algorithm = "AES-ECB",
                    KeyLen = new[] { 3 },
                    Mode = new[] { "encrypt" }
                }));
            mocks.MockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<Parameters>()))
                .Returns(new ParameterValidateResponse());
            mocks.MockITestVectorFactory
                .Setup(s => s.BuildTestVectorSet(It.IsAny<Parameters>()))
                .Returns(new TestVectorSet()
                {
                    Algorithm = "AES",
                    TestGroups = new List<ITestGroup>()
                    {
                        new TestGroup()
                        {
                            Function = "encrypt",
                            KeyLength = 3
                        }
                    }
                });
            mocks.MockITestCaseGeneratorFactoryFactory
                .Setup(s => s.BuildTestCases(It.IsAny<TestVectorSet>()))
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
                .Returns(new ParseResponse<Parameters>(new Parameters()
                {
                    Algorithm = "AES-ECB",
                    KeyLen = new[] { 3 },
                    Mode = new[] { "encrypt" }
                }));
            mocks.MockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<Parameters>()))
                .Returns(new ParameterValidateResponse());
            mocks.MockITestVectorFactory
                .Setup(s => s.BuildTestVectorSet(It.IsAny<Parameters>()))
                .Returns(new TestVectorSet()
                {
                    Algorithm = "AES",
                    TestGroups = new List<ITestGroup>()
                    {
                        new TestGroup()
                        {
                            Function = "encrypt",
                            KeyLength = 3
                        }
                    }
                });
            mocks.MockITestCaseGeneratorFactoryFactory
                .Setup(s => s.BuildTestCases(It.IsAny<TestVectorSet>()))
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

        private Generator GetSystem(ITestVectorFactory<Parameters> testVectorFactory, IParameterParser<Parameters> parameterParser, IParameterValidator<Parameters> parameterValidator, ITestCaseGeneratorFactoryFactory<TestVectorSet> testCaseGeneratorFactoryFactory)
        {
            return new Generator(testVectorFactory, parameterParser, parameterValidator, testCaseGeneratorFactoryFactory);
        }

        private Generator GetSystem(MockedSystemDependencies mocks)
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
            public Mock<ITestVectorFactory<Parameters>> MockITestVectorFactory { get; set; } = new Mock<ITestVectorFactory<Parameters>>();
            public Mock<ITestCaseGeneratorFactoryFactory<TestVectorSet>> MockITestCaseGeneratorFactoryFactory { get; set; } = new Mock<ITestCaseGeneratorFactoryFactory<TestVectorSet>>();
            public Mock<ITestCaseGenerator<TestGroup, TestCase>> MockITestCaseGenerator { get; set; } = new Mock<ITestCaseGenerator<TestGroup, TestCase>>();
            public Mock<IParameterParser<Parameters>> MockIParameterParser { get; set; } = new Mock<IParameterParser<Parameters>>();
            public Mock<IParameterValidator<Parameters>> MockIParameterValidator { get; set; } = new Mock<IParameterValidator<Parameters>>();
        }
    }
}
