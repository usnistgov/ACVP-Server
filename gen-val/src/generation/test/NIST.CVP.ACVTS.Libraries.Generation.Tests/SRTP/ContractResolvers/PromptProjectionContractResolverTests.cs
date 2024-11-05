using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SRTP.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups(1);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.AesKeyLength, Is.EqualTo(tg.AesKeyLength), nameof(newTg.AesKeyLength));
            Assert.That(newTg.Kdr, Is.EqualTo(tg.Kdr), nameof(newTg.Kdr));
        }

        [Test]
        public void ShouldSerializeCaseProperties()
        {
            var tvs = TestDataMother.GetTestGroups(1);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.MasterKey, Is.EqualTo(tc.MasterKey), nameof(newTc.MasterKey));
            Assert.That(newTc.MasterSalt, Is.EqualTo(tc.MasterSalt), nameof(newTc.MasterSalt));
            Assert.That(newTc.Index, Is.EqualTo(tc.Index), nameof(newTc.Index));
            Assert.That(newTc.SrtcpIndex, Is.EqualTo(tc.SrtcpIndex), nameof(newTc.SrtcpIndex));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            Assert.That(newTc.SrtpKe, Is.Not.EqualTo(tc.SrtpKe), nameof(newTc.SrtpKe));
            Assert.That(newTc.SrtpKa, Is.Not.EqualTo(tc.SrtpKa), nameof(newTc.SrtpKa));
            Assert.That(newTc.SrtpKs, Is.Not.EqualTo(tc.SrtpKs), nameof(newTc.SrtpKs));
            Assert.That(newTc.SrtcpKe, Is.Not.EqualTo(tc.SrtcpKe), nameof(newTc.SrtcpKe));
            Assert.That(newTc.SrtcpKa, Is.Not.EqualTo(tc.SrtcpKa), nameof(newTc.SrtcpKa));
            Assert.That(newTc.SrtcpKs, Is.Not.EqualTo(tc.SrtcpKs), nameof(newTc.SrtcpKs));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
