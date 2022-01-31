using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.EccComponent
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorFactoryTests
    {
        private TestCaseValidatorFactory<TestVectorSet> _subject;
        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse>> _deferredTestCaseResolver;

        [SetUp]
        public void Setup()
        {
            _deferredTestCaseResolver =
                new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse>>();
            _deferredTestCaseResolver
                .Setup(s => s.CompleteDeferredCryptoAsync(
                    It.IsAny<TestGroup>(),
                    It.IsAny<TestCase>(),
                    It.IsAny<TestCase>()
                ))
                .Returns(() => Task.FromResult(new SharedSecretResponse(new BitString(1))));

            _subject = new TestCaseValidatorFactory<TestVectorSet>(_deferredTestCaseResolver.Object);
        }

        [Test]
        public void ShouldReturnValidator()
        {
            var vectorSet = GetVectorSet();
            var result = _subject.GetValidators(vectorSet).ToList();

            Assert.That(result.Count() == 1, "count");
            Assert.IsInstanceOf(typeof(TestCaseValidator), result[0]);
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

            Assert.AreEqual(expectedValidators, result.Count());
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
