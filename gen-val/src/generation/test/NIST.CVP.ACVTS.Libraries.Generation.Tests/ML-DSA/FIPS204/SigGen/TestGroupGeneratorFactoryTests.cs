using System;
using System.Linq;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.SigGen;

[TestFixture]
public class TestGroupGeneratorFactoryTests
{
    private Mock<IOracle> _mockOracle;
    
    private TestGroupGeneratorFactory _subject;

    [SetUp]
    public void Setup()
    {
        _mockOracle = new Mock<IOracle>();
        _mockOracle.Setup(m => m.CanRetrieveFromPools).Returns(true);
    }
    
    [Test]
    [TestCase(typeof(TestGroupGenerator))]
    [TestCase(typeof(TestGroupGeneratorPools))]
    public void ReturnedResultShouldContainExpectedTypes(Type expectedType)
    {
        _subject = new TestGroupGeneratorFactory(_mockOracle.Object);

        var result = _subject.GetTestGroupGenerators(new Parameters());

        Assert.That(result.Count(w => w.GetType() == expectedType), Is.EqualTo(1));
    }

    [Test]
    public void ReturnedResultShouldContainTwoGenerators()
    {
        _subject = new TestGroupGeneratorFactory(_mockOracle.Object);

        var result = _subject.GetTestGroupGenerators(new Parameters());

        Assert.That(result.Count(), Is.EqualTo(2));
    }
}
