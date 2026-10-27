using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.Shared;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.SigVer;

[TestFixture]
public class TestGroupGeneratorTests
{
    private Mock<IOracle> _mockOracle;
    private TestGroupGenerator _generator;

    [SetUp]
    public void Setup()
    {
        _mockOracle = new Mock<IOracle>();
        _generator = new TestGroupGenerator(_mockOracle.Object);
    }

    [Test]
    public async Task BuildTestGroupsAsync_WithCapabilities_ReturnsCorrectGroups()
    {
        // Arrange
        var parameters = new Parameters
        {
            Capabilities = new GeneralCapabilities
            {
                XmssModes = [XmssMode.XMSS_SHA2_10_256]
            },
            MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
            IsSample = false
        };

        var mockKeyPair = new Mock<IXmssKeyPair>();
        mockKeyPair.Setup(k => k.PublicKey.Key).Returns([0x01, 0x02]);

        _mockOracle.Setup(o => o.GetXmssKeyCaseAsync(It.IsAny<XmssKeyPairParameters>())).ReturnsAsync(new XmssKeyPairResult { KeyPair = mockKeyPair.Object });

        // Act
        var result = await _generator.BuildTestGroupsAsync(parameters);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].XmssMode, Is.EqualTo(XmssMode.XMSS_SHA2_10_256));
        Assert.That(result[0].TestType, Is.EqualTo("AFT"));
    }

    [Test]
    public async Task BuildTestGroupsAsync_CallsOracleAndSetsKeyPair()
    {
        // Arrange
        var parameters = new Parameters
        {
            Capabilities = new GeneralCapabilities
            {
                XmssModes = [XmssMode.XMSS_SHA2_10_256]
            },
            MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
            IsSample = true
        };

        var mockKeyPair = new Mock<IXmssKeyPair>();
        mockKeyPair.Setup(k => k.PublicKey.Key).Returns([0x01, 0x02]);

        _mockOracle.Setup(o => o.GetXmssKeyCaseAsync(It.IsAny<XmssKeyPairParameters>())).ReturnsAsync(new XmssKeyPairResult { KeyPair = mockKeyPair.Object });

        // Act
        var result = await _generator.BuildTestGroupsAsync(parameters);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].KeyPair, Is.Not.Null);
        _mockOracle.Verify(o => o.GetXmssKeyCaseAsync(It.IsAny<XmssKeyPairParameters>()), Times.Once);
    }
}
