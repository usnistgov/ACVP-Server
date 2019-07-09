using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.Generation.RSA.v1_0.KeyGen.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));          
        }

        /// <summary>
        /// Encrypt test group should contain the cipherText, results array (when mct)
        /// all other properties excluded
        /// </summary>
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.E, newTc.E, nameof(newTc.E));
            Assert.AreEqual(tc.N.ToPositiveBigInteger(), newTc.N.ToPositiveBigInteger(), nameof(newTc.N));
            Assert.AreEqual(tc.P, newTc.P, nameof(newTc.P));
            Assert.AreEqual(tc.Q, newTc.Q, nameof(newTc.Q));
            Assert.AreEqual(tc.Iqmp, newTc.Iqmp, nameof(newTc.Iqmp));
            Assert.AreEqual(tc.Dmq1, newTc.Dmq1, nameof(newTc.Dmq1));
            Assert.AreEqual(tc.Dmp1, newTc.Dmp1, nameof(newTc.Dmp1));
        }
    }
}
