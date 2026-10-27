using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.SigVer;

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

        // Act
        var response = _generator.PrepareGenerator(group, false);

        // Assert
        Assert.That(response, Is.Not.Null);
    }

    [Test]
    public async Task GenerateAsync_CallsOracleAndReturnsTestCase()
    {
        // Arrange
        var group = new TestGroup
        {
            MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Mock<IRandom800_90>().Object, 32, 33)),
            KeyPair = new Mock<NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys.IXmssKeyPair>().Object,
            XmssMode = XmssMode.XMSS_SHA2_10_256
        };
        _generator.PrepareGenerator(group, true);

        var result = new VerifyResult<XmssSignatureResult>
        {
            Result = true,
            VerifiedValue = new XmssSignatureResult
            {
                Message = new BitString("AABBCCDD"),
                Signature = new BitString("FFEEDDCC")
            }
        };
        _mockOracle.Setup(o => o.GetXmssVerifyResultAsync(It.IsAny<XmssSignatureParameters>()))
            .ReturnsAsync(result);

        // Act
        var response = await _generator.GenerateAsync(group, true);

        // Assert
        Assert.That(response.TestCase, Is.Not.Null);
        Assert.That(response.TestCase.Message, Is.EqualTo(result.VerifiedValue.Message));
        Assert.That(response.TestCase.Signature, Is.EqualTo(result.VerifiedValue.Signature));
        Assert.That(response.TestCase.TestPassed, Is.EqualTo(true));
        _mockOracle.Verify(o => o.GetXmssVerifyResultAsync(It.IsAny<XmssSignatureParameters>()), Times.Once);
    }
}
