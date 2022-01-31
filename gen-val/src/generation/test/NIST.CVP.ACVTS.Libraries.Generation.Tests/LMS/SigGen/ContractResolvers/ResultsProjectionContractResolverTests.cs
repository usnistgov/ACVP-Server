using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.SigGen.ContractResolvers
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
            tg.TestType = "AFT";

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
        }

        [Test]
        [TestCase("AFT")]
        [TestCase("MCT")]
        public void ShouldSerializeCaseProperties(string testType)
        {
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];
            tg.TestType = testType;

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            if (testType == "AFT")
            {
                Assert.AreEqual(tc.Signature, newTc.Signature, nameof(newTc.Signature));
                Assert.AreEqual(null, newTc.ResultsArray, nameof(newTc.ResultsArray));
                Assert.AreNotEqual(tc.Message, newTc.Message, nameof(newTc.Message));
                Assert.AreNotEqual(tc.Seed, newTc.Seed, nameof(newTc.Seed));
                Assert.AreNotEqual(tc.RootI, newTc.RootI, nameof(newTc.RootI));
            }
            else
            {
                for (var i = 0; i < tc.ResultsArray.Count; i++)
                {
                    Assert.AreNotEqual(tc.ResultsArray[i].Message, newTc.ResultsArray[i].Message, "mctMessage");
                    Assert.AreEqual(tc.ResultsArray[i].Signature, newTc.ResultsArray[i].Signature, "mctSignature");
                }
                Assert.AreNotEqual(tc.Signature, newTc.Signature, nameof(newTc.Signature));
                Assert.AreNotEqual(tc.Message, newTc.Message, nameof(newTc.Message));
                Assert.AreNotEqual(tc.Seed, newTc.Seed, nameof(newTc.Seed));
                Assert.AreNotEqual(tc.RootI, newTc.RootI, nameof(newTc.RootI));
            }

            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
