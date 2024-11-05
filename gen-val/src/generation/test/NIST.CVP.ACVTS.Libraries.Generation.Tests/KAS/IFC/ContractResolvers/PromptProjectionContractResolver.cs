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
            Assert.That(newTg.TestType, Is.EqualTo(tg.TestType), nameof(newTg.TestType));
            Assert.That(newTg.KeyConfirmationDirection, Is.EqualTo(tg.KeyConfirmationDirection), nameof(newTg.KeyConfirmationDirection));
            Assert.That(newTg.KeyConfirmationRole, Is.EqualTo(tg.KeyConfirmationRole), nameof(newTg.KeyConfirmationRole));
            Assert.That(newTg.KeyGenerationMethod, Is.EqualTo(tg.KeyGenerationMethod), nameof(newTg.KeyGenerationMethod));
            Assert.That(newTg.L, Is.EqualTo(tg.L), nameof(newTg.L));
            Assert.That(newTg.Modulo, Is.EqualTo(tg.Modulo), nameof(newTg.Modulo));
            Assert.That(newTg.Scheme, Is.EqualTo(tg.Scheme), nameof(newTg.Scheme));
            Assert.That(newTg.IutId, Is.EqualTo(tg.IutId), nameof(newTg.IutId));
            Assert.That(newTg.ServerId, Is.EqualTo(tg.ServerId), nameof(newTg.ServerId));
            Assert.That(newTg.KasRole, Is.EqualTo(tg.KasRole), nameof(newTg.KasRole));
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

            Assert.That(newTc.TestCaseId, Is.EqualTo(tc.TestCaseId), nameof(tc.TestCaseId));
            Assert.That(newTc.IutE, Is.EqualTo(tc.IutE), nameof(tc.IutE));
            Assert.That(newTc.IutN, Is.EqualTo(tc.IutN), nameof(tc.IutN));
            Assert.That(newTc.ServerE, Is.EqualTo(tc.ServerE), nameof(tc.ServerE));
            Assert.That(newTc.ServerN, Is.EqualTo(tc.ServerN), nameof(tc.ServerN));
            Assert.That(newTc.ServerC, Is.EqualTo(tc.ServerC), nameof(tc.ServerC));
            Assert.That(newTc.ServerNonce, Is.EqualTo(tc.ServerNonce), nameof(tc.ServerNonce));
            Assert.That(newTc.IutD, Is.EqualTo(tc.IutD), nameof(tc.IutD));
            Assert.That(newTc.IutDmp1, Is.EqualTo(tc.IutDmp1), nameof(tc.IutDmp1));
            Assert.That(newTc.IutDmq1, Is.EqualTo(tc.IutDmq1), nameof(tc.IutDmq1));
            Assert.That(newTc.IutIqmp, Is.EqualTo(tc.IutIqmp), nameof(tc.IutIqmp));
            Assert.That(newTc.IutP, Is.EqualTo(tc.IutP), nameof(tc.IutP));
            Assert.That(newTc.IutQ, Is.EqualTo(tc.IutQ), nameof(tc.IutQ));

            if (testType.Equals("AFT", StringComparison.OrdinalIgnoreCase))
            {
                Assert.That(newTc.ServerDmp1, Is.Not.EqualTo(tc.ServerDmp1), nameof(tc.ServerDmp1));
                Assert.That(newTc.ServerDmq1, Is.Not.EqualTo(tc.ServerDmq1), nameof(tc.ServerDmq1));
                Assert.That(newTc.ServerIqmp, Is.Not.EqualTo(tc.ServerIqmp), nameof(tc.ServerIqmp));
                Assert.That(newTc.Tag, Is.Not.EqualTo(tc.Tag), nameof(tc.Tag));
                Assert.That(newTc.Dkm, Is.Not.EqualTo(tc.Dkm), nameof(tc.Dkm));
            }

            if (testType.Equals("VAL", StringComparison.OrdinalIgnoreCase))
            {
                Assert.That(newTc.Tag, Is.EqualTo(tc.Tag), nameof(tc.Tag));
                Assert.That(newTc.Dkm, Is.EqualTo(tc.Dkm), nameof(tc.Dkm));
            }
        }
    }
}
