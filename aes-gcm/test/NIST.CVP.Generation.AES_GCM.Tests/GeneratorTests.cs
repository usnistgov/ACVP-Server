using Moq;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_GCM.Tests
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
            var sut = GetSystem(mocks);

            var result = sut.Generate(It.IsAny<string>());

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
            var sut = GetSystem(mocks);

            var result = sut.Generate(It.IsAny<string>());

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
                    aadLen = new[] { 1 },
                    Algorithm = "AES-GCM",
                    ivGen = "external",
                    ivGenMode = "8.2.1",
                    ivLen = new[] { 2 },
                    KeyLen = new[] { 3 },
                    PtLen = new[] { 4 },
                    TagLen = new[] { 5 },
                    Mode = new[] { "encrypt" }
                }));
            mocks.MockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<Parameters>()))
                .Returns(new ParameterValidateResponse());
            mocks.MockITestVectorFactory
                .Setup(s => s.BuildTestVectorSet(It.IsAny<IParameters>()))
                .Returns(new TestVectorSet()
                {
                    Algorithm = "AES",
                    TestGroups = new List<ITestGroup>()
                    {
                        new TestGroup()
                        {
                            AADLength = 1,
                            Function = "encrypt",
                            IVGeneration = "external",
                            IVGenerationMode = "8.2.1",
                            IVLength = 2,
                            KeyLength = 3,
                            PTLength = 4,
                            TagLength = 5
                        }
                    }
                });
            mocks.MockITestCaseGenerator
                .Setup(s => s.Generate(It.IsAny<TestGroup>(), false))
                .Returns(new TestCaseGenerateResponse(errorMessage));
            mocks.MockITestCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(mocks.MockITestCaseGenerator.Object);

            var sut = GetSystem(mocks);

            var result = sut.Generate(It.IsAny<string>());

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
                    aadLen = new[] { 1 },
                    Algorithm = "AES-GCM",
                    ivGen = "external",
                    ivGenMode = "8.2.1",
                    ivLen = new[] { 2 },
                    KeyLen = new[] { 3 },
                    PtLen = new[] { 4 },
                    TagLen = new[] { 5 },
                    Mode = new[] { "encrypt" }
                }));
            mocks.MockIParameterValidator
                .Setup(s => s.Validate(It.IsAny<Parameters>()))
                .Returns(new ParameterValidateResponse());
            mocks.MockITestVectorFactory
                .Setup(s => s.BuildTestVectorSet(It.IsAny<IParameters>()))
                .Returns(new TestVectorSet()
                {
                    Algorithm = "AES",
                    TestGroups = new List<ITestGroup>()
                    {
                        new TestGroup()
                        {
                            AADLength = 1,
                            Function = "encrypt",
                            IVGeneration = "external",
                            IVGenerationMode = "8.2.1",
                            IVLength = 2,
                            KeyLength = 3,
                            PTLength = 4,
                            TagLength = 5
                        }
                    }
                });
            mocks.MockITestCaseGenerator
               .Setup(s => s.Generate(It.IsAny<TestGroup>(), false))
               .Returns(new TestCaseGenerateResponse(new TestCase()));
            mocks.MockITestCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(mocks.MockITestCaseGenerator.Object);

            var sut = GetSystem(mocks);

            GenerateResponse result = null;
            Guid fileNameRoot = Guid.NewGuid();

            try
            {
                result = sut.Generate($"{_WORKING_PATH}\\{fileNameRoot.ToString()}.json");
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

       

        private Generator GetSystem(ITestVectorFactory testVectorFactory, IParameterParser parameterParser, IParameterValidator parameterValidator, ITestCaseGeneratorFactory testCaseGeneratorFactory)
        {
            return new Generator(testVectorFactory, parameterParser, parameterValidator, testCaseGeneratorFactory);
        }

        private Generator GetSystem(MockedSystemDependencies mocks)
        {
            return GetSystem(
                mocks.MockITestVectorFactory.Object,
                mocks.MockIParameterParser.Object,
                mocks.MockIParameterValidator.Object,
                mocks.MockITestCaseGeneratorFactory.Object
            );
        }

        private class MockedSystemDependencies
        {
            public Mock<ITestVectorFactory> MockITestVectorFactory { get; set; } = new Mock<ITestVectorFactory>();
            public Mock<ITestCaseGeneratorFactory> MockITestCaseGeneratorFactory { get; set; } = new Mock<ITestCaseGeneratorFactory>();
            public Mock<ITestCaseGenerator> MockITestCaseGenerator { get; set; } = new Mock<ITestCaseGenerator>();
            public Mock<IParameterParser> MockIParameterParser { get; set; } = new Mock<IParameterParser>();
            public Mock<IParameterValidator> MockIParameterValidator { get; set; } = new Mock<IParameterValidator>();
           
            
        }
    }
}
