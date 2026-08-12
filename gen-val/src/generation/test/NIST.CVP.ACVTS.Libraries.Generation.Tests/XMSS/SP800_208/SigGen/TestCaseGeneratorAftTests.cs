using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.SigGen;

[TestFixture]
public class TestCaseGeneratorAftTests
{
    private Mock<IOracle> _mockOracle;
    private TestCaseGeneratorAft _generator;

    [SetUp]
    public void Setup()
    {
        _mockOracle = new Mock<IOracle>();
        _generator = new TestCaseGeneratorAft(_mockOracle.Object);
    }

    [Test]
    public void PrepareGenerator_SetsUpMessageLengths()
    {
        // Arrange
        var group = new TestGroup
        {
            MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Mock<IRandom800_90>().Object, 10, 101)),
            XmssMode = XmssMode.XMSS_SHA2_10_256
        };

        // This value should not be modified by PrepareGenerator
        var casesToGenerate = _generator.NumberOfTestCasesToGenerate;

        // Act
        var response = _generator.PrepareGenerator(group, false);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(_generator.NumberOfTestCasesToGenerate, Is.EqualTo(casesToGenerate));
    }

    [Test]
    public async Task GenerateAsync_IsSample_CallsOracleWithKeyPair()
    {
        // Arrange
        var group = new TestGroup
        {
            MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Mock<IRandom800_90>().Object, 32, 33)),
            KeyPair = new Mock<IXmssKeyPair>().Object,
            XmssMode = XmssMode.XMSS_SHA2_10_256
        };
        _generator.PrepareGenerator(group, true);

        var result = new XmssSignatureResult
        {
            Message = new BitString("AABBCCDD"),
            Signature = new BitString("FFEEDDCC")
        };
        _mockOracle.Setup(o => o.GetXmssSignatureCaseAsync(It.IsAny<XmssSignatureParameters>())).ReturnsAsync(result);

        // Act
        var response = await _generator.GenerateAsync(group, true);

        // Assert
        Assert.That(response.TestCase, Is.Not.Null);
        Assert.That(response.TestCase.Message, Is.EqualTo(result.Message));
        Assert.That(response.TestCase.Signature, Is.EqualTo(result.Signature));
        _mockOracle.Verify(o => o.GetXmssSignatureCaseAsync(It.IsAny<XmssSignatureParameters>()), Times.Once);
    }

    [Test]
    public async Task GenerateAsync_IsNotSample_CallsDeferredOracle()
    {
        // Arrange
        var group = new TestGroup
        {
            MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Mock<IRandom800_90>().Object, 32, 33)),
            XmssMode = XmssMode.XMSS_SHA2_10_256
        };
        _generator.PrepareGenerator(group, false);

        var result = new XmssSignatureResult
        {
            Message = new BitString("AABBCCDD"),
            Signature = new BitString("FFEEDDCC")
        };
        _mockOracle.Setup(o => o.GetDeferredXmssSignatureCaseAsync(It.IsAny<XmssSignatureParameters>())).ReturnsAsync(result);

        // Act
        var response = await _generator.GenerateAsync(group, false);

        // Assert
        Assert.That(response.TestCase, Is.Not.Null);
        Assert.That(response.TestCase.Message, Is.EqualTo(result.Message));
        _mockOracle.Verify(o => o.GetDeferredXmssSignatureCaseAsync(It.IsAny<XmssSignatureParameters>()), Times.Once);
    }
}
