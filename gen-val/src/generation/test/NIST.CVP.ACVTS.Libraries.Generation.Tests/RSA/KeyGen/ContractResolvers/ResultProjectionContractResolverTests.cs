using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.KeyGen.ContractResolvers
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

        /// <summary>
        /// Only the groupId and tests should be present in the result file
        /// </summary>
        [Test]
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups(PrimeGenFips186_4Modes.B32);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
        }

        /// <summary>
        /// Encrypt test group should contain the cipherText, results array (when mct)
        /// all other properties excluded
        /// </summary>
        [Test]
        public void ShouldSerializeCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(PrimeGenFips186_4Modes.B32);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.E, Is.EqualTo(tc.E), nameof(newTc.E));
            Assert.That(newTc.N.ToPositiveBigInteger(), Is.EqualTo(tc.N.ToPositiveBigInteger()), nameof(newTc.N));
            Assert.That(newTc.P, Is.EqualTo(tc.P), nameof(newTc.P));
            Assert.That(newTc.Q, Is.EqualTo(tc.Q), nameof(newTc.Q));
            Assert.That(newTc.Iqmp, Is.EqualTo(tc.Iqmp), nameof(newTc.Iqmp));
            Assert.That(newTc.Dmq1, Is.EqualTo(tc.Dmq1), nameof(newTc.Dmq1));
            Assert.That(newTc.Dmp1, Is.EqualTo(tc.Dmp1), nameof(newTc.Dmp1));
        }
    }
}
