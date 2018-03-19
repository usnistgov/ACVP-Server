using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.SRTP.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SRTP.Tests.ContractResolvers
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

            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.Tests.Count, newTg.Tests.Count, nameof(newTg.Tests));
            Assert.AreEqual(tg.AesKeyLength, newTg.AesKeyLength, nameof(newTg.AesKeyLength));
            Assert.AreEqual(tg.Kdr, newTg.Kdr, nameof(newTg.Kdr));
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

            Assert.AreEqual(tc.ParentGroup.TestGroupId, newTc.ParentGroup.TestGroupId, nameof(newTc.ParentGroup));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));
            Assert.AreEqual(tc.MasterKey, newTc.MasterKey, nameof(newTc.MasterKey));
            Assert.AreEqual(tc.MasterSalt, newTc.MasterSalt, nameof(newTc.MasterSalt));
            Assert.AreEqual(tc.Index, newTc.Index, nameof(newTc.Index));
            Assert.AreEqual(tc.SrtcpIndex, newTc.SrtcpIndex, nameof(newTc.SrtcpIndex));
            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(newTc.TestCaseId));

            Assert.AreNotEqual(tc.SrtpKe, newTc.SrtpKe, nameof(newTc.SrtpKe));
            Assert.AreNotEqual(tc.SrtpKa, newTc.SrtpKa, nameof(newTc.SrtpKa));
            Assert.AreNotEqual(tc.SrtpKs, newTc.SrtpKs, nameof(newTc.SrtpKs));
            Assert.AreNotEqual(tc.SrtcpKe, newTc.SrtcpKe, nameof(newTc.SrtcpKe));
            Assert.AreNotEqual(tc.SrtcpKa, newTc.SrtcpKa, nameof(newTc.SrtcpKa));
            Assert.AreNotEqual(tc.SrtcpKs, newTc.SrtcpKs, nameof(newTc.SrtcpKs));
            
            // TestPassed will have the default value when re-hydrated, check to make sure it isn't in the JSON
            Regex regex = new Regex(nameof(TestCase.TestPassed), RegexOptions.IgnoreCase);
            Assert.IsTrue(regex.Matches(json).Count == 0);
        }
    }
}
