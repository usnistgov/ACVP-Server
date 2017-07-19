using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CFB1.Tests
{
    [TestFixture, UnitTest]
    public class BitOrientedGeneratorTests
    {
        public class FakeBitOrientedGenerator : BitOrientedGenerator<FakeParameters, FakeTestVectorSet>
        {
            public FakeBitOrientedGenerator(ITestVectorFactory<FakeParameters> testVectorFactory, IParameterParser<FakeParameters> parameterParser, IParameterValidator<FakeParameters> parameterValidator, ITestCaseGeneratorFactoryFactory<FakeTestVectorSet> iTestCaseGeneratorFactoryFactory) : base(testVectorFactory, parameterParser, parameterValidator, iTestCaseGeneratorFactoryFactory)
            {
            }

            public IList<JsonConverter> GetJsonConverters()
            {
                return _jsonConverters;
            }
        }

        public class FakeBGenerator : Generator<FakeParameters, FakeTestVectorSet>
        {
            public FakeBGenerator(ITestVectorFactory<FakeParameters> testVectorFactory, IParameterParser<FakeParameters> parameterParser, IParameterValidator<FakeParameters> parameterValidator, ITestCaseGeneratorFactoryFactory<FakeTestVectorSet> iTestCaseGeneratorFactoryFactory) : base(testVectorFactory, parameterParser, parameterValidator, iTestCaseGeneratorFactoryFactory)
            {
            }

            public IList<JsonConverter> GetJsonConverters()
            {
                return _jsonConverters;
            }
        }

        [Test]
        [TestCase(typeof(BitOrientedBitStringConverter))]
        [TestCase(typeof(BitstringConverter))]
        public void ShouldReturnJsonConverters(Type expectedType)
        {
            var subject = new FakeBitOrientedGenerator(null, null, null, null);
            var converters = subject.GetJsonConverters();

            Assert.IsTrue(converters.Any(a => a.GetType() == expectedType));
        }

        [Test]
        public void ShouldNotReturnBitOrientedConverterInGenerator()
        {
            var expectedType = typeof(BitOrientedBitStringConverter);
            var subject = new FakeBGenerator(null, null, null, null);
            var converters = subject.GetJsonConverters();

            Assert.IsTrue(converters.All(a => a.GetType() != expectedType));
        }
    }
}
