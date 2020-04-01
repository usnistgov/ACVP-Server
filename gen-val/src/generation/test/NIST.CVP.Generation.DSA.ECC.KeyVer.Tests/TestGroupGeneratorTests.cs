﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.ECDSA.v1_0.KeyVer;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithCurves(new [] { "p-224" })
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurves(new [] { "k-283", "p-224" })
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithCurves(new [] { "p-256", "b-233", "k-409", "k-571" })
                    .Build()
            },
            new object[]
            {
                15,
                new ParameterBuilder()
                    .WithCurves(ParameterValidator.VALID_CURVES)
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreate1TestGroupForEachCombinationOfCurveAndSecret(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var result = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}
