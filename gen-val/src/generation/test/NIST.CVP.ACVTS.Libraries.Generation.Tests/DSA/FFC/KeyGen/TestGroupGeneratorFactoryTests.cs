﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.KeyGen
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorFactoryTests
    {
        private TestGroupGeneratorFactory _subject;

        [SetUp]
        public void SetUp()
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetDsaKeyAsync(It.IsAny<DsaKeyParameters>()))
                .Returns(Task.FromResult(new DsaKeyResult { Key = new FfcKeyPair() }));

            _subject = new TestGroupGeneratorFactory(oracleMock.Object);
        }

        [Test]
        [TestCase(typeof(TestGroupGenerator))]
        public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            Assert.That(result.Count(w => w.GetType() == expectedType) == 1, Is.True);
        }

        [Test]
        public void ReturnedResultShouldContainOneGenerator()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldReturnTestGroups()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var p = new Parameters
            {
                Algorithm = "DSA",
                Mode = "keyGen",
                IsSample = false,
                Capabilities = GetCapabilities(),
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var result = _subject.GetTestGroupGenerators(new Parameters());

            var p = new Parameters
            {
                Algorithm = "DSA",
                Mode = "keyGen",
                IsSample = false,
                Capabilities = GetCapabilities(),
            };

            var groups = new List<TestGroup>();

            foreach (var genny in result)
            {
                groups.AddRangeIfNotNullOrEmpty(await genny.BuildTestGroupsAsync(p));
            }

            Assert.That(groups.Count, Is.EqualTo(3));
        }

        private Capability[] GetCapabilities()
        {
            var caps = new Capability[3];
            caps[0] = new Capability
            {
                L = 2048,
                N = 224
            };

            caps[1] = new Capability
            {
                L = 2048,
                N = 256
            };

            caps[2] = new Capability
            {
                L = 3072,
                N = 256
            };

            return caps;
        }
    }
}
