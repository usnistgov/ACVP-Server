using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SigVer.ContractResolvers
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

        /// <summary>
        /// All group level properties are present in the prompt file
        /// </summary>
        [Test]
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.Modulo, Is.EqualTo(tg.Modulo), nameof(newTg.Modulo));
            Assert.That(newTg.Mode, Is.EqualTo(tg.Mode), nameof(newTg.Mode));
            Assert.That(newTg.SaltLen, Is.EqualTo(tg.SaltLen), nameof(newTg.SaltLen));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.N, Is.EqualTo(tg.N), nameof(newTg.N));
            Assert.That(newTg.E, Is.EqualTo(tg.E), nameof(newTg.E));

            Assert.That(newTg.IsMessageRandomized, Is.False, nameof(newTg.IsMessageRandomized));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        [Test]
        public void ShouldSerializeCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1);
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
            Assert.That(newTc.Deferred, Is.EqualTo(tc.Deferred), nameof(newTc.Deferred));
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
            Assert.That(newTc.Signature, Is.EqualTo(tc.Signature), nameof(newTc.Signature));

            Assert.That(newTc.RandomValue, Is.Null, nameof(newTc.RandomValue));
            Assert.That(newTc.RandomValueLen == 0, Is.True, nameof(newTc.RandomValueLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }

        /// <summary>
        /// All group level properties are present in the prompt file
        /// </summary>
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
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.Modulo, Is.EqualTo(tg.Modulo), nameof(newTg.Modulo));
            Assert.That(newTg.Mode, Is.EqualTo(tg.Mode), nameof(newTg.Mode));
            Assert.That(newTg.SaltLen, Is.EqualTo(tg.SaltLen), nameof(newTg.SaltLen));
            Assert.That(newTg.HashAlgName, Is.EqualTo(tg.HashAlgName), nameof(newTg.HashAlgName));
            Assert.That(newTg.N, Is.EqualTo(tg.N), nameof(newTg.N));
            Assert.That(newTg.E, Is.EqualTo(tg.E), nameof(newTg.E));

            Assert.That(newTg.IsMessageRandomized, Is.True, nameof(newTg.IsMessageRandomized));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        [Test]
        public void ShouldSerializeCasePropertiesSp800_106()
        {
            var tvs = TestDataMother.GetTestGroups(1);
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
            Assert.That(newTc.Deferred, Is.EqualTo(tc.Deferred), nameof(newTc.Deferred));
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
            Assert.That(newTc.Signature, Is.EqualTo(tc.Signature), nameof(newTc.Signature));

            Assert.That(newTc.RandomValue, Is.Not.Null, nameof(newTc.RandomValue));
            Assert.That(newTc.RandomValueLen != 0, Is.True, nameof(newTc.RandomValueLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
