using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;
        private Mock<IBlockCipherEngine> _engine;
        private Mock<IBlockCipherEngineFactory> _engineFactory;
        
        [SetUp]
        public void Setup()
        {
            _engine = new Mock<IBlockCipherEngine>();
            _engineFactory = new Mock<IBlockCipherEngineFactory>();
            _engineFactory
                .Setup(s => s.GetSymmetricCipherPrimitive(It.IsAny<BlockCipherEngines>()))
                .Returns(_engine.Object);
            _subject = new TestCaseValidatorFactory(_engineFactory.Object, null);
        }

        [Test]
        [TestCase("encrypt", "singleblock", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "SingleBlock", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "partialblock", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "PartialBlock", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "PERMUTATIOn", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "permutation", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "substitutionTABLE", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "SUBSTItutionTable", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "VariableKey", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "variablekey", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "Variabletext", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "variableTeXT", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypT", "inversepermutation", typeof(TestCaseValidatorEncrypt))]
        [TestCase("DECRYPT", "INVERSEpermutation", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypT", "COUNTER", typeof(TestCaseValidatorCounterEncrypt))]
        [TestCase("DECRYPT", "counter", typeof(TestCaseValidatorCounterDecrypt))]

        [TestCase("encrypt", "Junk", typeof(TestCaseValidatorNull))]
        [TestCase("", "", typeof(TestCaseValidatorNull))]
        public void ShouldReturnCorrectValidatorType(string direction, string testType, Type expectedType)
        {
            var testVectorSet = GetTestGroup(direction, testType);
            var result = _subject.GetValidators(testVectorSet);

            Assert.AreEqual(1, result.Count());
            Assert.IsInstanceOf(expectedType, result.First());
        }

        private TestVectorSet GetTestGroup(string direction, string testType)
        {
            var testVectorSet = new TestVectorSet
            {
                TestGroups = new List<TestGroup>
                {
                    new TestGroup
                    {
                        TestType = testType,
                        Direction = direction,
                        Tests = new List<TestCase>
                        {
                            new TestCase()
                        }
                    }
                }
            };

            return testVectorSet;
        }
    }
}
