using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigGen.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.SigGen.ContractResolvers
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class ResultsProjectionContractResolverTests
    {
        private readonly JsonConverterProvider _jsonConverterProvider = new JsonConverterProvider();
        private readonly ContractResolverFactory _contractResolverFactory = new ContractResolverFactory();
        private readonly Projection _projection = Projection.Result;

        private VectorSetSerializer<TestVectorSet, TestGroup, TestCase> _serializer;
        private VectorSetDeserializer<TestVectorSet, TestGroup, TestCase> _deserializer;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _serializer =
                new VectorSetSerializer<TestVectorSet, TestGroup, TestCase>(
                    _jsonConverterProvider,
                    _contractResolverFactory
                );
            _deserializer =
                new VectorSetDeserializer<TestVectorSet, TestGroup, TestCase>(
                    _jsonConverterProvider
                );
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeGroupProperties(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));

            if (isSample)
            {
                Assert.That(newTg.Qx, Is.EqualTo(tg.Qx), nameof(newTg.Qx));
                Assert.That(newTg.Qy, Is.EqualTo(tg.Qy), nameof(newTg.Qy));
            }
            else
            {
                Assert.That(tg.D, Is.Null, nameof(newTg.D));
                Assert.That(tg.Qx, Is.Null, nameof(newTg.Qx));
                Assert.That(tg.Qy, Is.Null, nameof(newTg.Qy));
            }

            Assert.That(newTg.IsMessageRandomized, Is.False, nameof(newTg.IsMessageRandomized));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeCaseProperties(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];
            tc.RandomValue = new BitString(128);
            tc.RandomValueLen = tc.RandomValue.BitLength;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            if (isSample)
            {
                Assert.That(newTc.R, Is.EqualTo(tc.R), nameof(newTc.R));
                Assert.That(newTc.S, Is.EqualTo(tc.S), nameof(newTc.S));
            }
            else
            {
                Assert.That(tc.R, Is.Null, nameof(newTc.R));
                Assert.That(tc.S, Is.Null, nameof(newTc.S));
            }

            Assert.That(newTc.RandomValue, Is.Null, nameof(newTc.RandomValue));
            Assert.That(newTc.RandomValueLen == 0, Is.True, nameof(newTc.RandomValueLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeGroupPropertiesSp800_106(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];
            tg.Conformance = "SP800-106";

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));

            if (isSample)
            {
                Assert.That(newTg.Qx, Is.EqualTo(tg.Qx), nameof(newTg.Qx));
                Assert.That(newTg.Qy, Is.EqualTo(tg.Qy), nameof(newTg.Qy));
            }
            else
            {
                Assert.That(tg.D, Is.Null, nameof(newTg.D));
                Assert.That(tg.Qx, Is.Null, nameof(newTg.Qx));
                Assert.That(tg.Qy, Is.Null, nameof(newTg.Qy));
            }

            Assert.That(newTg.IsMessageRandomized, Is.True, nameof(newTg.IsMessageRandomized));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeCasePropertiesSp800_106(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];
            tg.Conformance = "SP800-106";
            var tc = tg.Tests[0];
            tc.RandomValue = new BitString(128);
            tc.RandomValueLen = tc.RandomValue.BitLength;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            if (isSample)
            {
                Assert.That(newTc.R, Is.EqualTo(tc.R), nameof(newTc.R));
                Assert.That(newTc.S, Is.EqualTo(tc.S), nameof(newTc.S));
            }
            else
            {
                Assert.That(tc.R, Is.Null, nameof(newTc.R));
                Assert.That(tc.S, Is.Null, nameof(newTc.S));
            }

            Assert.That(newTc.RandomValue, Is.Not.Null, nameof(newTc.RandomValue));
            Assert.That(newTc.RandomValueLen != 0, Is.True, nameof(newTc.RandomValueLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
