using Newtonsoft.Json;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigGen.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.SigGen;

[TestFixture]
public class ContractResolverTests
{
    private TestGroup _group;
    private TestCase _testCase;

    [SetUp]
    public void Setup()
    {
        _testCase = new TestCase { TestCaseId = 1, Message = new BitString("AABBCCDD"), MessageLength = 32, Signature = new BitString("FFEEDDCC") };
        _group = new TestGroup
        {
            TestGroupId = 1,
            TestType = "AFT",
            XmssMode = XmssMode.XMSS_SHA2_10_256,
            PublicKey = new BitString("0123456789ABCDEF"),
            Tests = [_testCase]
        };
    }

    [Test]
    public void PromptProjectionContractResolver_SerializesCorrectProperties()
    {
        // Arrange
        var settings = new JsonSerializerSettings { ContractResolver = new PromptProjectionContractResolver() };

        // Act
        var jsonGroup = JsonConvert.SerializeObject(_group, settings);
        var jsonCase = JsonConvert.SerializeObject(_testCase, settings);

        // Assert
        Assert.That(jsonGroup, Does.Contain("tgId"));
        Assert.That(jsonGroup, Does.Contain("testType"));
        Assert.That(jsonGroup, Does.Contain("xmssMode"));
        Assert.That(jsonGroup, Does.Contain("tests"));
        Assert.That(jsonGroup, Does.Not.Contain("publicKey"));

        Assert.That(jsonCase, Does.Contain("tcId"));
        Assert.That(jsonCase, Does.Contain("message"));
        Assert.That(jsonCase, Does.Contain("messageLength"));
        Assert.That(jsonCase, Does.Not.Contain("signature"));
    }

    [Test]
    public void ResultProjectionContractResolver_SerializesCorrectProperties()
    {
        // Arrange
        var settings = new JsonSerializerSettings { ContractResolver = new ResultProjectionContractResolver() };

        // Act
        var jsonGroup = JsonConvert.SerializeObject(_group, settings);
        var jsonCase = JsonConvert.SerializeObject(_testCase, settings);

        // Assert
        Assert.That(jsonGroup, Does.Contain("tgId"));
        Assert.That(jsonGroup, Does.Contain("tests"));
        Assert.That(jsonGroup, Does.Contain("publicKey"));
        Assert.That(jsonGroup, Does.Not.Contain("testType"));
        Assert.That(jsonGroup, Does.Not.Contain("xmssMode"));

        Assert.That(jsonCase, Does.Contain("tcId"));
        Assert.That(jsonCase, Does.Contain("signature"));
        Assert.That(jsonCase, Does.Not.Contain("message"));
        Assert.That(jsonCase, Does.Not.Contain("messageLength"));
    }
}
