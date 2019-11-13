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
    public class ResultProjectionContractResolver
    {
        private readonly KasJsonConverterProvider _jsonConverterProvider = new KasJsonConverterProvider();
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
            Assert.AreNotEqual(tg.TestType, newTg.TestType, nameof(newTg.TestType));
            Assert.AreNotEqual(tg.L, newTg.L, nameof(newTg.L));
            Assert.AreNotEqual(tg.Modulo, newTg.Modulo, nameof(newTg.Modulo));
            Assert.AreNotEqual(tg.Scheme, newTg.Scheme, nameof(newTg.Scheme));
            Assert.AreNotEqual(tg.IutId, newTg.IutId, nameof(newTg.IutId));
            Assert.AreNotEqual(tg.ServerId, newTg.ServerId, nameof(newTg.ServerId));
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

            if (testType.Equals("AFT", StringComparison.OrdinalIgnoreCase))
            {
                Assert.AreEqual(tc.Dkm, newTc.Dkm, nameof(tc.Dkm));
                Assert.AreEqual(tc.Tag, newTc.Tag, nameof(tc.Tag));
            }

            if (testType.Equals("VAL", StringComparison.OrdinalIgnoreCase))
            {
                Assert.AreEqual(tc.TestPassed, newTc.TestPassed, nameof(tc.TestPassed));
            }
        }
    }
}