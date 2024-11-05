using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SpComponent;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SpComponent.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SPComponent.ContractResolvers
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
            Assert.That(newTg.Modulus, Is.EqualTo(tg.Modulus), nameof(newTg.Modulus));
            Assert.That(newTg.KeyFormat, Is.EqualTo(tg.KeyFormat), nameof(newTg.KeyFormat));
        }

        /// <summary>
        /// Encrypt test group should not contain the cipherText, results array, deferred, testPassed
        /// all other properties included
        /// </summary>
        /// <param name="keyFormat">KeyFormat being tested</param>
        [Test]
        [TestCase(PrivateKeyModes.Crt)]
        [TestCase(PrivateKeyModes.Standard)]
        public void ShouldSerializeCaseProperties(PrivateKeyModes keyFormat)
        {
            var tvs = TestDataMother.GetTestGroups(1, keyFormat);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Deferred, Is.EqualTo(tc.Deferred), nameof(newTc.Deferred));
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));
            Assert.That(newTc.N, Is.EqualTo(tc.N), nameof(newTc.N));

            if (keyFormat == PrivateKeyModes.Crt)
            {
                Assert.That(newTc.Dmp1, Is.EqualTo(tc.Dmp1), nameof(newTc.Dmp1));
                Assert.That(newTc.Dmq1, Is.EqualTo(tc.Dmq1), nameof(newTc.Dmq1));
                Assert.That(newTc.Iqmp, Is.EqualTo(tc.Iqmp), nameof(newTc.Iqmp));
                Assert.That(newTc.P, Is.EqualTo(tc.P), nameof(newTc.P));
                Assert.That(newTc.Q, Is.EqualTo(tc.Q), nameof(newTc.Q));
            }
            else if (keyFormat == PrivateKeyModes.Standard)
            {
                Assert.That(newTc.D, Is.EqualTo(tc.D), nameof(newTc.D));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
