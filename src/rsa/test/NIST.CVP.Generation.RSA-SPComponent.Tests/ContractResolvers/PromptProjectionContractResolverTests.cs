using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.RSA.v1_0.SpComponent;
using NIST.CVP.Generation.RSA.v1_0.SpComponent.ContractResolvers;

namespace NIST.CVP.Generation.RSA_SPComponent.Tests.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Modulo, newTg.Modulo, nameof(newTg.Modulo));
            Assert.AreEqual(tg.KeyFormat, newTg.KeyFormat, nameof(newTg.KeyFormat));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Deferred, newTc.Deferred, nameof(newTc.Deferred));
            Assert.AreEqual(tc.Message, newTc.Message, nameof(newTc.Message));
            Assert.AreEqual(tc.N, newTc.N, nameof(newTc.N));

            if (keyFormat == PrivateKeyModes.Crt)
            {
                Assert.AreEqual(tc.Dmp1, newTc.Dmp1, nameof(newTc.Dmp1));
                Assert.AreEqual(tc.Dmq1, newTc.Dmq1, nameof(newTc.Dmq1));
                Assert.AreEqual(tc.Iqmp, newTc.Iqmp, nameof(newTc.Iqmp));
            }
            else if (keyFormat == PrivateKeyModes.Standard)
            {
                Assert.AreEqual(tc.D, newTc.D, nameof(newTc.D));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            var regex = new Regex("testPassed", RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
