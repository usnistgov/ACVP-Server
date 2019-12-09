using System;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.KAS_IFC.Sp800_56Br2;
using NIST.CVP.Generation.KAS_IFC.Sp800_56Br2.ContractResolvers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.IFC.Tests.ContractResolvers
{
    [TestFixture, UnitTest]
    public class PromptProjectionContractResolver
    {
        private readonly KasJsonConverterProvider _jsonConverterProvider = new KasJsonConverterProvider();
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
        [TestCase("aft", KasKdf.OneStep)]
        [TestCase("val", KasKdf.OneStep)]
        [TestCase("aft", KasKdf.TwoStep)]
        [TestCase("val", KasKdf.TwoStep)]
        public void ShouldSerializeGroupProperties(string testType, KasKdf kdfType)
        {
            var tvs = TestDataMother.GetVectorSet(testType, kdfType);
            
            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            
            Assert.AreEqual(tg.TestGroupId, newTg.TestGroupId, nameof(newTg.TestGroupId));
            Assert.AreEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreEqual(tg.KeyConfirmationDirection, newTg.KeyConfirmationDirection, nameof(newTg.KeyConfirmationDirection));
            Assert.AreEqual(tg.KeyConfirmationRole, newTg.KeyConfirmationRole, nameof(newTg.KeyConfirmationRole));
            Assert.AreEqual(tg.KeyGenerationMethod, newTg.KeyGenerationMethod, nameof(newTg.KeyGenerationMethod));
            Assert.AreEqual(tg.L, newTg.L, nameof(newTg.L));
            Assert.AreEqual(tg.Modulo, newTg.Modulo, nameof(newTg.Modulo));
            Assert.AreEqual(tg.Scheme, newTg.Scheme, nameof(newTg.Scheme));
            Assert.AreEqual(tg.IutId, newTg.IutId, nameof(newTg.IutId));
            Assert.AreEqual(tg.ServerId, newTg.ServerId, nameof(newTg.ServerId));
            Assert.AreEqual(tg.KasRole, newTg.KasRole, nameof(newTg.KasRole));
        }
        
        [Test]
        [TestCase("aft", KasKdf.OneStep)]
        [TestCase("val", KasKdf.OneStep)]
        [TestCase("aft", KasKdf.TwoStep)]
        [TestCase("val", KasKdf.TwoStep)]
        public void ShouldSerializeCaseProperties(string testType, KasKdf kdfType)
        {
            var tvs = TestDataMother.GetVectorSet(testType, kdfType);
            
            var tg = tvs.TestGroups[0];
            var tc = tg.Tests[0];
            
            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];
            var newTc = newTg.Tests[0];

            Assert.AreEqual(tc.TestCaseId, newTc.TestCaseId, nameof(tc.TestCaseId));
            Assert.AreEqual(tc.IutE, newTc.IutE, nameof(tc.IutE));
            Assert.AreEqual(tc.IutN, newTc.IutN, nameof(tc.IutN));
            Assert.AreEqual(tc.ServerE, newTc.ServerE, nameof(tc.ServerE));
            Assert.AreEqual(tc.ServerN, newTc.ServerN, nameof(tc.ServerN));
            Assert.AreEqual(tc.ServerC, newTc.ServerC, nameof(tc.ServerC));
            Assert.AreEqual(tc.ServerNonce, newTc.ServerNonce, nameof(tc.ServerNonce));
            Assert.AreEqual(tc.IutD, newTc.IutD, nameof(tc.IutD));
            Assert.AreEqual(tc.IutDmp1, newTc.IutDmp1, nameof(tc.IutDmp1));
            Assert.AreEqual(tc.IutDmq1, newTc.IutDmq1, nameof(tc.IutDmq1));
            Assert.AreEqual(tc.IutIqmp, newTc.IutIqmp, nameof(tc.IutIqmp));
            Assert.AreEqual(tc.IutP, newTc.IutP, nameof(tc.IutP));
            Assert.AreEqual(tc.IutQ, newTc.IutQ, nameof(tc.IutQ));
            
            if (testType.Equals("AFT", StringComparison.OrdinalIgnoreCase))
            {
                Assert.AreNotEqual(tc.ServerDmp1, newTc.ServerDmp1, nameof(tc.ServerDmp1));
                Assert.AreNotEqual(tc.ServerDmq1, newTc.ServerDmq1, nameof(tc.ServerDmq1));
                Assert.AreNotEqual(tc.ServerIqmp, newTc.ServerIqmp, nameof(tc.ServerIqmp));
                Assert.AreNotEqual(tc.Tag, newTc.Tag, nameof(tc.Tag));
                Assert.AreNotEqual(tc.Dkm, newTc.Dkm, nameof(tc.Dkm));
            }

            if (testType.Equals("VAL", StringComparison.OrdinalIgnoreCase))
            {
                Assert.AreEqual(tc.ServerD, newTc.ServerD, nameof(tc.ServerD));
                Assert.AreEqual(tc.ServerDmp1, newTc.ServerDmp1, nameof(tc.ServerDmp1));
                Assert.AreEqual(tc.ServerDmq1, newTc.ServerDmq1, nameof(tc.ServerDmq1));
                Assert.AreEqual(tc.ServerIqmp, newTc.ServerIqmp, nameof(tc.ServerIqmp));
                Assert.AreEqual(tc.ServerP, newTc.ServerP, nameof(tc.ServerP));
                Assert.AreEqual(tc.ServerQ, newTc.ServerQ, nameof(tc.ServerQ));
                Assert.AreEqual(tc.Tag, newTc.Tag, nameof(tc.Tag));
                Assert.AreEqual(tc.Dkm, newTc.Dkm, nameof(tc.Dkm));
            }
        }
    }
}