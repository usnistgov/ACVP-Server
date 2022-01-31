using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.SigGen
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorMctTests
    {
        [Test]
        [TestCase("beefface", "beefface")]
        [TestCase("facebeef", "facebeef")]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodTest(string expected, string supplied)
        {
            var subject = new TestCaseValidatorMctSig(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(2, 2)]
        [TestCase(4, 4)]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodTest(int expected, int supplied)
        {
            var subject = new TestCaseValidatorMctSig(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase("beefface", "facebeef")]
        [TestCase("facebeef", "beefface")]
        public async Task ShouldRunVerifyMethodAndFailWithBadTest(string expected, string supplied)
        {
            var subject = new TestCaseValidatorMctSig(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        [Test]
        [TestCase(2, 1)]
        [TestCase(1, 2)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        public async Task ShouldRunVerifyMethodAndFailWithBadTest(int expected, int supplied)
        {
            var subject = new TestCaseValidatorMctSig(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithNullTest()
        {
            var subject = new TestCaseValidatorMctSig(GetNullTestCase());
            var result = await subject.ValidateAsync(GetNullTestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        private TestCase GetResultTestCase(string signature)
        {
            return new TestCase
            {
                TestCaseId = 1,
                ResultsArray = new List<AlgoArrayResponse>(new AlgoArrayResponse[]
                {
                    new AlgoArrayResponse()
                    {
                        Signature = new BitString(signature)
                    }
                })
            };
        }

        private TestCase GetResultTestCase(int length)
        {
            var array = new AlgoArrayResponse[length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new AlgoArrayResponse() { Signature = new BitString("beefface") };
            }
            return new TestCase
            {
                TestCaseId = 1,
                ResultsArray = new List<AlgoArrayResponse>(array)
            };
        }

        private TestCase GetNullTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                ResultsArray = null
            };
        }
    }
}
