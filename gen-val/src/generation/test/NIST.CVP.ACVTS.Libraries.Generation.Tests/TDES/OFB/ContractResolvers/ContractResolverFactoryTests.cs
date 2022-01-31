using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_OFB.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_OFB.v1_0.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.OFB.ContractResolvers
{
    [TestFixture, UnitTest]
    public class ContractResolverFactoryTests
    {
        private readonly ContractResolverFactory _subject = new ContractResolverFactory();

        [Test]
        [TestCase(Projection.Server, typeof(ServerProjectionContractResolver<TestGroup, TestCase>))]
        [TestCase(Projection.Prompt, typeof(PromptProjectionContractResolver))]
        [TestCase(Projection.Result, typeof(ResultProjectionContractResolver))]
        public void ShouldReturnAppropriateTypeBasedOnProjection(Projection projection, Type expectedType)
        {
            var result = _subject.GetContractResolver(projection);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldThrowArgumentExceptionWithInvalidEnum()
        {
            var projection = (Projection)(-42);

            Assert.Throws(typeof(ArgumentException), () => _subject.GetContractResolver(projection));
        }
    }
}
