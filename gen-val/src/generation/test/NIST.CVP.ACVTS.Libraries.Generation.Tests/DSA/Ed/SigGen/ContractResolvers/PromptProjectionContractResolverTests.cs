using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.SigGen.ContractResolvers
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
            Assert.That(newTg.PreHash, Is.EqualTo(tg.PreHash), nameof(newTg.PreHash));

            Assert.That(newTg.D, Is.Not.EqualTo(tg.D), nameof(newTg.D));
            Assert.That(newTg.Q, Is.Not.EqualTo(tg.Q), nameof(newTg.Q));
        }

        [Test]
        [TestCase(25519, true)]
        [TestCase(25519, false)]
        [TestCase(448, true)]
        [TestCase(448, false)]
        public void ShouldSerializeCaseProperties(int curve, bool preHash)
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            if (curve == 448)
            {
                tg.Curve = Crypto.Common.Asymmetric.DSA.Ed.Enums.Curve.Ed448;
            }
            else
            {
                tg.Curve = Crypto.Common.Asymmetric.DSA.Ed.Enums.Curve.Ed25519;
            }

            tg.PreHash = preHash;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));
            Assert.That(newTc.Message, Is.EqualTo(tc.Message), nameof(newTc.Message));

            if (curve == 25519 && !preHash)
            {
                Assert.That(newTc.Context, Is.Not.EqualTo(tc.Context), nameof(newTc.Context));
            }
            else
            {
                Assert.That(newTc.Context, Is.EqualTo(tc.Context), nameof(newTc.Context));
            }

            Assert.That(newTc.Sig, Is.Not.EqualTo(tc.Sig), nameof(newTc.Sig));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
