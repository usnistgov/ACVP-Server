using System.Text.RegularExpressions;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.EDDSA.v1_0.SigGen;
using NIST.CVP.Generation.EDDSA.v1_0.SigGen.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.SigGen.Tests.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.Curve, newTg.Curve, nameof(newTg.Curve));
            Assert.AreEqual(tg.PreHash, newTg.PreHash, nameof(newTg.PreHash));

            Assert.AreNotEqual(tg.D, newTg.D, nameof(newTg.D));
            Assert.AreNotEqual(tg.Q, newTg.Q, nameof(newTg.Q));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.Message, newTc.Message, nameof(newTc.Message));

            if (curve == 25519 && !preHash)
            {
                Assert.AreNotEqual(tc.Context, newTc.Context, nameof(newTc.Context));
            }
            else
            {
                Assert.AreEqual(tc.Context, newTc.Context, nameof(newTc.Context));
            }

            Assert.AreNotEqual(tc.Sig, newTc.Sig, nameof(newTc.Sig));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
