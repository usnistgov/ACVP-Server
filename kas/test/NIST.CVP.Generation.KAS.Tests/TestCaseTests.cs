using System.Linq;
using System.Numerics;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseTests
    {
        private readonly TestDataMother _tdm = new TestDataMother();
        private TestCase _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCase();
        }

        [Test]
        public void ShouldReconstituteTestCaseFromProperJObject()
        {
            var sourceTest = new JObject();
            sourceTest.Add("tcId", new JValue(1));
            _subject = new TestCase(sourceTest);
            Assert.IsNotNull(_subject);
        }

        [Test]
        public void ShouldNotReconstituteTestCaseFromJObjectWithouttcId_ThrowsException()
        {
            var sourceTest = new JObject();
            Assert.That(() => new TestCase(sourceTest), Throws.InstanceOf<RuntimeBinderException>());
        }

        [Test]
        public void ShouldSetProperTestCaseId()
        {
            const int testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.tcId = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.TestCaseId);
        }

        [Test]
        public void ShouldSetProperFailureTest()
        {
            const bool testValue = true;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.failureTest = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.FailureTest);
        }

        [Test]
        public void ShouldSetProperDeferred()
        {
            const bool testValue = true;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.deferred = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.Deferred);
        }

        [Test]
        public void ShouldSetProperPrivateStaticServer()
        {
            BigInteger testValue= 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.xStaticServer = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPrivateKeyServer);
        }

        [Test]
        public void ShouldSetProperPublicStaticServer()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.yStaticServer = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPublicKeyServer);
        }

        [Test]
        public void ShouldSetProperPrivateEphemeralServer()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.xEphemeralServer = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPrivateKeyServer);
        }

        [Test]
        public void ShouldSetProperPublicEphemeralServer()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.yEphemeralServer = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPublicKeyServer);
        }

        [Test]
        public void ShouldSetProperPrivateStaticIut()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.xStaticIut = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPrivateKeyIut);
        }

        [Test]
        public void ShouldSetProperPublicStaticIut()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.yStaticIut = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPublicKeyIut);
        }

        [Test]
        public void ShouldSetProperPrivateEphemeralIut()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.xEphemeralIut = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPrivateKeyIut);
        }

        [Test]
        public void ShouldSetProperPublicEphemeralIut()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.yEphemeralIut = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPublicKeyIut);
        }

        [Test]
        public void ShouldSetProperNonceNoKc()
        {
            BitString testValue = new BitString("42");

            var sourceTest = GetSourceAnswerTest();
            sourceTest.nonceNoKc = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.NonceNoKc);
        }

        private dynamic GetSourceAnswerTest()
        {
            var sourceVector = new TestVectorSet() { TestGroups = _tdm.GetTestGroups().Select(g => (ITestGroup)g).ToList() };
            var sourceTest = sourceVector.AnswerProjection[0].tests[0];
            return sourceTest;
        }
    }
}