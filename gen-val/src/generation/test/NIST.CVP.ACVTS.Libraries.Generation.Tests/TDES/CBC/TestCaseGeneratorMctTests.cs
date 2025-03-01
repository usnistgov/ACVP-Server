﻿using System;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBC.v1_0;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CBC
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMctTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorMct _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.GetTdesMctCaseAsync(It.IsAny<TdesParameters>()))
                .Returns(() => Task.FromResult(new MctResult<TdesResult>()));
            _subject = new TestCaseGeneratorMct(_oracle.Object);
        }

        [Test]
        public async Task ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened! oh noes!";
            _oracle.Setup(s => s.GetTdesMctCaseAsync(It.IsAny<TdesParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            var result = await _subject.GenerateAsync(testGroup, true);

            Assert.That(result.Success, Is.False, nameof(result.Success));
        }
    }
}
