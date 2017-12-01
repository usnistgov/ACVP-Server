using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCaseValidatorFactory(null);
        }

        [Test]
        [TestCase("encrypt", "singleblock", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "SingleBlock", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "partialblock", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "PartialBlock", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "GFSBOX", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "gfsbox", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "KeySBox", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "KEYSBOX", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "VarKey", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "varkey", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypt", "Vartxt", typeof(TestCaseValidatorEncrypt))]
        [TestCase("decrypt", "varTXT", typeof(TestCaseValidatorDecrypt))]

        [TestCase("encrypT", "cOUNTER", typeof(TestCaseValidatorCounterEncrypt))]
        [TestCase("DECRYPT", "coUNTER", typeof(TestCaseValidatorCounterDecrypt))]
        [TestCase("encrypt", "Junk", typeof(TestCaseValidatorNull))]
        [TestCase("", "", typeof (TestCaseValidatorNull))]
        public void ShouldReturnCorrectValidatorType(string direction, string testType, Type expectedType)
        {
            var testVectorSet = GetTestGroup(direction, testType);
            var result = _subject.GetValidators(testVectorSet, null);

            Assert.AreEqual(1, result.Count());
            Assert.IsInstanceOf(expectedType, result.First());
        }

        private TestVectorSet GetTestGroup(string direction, string testType)
        {
            var testVectorSet = new TestVectorSet
            {
                TestGroups = new List<ITestGroup>
                {
                    new TestGroup
                    {
                        TestType = testType,
                        Direction = direction,
                        Tests = new List<ITestCase>
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
