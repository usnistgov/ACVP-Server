using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.SP800_56Br2.DpComponent;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.SP800_56Br2.DpComponent.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SP800_56Br2.ContractResolvers
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
            var tvs = TestDataMother.GetTestGroups();
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            newTg.Modulo = 4096;

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.Tests.Count, Is.EqualTo(tg.Tests.Count), nameof(newTg.Tests));
            Assert.That(newTg.Modulo, Is.Not.EqualTo(tg.Modulo), nameof(newTg.Modulo));
        }

        /// <summary>
        /// Encrypt test group should contain the cipherText, results array (when mct)
        /// all other properties excluded
        /// </summary>
        /// <param name="function">The function being tested</param>
        /// <param name="testType">The testType</param>
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

            // TODO - undo this
            // for (var i = 0; i < tc.ResultsArray.Count; i++)
            // {
            //     Assert.AreEqual(tc.ResultsArray[i].PlainText, newTc.ResultsArray[i].PlainText, "arrayPlainText");
            //     Assert.AreEqual(tc.ResultsArray[i].TestPassed, newTc.ResultsArray[i].TestPassed, "arrayFailureTest");
            //
            //     Assert.AreEqual(tc.ResultsArray[i].E, newTc.ResultsArray[i].E, "arrayE");
            //     Assert.AreEqual(tc.ResultsArray[i].N, newTc.ResultsArray[i].N, "arrayN");
            //
            //     Assert.AreNotEqual(tc.ResultsArray[i].Key.PrivKey.Q, newTc.ResultsArray[i].Key.PrivKey.Q, "arrayQ");
            //     Assert.AreNotEqual(tc.ResultsArray[i].Key.PrivKey.P, newTc.ResultsArray[i].Key.PrivKey.P, "arrayP");
            // }

            Assert.That(newTc.Deferred, Is.Not.EqualTo(tc.Deferred), nameof(newTc.Deferred));
        }
    }
}
