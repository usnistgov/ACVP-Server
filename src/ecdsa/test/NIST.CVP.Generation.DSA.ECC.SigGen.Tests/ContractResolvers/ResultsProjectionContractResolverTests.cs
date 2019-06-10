using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen.ContractResolvers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));

            if (isSample)
            {
                Assert.AreEqual(tg.Qx, newTg.Qx, nameof(newTg.Qx));
                Assert.AreEqual(tg.Qy, newTg.Qy, nameof(newTg.Qy));
            }
            else
            {
                Assert.AreNotEqual(tg.D, newTg.D, nameof(newTg.D));
                Assert.AreNotEqual(tg.Qx, newTg.Qx, nameof(newTg.Qx));
                Assert.AreNotEqual(tg.Qy, newTg.Qy, nameof(newTg.Qy));
            }

            Assert.IsFalse(newTg.IsMessageRandomized, nameof(newTg.IsMessageRandomized));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeCaseProperties(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];
            tc.RandomValue = new BitString(128);
            tc.RandomValueLen = tc.RandomValue.BitLength;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            if (isSample)
            {
                Assert.AreEqual(tc.R, newTc.R, nameof(newTc.R));
                Assert.AreEqual(tc.S, newTc.S, nameof(newTc.S));
            }
            else
            {
                Assert.AreNotEqual(tc.R, newTc.R, nameof(newTc.R));
                Assert.AreNotEqual(tc.S, newTc.S, nameof(newTc.S));
            }

            Assert.IsNull(newTc.RandomValue, nameof(newTc.RandomValue));
            Assert.IsTrue(newTc.RandomValueLen == 0, nameof(newTc.RandomValueLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeGroupPropertiesSp800_106(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];
            tg.Conformance = "SP800-106";

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));

            if (isSample)
            {
                Assert.AreEqual(tg.Qx, newTg.Qx, nameof(newTg.Qx));
                Assert.AreEqual(tg.Qy, newTg.Qy, nameof(newTg.Qy));
            }
            else
            {
                Assert.AreNotEqual(tg.D, newTg.D, nameof(newTg.D));
                Assert.AreNotEqual(tg.Qx, newTg.Qx, nameof(newTg.Qx));
                Assert.AreNotEqual(tg.Qy, newTg.Qy, nameof(newTg.Qy));
            }

            Assert.IsTrue(newTg.IsMessageRandomized, nameof(newTg.IsMessageRandomized));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSerializeCasePropertiesSp800_106(bool isSample)
        {
            var tvs = TestDataMother.GetTestGroups(1, isSample);
            var tg = tvs.TestGroups[0];
            tg.Conformance = "SP800-106";
            var tc = tg.Tests[0];
            tc.RandomValue = new BitString(128);
            tc.RandomValueLen = tc.RandomValue.BitLength;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            if (isSample)
            {
                Assert.AreEqual(tc.R, newTc.R, nameof(newTc.R));
                Assert.AreEqual(tc.S, newTc.S, nameof(newTc.S));
            }
            else
            {
                Assert.AreNotEqual(tc.R, newTc.R, nameof(newTc.R));
                Assert.AreNotEqual(tc.S, newTc.S, nameof(newTc.S));
            }

            Assert.IsNotNull(newTc.RandomValue, nameof(newTc.RandomValue));
            Assert.IsTrue(newTc.RandomValueLen != 0, nameof(newTc.RandomValueLen));

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
