using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.EDDSA.v1_0.SigVer;
using NIST.CVP.Generation.EDDSA.v1_0.SigVer.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.SigVer.Tests.ContractResolvers
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
        public void ShouldSerializeGroupProperties()
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeCaseProperties(bool testPassed)
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];
            tc.TestPassed = testPassed;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            Assert.AreEqual(tc.TestPassed, newTc.TestPassed, nameof(newTc.TestPassed));
        }
    }
}
