using System.Text.RegularExpressions;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.DSA.FFC.PQGGen.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests.ContractResolvers
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
        [TestCase(GeneratorGenMode.None, PrimeGenMode.Probable)]
        [TestCase(GeneratorGenMode.None, PrimeGenMode.Provable)]
        [TestCase(GeneratorGenMode.Canonical, PrimeGenMode.None)]
        [TestCase(GeneratorGenMode.Unverifiable, PrimeGenMode.None)]
        public void ShouldSerializeGroupProperties(GeneratorGenMode gGenMode, PrimeGenMode pqGenMode)
        {
            var tvs = TestDataMother.GetTestGroups(1, gGenMode, pqGenMode);
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
        }

        [Test]
        [TestCase(GeneratorGenMode.None, PrimeGenMode.Probable)]
        [TestCase(GeneratorGenMode.None, PrimeGenMode.Provable)]
        [TestCase(GeneratorGenMode.Canonical, PrimeGenMode.None)]
        [TestCase(GeneratorGenMode.Unverifiable, PrimeGenMode.None)]
        public void ShouldSerializeCaseProperties(GeneratorGenMode gGenMode, PrimeGenMode pqGenMode)
        {
            var tvs = TestDataMother.GetTestGroups(1, gGenMode, pqGenMode);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            if (pqGenMode != PrimeGenMode.None)
            {
                Assert.AreEqual(tc.P, newTc.P, nameof(newTc.P));
                Assert.AreEqual(tc.Q, newTc.Q, nameof(newTc.Q));
            }
            else
            {
                Assert.AreNotEqual(tc.P, newTc.P, nameof(newTc.P));
                Assert.AreNotEqual(tc.Q, newTc.Q, nameof(newTc.Q));
            }

            if (pqGenMode == PrimeGenMode.Provable)
            {
                Assert.AreEqual(tc.PSeed, newTc.PSeed, nameof(newTc.PSeed));
                Assert.AreEqual(tc.QSeed, newTc.QSeed, nameof(newTc.QSeed));
                Assert.AreEqual(tc.PCount, newTc.PCount, nameof(newTc.PCount));
                Assert.AreEqual(tc.QCount, newTc.QCount, nameof(newTc.QCount));
            }
            else
            {
                Assert.AreNotEqual(tc.PSeed, newTc.PSeed, nameof(newTc.PSeed));
                Assert.AreNotEqual(tc.QSeed, newTc.QSeed, nameof(newTc.QSeed));
                Assert.AreNotEqual(tc.PCount, newTc.PCount, nameof(newTc.PCount));
                Assert.AreNotEqual(tc.QCount, newTc.QCount, nameof(newTc.QCount));
            }

            if (pqGenMode == PrimeGenMode.Probable)
            {
                Assert.AreEqual(tc.Count, newTc.Count, nameof(newTc.Count));
            }
            else
            {
                Assert.AreNotEqual(tc.Count, newTc.Count, nameof(newTc.Count));
            }

            if (gGenMode != GeneratorGenMode.None)
            {
                Assert.AreEqual(tc.G, newTc.G, nameof(newTc.G));
            }
            else
            {
                Assert.AreNotEqual(tc.G, newTc.G, nameof(newTc.G));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
