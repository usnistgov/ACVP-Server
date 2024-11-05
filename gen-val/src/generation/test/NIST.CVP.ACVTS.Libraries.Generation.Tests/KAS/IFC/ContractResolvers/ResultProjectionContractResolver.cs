using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.IFC.ContractResolvers
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
        [TestCase("aft", Kda.OneStep)]
        [TestCase("val", Kda.OneStep)]
        [TestCase("aft", Kda.TwoStep)]
        [TestCase("val", Kda.TwoStep)]
        public void ShouldSerializeGroupProperties(string testType, Kda kdfType)
        {
            var tvs = TestDataMother.GetVectorSet(testType, kdfType);

            var tg = tvs.TestGroups[0];

            var json = _serializer.Serialize(tvs, _projection);
            var newTvs = _deserializer.Deserialize(json);

            var newTg = newTvs.TestGroups[0];

            Assert.That(newTg.TestGroupId, Is.EqualTo(tg.TestGroupId), nameof(newTg.TestGroupId));
            Assert.That(newTg.TestType, Is.Not.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.L, Is.Not.EqualTo(tg.L), nameof(newTg.L));
            Assert.That(newTg.Modulo, Is.Not.EqualTo(tg.Modulo), nameof(newTg.Modulo));
            Assert.That(newTg.Scheme, Is.Not.EqualTo(tg.Scheme), nameof(newTg.Scheme));
            Assert.That(newTg.IutId, Is.Not.EqualTo(tg.IutId), nameof(newTg.IutId));
            Assert.That(newTg.ServerId, Is.Not.EqualTo(tg.ServerId), nameof(newTg.ServerId));
        }

        [Test]
        [TestCase("aft", Kda.OneStep)]
        [TestCase("val", Kda.OneStep)]
        [TestCase("aft", Kda.TwoStep)]
        [TestCase("val", Kda.TwoStep)]
        public void ShouldSerializeCaseProperties(string testType, Kda kdfType)
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
                Assert.That(newTc.Dkm, Is.EqualTo(tc.Dkm), nameof(tc.Dkm));
                Assert.That(newTc.Tag, Is.EqualTo(tc.Tag), nameof(tc.Tag));
            }

            if (testType.Equals("VAL", StringComparison.OrdinalIgnoreCase))
            {
                Assert.That(newTc.TestPassed, Is.EqualTo(tc.TestPassed), nameof(tc.TestPassed));
            }
        }
    }
}
