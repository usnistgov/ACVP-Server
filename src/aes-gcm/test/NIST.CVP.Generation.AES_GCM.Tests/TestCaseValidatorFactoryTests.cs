﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private Mock<IAeadModeBlockCipherFactory> _cipherFactory;
        private Mock<IBlockCipherEngineFactory> _engineFactory;
        private TestCaseValidatorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _cipherFactory = new Mock<IAeadModeBlockCipherFactory>();
            _engineFactory = new Mock<IBlockCipherEngineFactory>();
            _subject = new TestCaseValidatorFactory(_cipherFactory.Object, _engineFactory.Object);
        }

        [Test]
        [TestCase("encrypt", false, typeof(TestCaseValidatorEncrypt))]
        [TestCase("encrypt", true, typeof(TestCaseValidatorDeferredEncrypt))]
        [TestCase("decrypt", false, typeof(TestCaseValidatorDecrypt))]
        [TestCase("decrypt", true, typeof(TestCaseValidatorDecrypt))]
        public void ShouldReturnCorrectValidatorTypeDependantOnFunction(string function, bool isDeferred, Type expectedType)
        {
            TestVectorSet testVectorSet = null;
            List<TestCase> suppliedResults = null;

            GetData(ref testVectorSet, ref suppliedResults, function, isDeferred);

            var results = _subject.GetValidators(testVectorSet);

            Assert.IsTrue(results.Count() == 1, "Expected 1 validator");
            Assert.IsInstanceOf(expectedType, results.First());
        }

        private void GetData(ref TestVectorSet testVectorSet, ref List<TestCase> suppliedResults, string function, bool isDeferred)
        {
            testVectorSet = new TestVectorSet()
            {
                Algorithm = string.Empty,
                TestGroups = new List<TestGroup>()
                {
                    new TestGroup()
                    {
                        Function = function,
                        AADLength = 128,
                        TestType = string.Empty,
                        IVGeneration = string.Empty,
                        IVGenerationMode = string.Empty,
                        IVLength = 128,
                        KeyLength = 128,
                        PTLength = 128,
                        TagLength = 128,
                        Tests = new List<TestCase>()
                        {
                            new TestCase()
                            {
                                AAD = new BitString(128),
                                CipherText = new BitString(128),
                                Deferred = isDeferred,
                                TestPassed = true,
                                Key = new BitString(128),
                                PlainText = new BitString(128),
                                Tag = new BitString(128),
                                TestCaseId = 1
                            }
                        }
                    }
                }
            };

            suppliedResults = new List<TestCase>()
            {
                new TestCase()
                {
                    IV = new BitString(128),
                    TestCaseId = 1
                }
            };
        }
    }
}