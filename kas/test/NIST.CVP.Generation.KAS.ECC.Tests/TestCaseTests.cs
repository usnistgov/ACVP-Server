using System.Linq;
using System.Numerics;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
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
            sourceTest.staticPrivateServer = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPrivateKeyServer);
        }

        [Test]
        public void ShouldSetProperPublicStaticServerX()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.staticPublicServerX = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPublicKeyServerX);
        }

        [Test]
        public void ShouldSetProperPublicStaticServerY()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.staticPublicServerY = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPublicKeyServerY);
        }

        [Test]
        public void ShouldSetProperPrivateEphemeralServer()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.ephemeralPrivateServer = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPrivateKeyServer);
        }

        [Test]
        public void ShouldSetProperPublicEphemeralServerX()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.ephemeralPublicServerX = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPublicKeyServerX);
        }

        [Test]
        public void ShouldSetProperPublicEphemeralServerY()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.ephemeralPublicServerY = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPublicKeyServerY);
        }

        [Test]
        public void ShouldSetProperPrivateStaticIut()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.staticPrivateIut = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPrivateKeyIut);
        }

        [Test]
        public void ShouldSetProperPublicStaticIutX()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.staticPublicIutX = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPublicKeyIutX);
        }

        [Test]
        public void ShouldSetProperPublicStaticIutY()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.staticPublicIutY = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.StaticPublicKeyIutY);
        }

        [Test]
        public void ShouldSetProperPrivateEphemeralIut()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.ephemeralPrivateIut = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPrivateKeyIut);
        }

        [Test]
        public void ShouldSetProperPublicEphemeralIutX()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.ephemeralPublicIutX = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPublicKeyIutX);
        }

        [Test]
        public void ShouldSetProperPublicEphemeralIutY()
        {
            BigInteger testValue = 42;

            var sourceTest = GetSourceAnswerTest();
            sourceTest.ephemeralPublicIutY = testValue;
            var subject = new TestCase(sourceTest);
            Assume.That(_subject != null);
            Assert.AreEqual(testValue, subject.EphemeralPublicKeyIutY);
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