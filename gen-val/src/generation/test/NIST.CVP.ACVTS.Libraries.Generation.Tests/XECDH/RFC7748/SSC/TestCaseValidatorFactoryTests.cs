using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.SSC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XECDH.RFC7748.SSC
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory _subject;
        private Mock<IOracle> _oracle;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _oracle
                .Setup(s => s.CompleteDeferredXecdhSscTestAsync(It.IsAny<XecdhSscDeferredParameters>()))
                .Returns(() => Task.FromResult(new XecdhSscDeferredResult
                {
                    Z = new BitString("01")
                }));

            _subject = new TestCaseValidatorFactory(_oracle.Object);
        }

        [Test]
        public void ShouldReturnValidator()
        {
            var vectorSet = GetVectorSet();
            var result = _subject.GetValidators(vectorSet).ToList();

            Assert.That(result.Count() == 1, "count");
            Assert.That(result[0], Is.InstanceOf(typeof(TestCaseValidator)));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(1, 2)]
        [TestCase(5, 4)]
        public void ShouldReturnOneValidatorForEachTest(int numberOfGroups, int numberOfTestsPerGroup)
        {
            int expectedValidators = numberOfGroups * numberOfTestsPerGroup;

            var vectorSet = GetVectorSet(numberOfGroups, numberOfTestsPerGroup);
            var result = _subject.GetValidators(vectorSet).ToList();

            Assert.That(result.Count(), Is.EqualTo(expectedValidators));
        }

        public TestVectorSet GetVectorSet(int numberOfGroups = 1, int numberOfCasesPerGroup = 1)
        {
            if (numberOfGroups == 0 || numberOfCasesPerGroup == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var vectorSet = new TestVectorSet();
            for (int i = 0; i < numberOfGroups; i++)
            {
                var testGroup = new TestGroup();
                vectorSet.TestGroups.Add(testGroup);

                for (int j = 0; j < numberOfCasesPerGroup; j++)
                {
                    var testCase = new TestCase();
                    testGroup.Tests.Add(testCase);
                }
            }

            return vectorSet;
        }
    }
}
