using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
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
                    Algorithm = "AES",
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
                result = subject.Generate($"{_testPath}\\{fileNameRoot.ToString()}.json");
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
            var subject = new FakeGenerator();
            var testVectorSet = new FakeTestVectorSet();
            var result = subject.SaveOutputsTester(_testPath, testVectorSet);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldNotSaveOutputsForEachResolverWithInvalidPath()
        {
            var subject = new FakeGenerator();
            var testVectorSet = new FakeTestVectorSet();
            var jsonPath = Path.Combine(_testPath, "fakePath/");
            var result = subject.SaveOutputsTester(jsonPath, testVectorSet);
            Assert.IsFalse(result.Success);
        }

        private Generator<FakeParameters,FakeTestVectorSet, FakeTestGroup, FakeTestCase> GetSystem(
            ITestVectorFactory<FakeParameters> testVectorFactory, 
            IParameterParser<FakeParameters> parameterParser, 
            IParameterValidator<FakeParameters> parameterValidator, 
            ITestCaseGeneratorFactoryFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase> testCaseGeneratorFactoryFactory
        )
        {
            return new Generator<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(testVectorFactory, parameterParser, parameterValidator, testCaseGeneratorFactoryFactory);
        }

        private Generator<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase> GetSystem(MockedSystemDependencies mocks)
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
            public Mock<ITestCaseGeneratorFactoryFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase>> MockITestCaseGeneratorFactoryFactory { get; set; } = new Mock<ITestCaseGeneratorFactoryFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase>>();
            public Mock<IParameterParser<FakeParameters>> MockIParameterParser { get; set; } = new Mock<IParameterParser<FakeParameters>>();
            public Mock<IParameterValidator<FakeParameters>> MockIParameterValidator { get; set; } = new Mock<IParameterValidator<FakeParameters>>();
        }
    }
}
