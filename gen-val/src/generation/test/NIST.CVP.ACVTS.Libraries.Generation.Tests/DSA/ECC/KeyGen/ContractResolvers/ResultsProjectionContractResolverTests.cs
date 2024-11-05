using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.KeyGen.ContractResolvers
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
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeCaseProperties(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.That(newTc.ParentGroup.TestGroupId, Is.EqualTo(tc.ParentGroup.TestGroupId), nameof(newTc.ParentGroup));
            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(newTc.TestCaseId));

            if (isSample)
            {
                Assert.That(newTc.D.ToPositiveBigInteger(), Is.EqualTo(tc.D.ToPositiveBigInteger()), nameof(newTc.D));
                Assert.That(newTc.Qx.ToPositiveBigInteger(), Is.EqualTo(tc.Qx.ToPositiveBigInteger()), nameof(newTc.Qx));
                Assert.That(newTc.Qy.ToPositiveBigInteger(), Is.EqualTo(tc.Qy.ToPositiveBigInteger()), nameof(newTc.Qy));
            }
            else
            {
                Assert.That(tc.D, Is.Null, nameof(newTc.D));
                Assert.That(tc.Qx, Is.Null, nameof(newTc.Qx));
                Assert.That(tc.Qy, Is.Null, nameof(newTc.Qy));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.That(regex.Matches(json).Count == 0, Is.True);
        }
    }
}
