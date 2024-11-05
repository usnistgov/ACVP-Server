using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen.ContractResolvers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.KeyGen;

[TestFixture, UnitTest]
public class ContractResolverFactoryTests
{
    private readonly ContractResolverFactory _subject = new();

    [Test]
    [TestCase(Projection.Server, typeof(ServerProjectionContractResolver<TestGroup, TestCase>))]
    [TestCase(Projection.Prompt, typeof(PromptProjectionContractResolver))]
    [TestCase(Projection.Result, typeof(ResultProjectionContractResolver))]
    public void ShouldReturnAppropriateTypeBasedOnProjection(Projection projection, Type expectedType)
    {
        var result = _subject.GetContractResolver(projection);

        Assert.That(result, Is.InstanceOf(expectedType));
    }

    [Test]
    public void ShouldThrowArgumentExceptionWithInvalidEnum()
    {
        var projection = (Projection)(-42);

        Assert.Throws(typeof(ArgumentException), () => _subject.GetContractResolver(projection));
    }    
}
