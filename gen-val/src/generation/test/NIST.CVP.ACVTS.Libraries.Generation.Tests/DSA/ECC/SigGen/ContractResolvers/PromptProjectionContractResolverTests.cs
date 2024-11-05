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
    public class PromptProjectionContractResolverTests
    {
        private readonly JsonConverterProvider _jsonConverterProvider = new JsonConverterProvider();
        private readonly ContractResolverFactory _contractResolverFactory = new ContractResolverFactory();
        private readonly Projection _projection = Projection.Prompt;

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
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.Curve, Is.EqualTo(tg.Curve), nameof(newTg.Curve));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.ComponentTest, Is.EqualTo(tg.ComponentTest), nameof(newTg.ComponentTest));

            Assert.That(newTg.D, Is.Not.EqualTo(tg.D), nameof(newTg.D));
            Assert.That(newTg.Qx, Is.Not.EqualTo(tg.Qx), nameof(newTg.Qx));
            Assert.That(newTg.Qy, Is.Not.EqualTo(tg.Qy), nameof(newTg.Qy));

            Assert.That(newTg.IsMessageRandomized, Is.False, nameof(newTg.IsMessageRandomized));
        }

        [Test]
        public void ShouldSerializeCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups();
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
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));

            Assert.That(newTc.R, Is.Not.EqualTo(tc.R), nameof(newTc.R));
            Assert.That(newTc.S, Is.Not.EqualTo(tc.S), nameof(newTc.S));

            Assert.That(newTc.RandomValue, Is.Null, nameof(newTc.RandomValue));
            Assert.That(newTc.RandomValueLen == 0, Is.True, nameof(newTc.RandomValueLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        [Test]
        public void ShouldSerializeGroupPropertiesSp800_106()
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];
            tg.Conformance = "SP800-106";

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.Curve, Is.EqualTo(tg.Curve), nameof(newTg.Curve));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.ComponentTest, Is.EqualTo(tg.ComponentTest), nameof(newTg.ComponentTest));

            Assert.That(newTg.D, Is.Not.EqualTo(tg.D), nameof(newTg.D));
            Assert.That(newTg.Qx, Is.Not.EqualTo(tg.Qx), nameof(newTg.Qx));
            Assert.That(newTg.Qy, Is.Not.EqualTo(tg.Qy), nameof(newTg.Qy));

            Assert.That(newTg.IsMessageRandomized, Is.True, nameof(newTg.IsMessageRandomized));
        }

        [Test]
        public void ShouldSerializeCasePropertiesSp800_106()
        {
            var tvs = TestDataMother.GetTestGroups();
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
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));

            Assert.That(newTc.R, Is.Not.EqualTo(tc.R), nameof(newTc.R));
            Assert.That(newTc.S, Is.Not.EqualTo(tc.S), nameof(newTc.S));

            Assert.That(newTc.RandomValue, Is.Null, nameof(newTc.RandomValue));
            Assert.That(newTc.RandomValueLen == 0, Is.True, nameof(newTc.RandomValueLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
