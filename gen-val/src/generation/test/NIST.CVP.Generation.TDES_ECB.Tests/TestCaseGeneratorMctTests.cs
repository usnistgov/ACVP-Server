﻿using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.TDES_ECB.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_ECB.Tests
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

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(result.ErrorMessage.Contains(errorMessage));
        }
    }
}