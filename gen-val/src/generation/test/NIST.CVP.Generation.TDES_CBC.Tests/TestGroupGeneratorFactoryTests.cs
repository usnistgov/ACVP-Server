﻿using NIST.CVP.Generation.TDES_CBC.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Linq;

namespace NIST.CVP.Generation.TDES_CBC.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGeneratorFactory();
        }
        
        [Test]
        [TestCase(typeof(TestGroupGeneratorMultiblockMessage))]
        [TestCase(typeof(TestGroupGeneratorKnownAnswer))]
        [TestCase(typeof(TestGroupGeneratorMonteCarlo))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.IsTrue(result.Count(w => w.GetType() == expectedType) == 1);
        }

        [Test]
        public void ReturnedResultShouldContainThreeGenerators()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            Assert.IsTrue(result.Count() == 3);
        }
    }
}