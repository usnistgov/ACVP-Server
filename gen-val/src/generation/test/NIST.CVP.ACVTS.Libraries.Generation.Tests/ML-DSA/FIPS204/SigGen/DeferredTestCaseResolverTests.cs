using System;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.SigGen;

[TestFixture, UnitTest]
public class TestCaseValidatorGdtTests
{
    private Mock<IOracle> _mock;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mock = new Mock<IOracle>();
        _mock.Setup(s =>
                s.CompleteDeferredMLDSASignatureAsync(It.IsAny<MLDSASignatureParameters>(),
                    It.IsAny<MLDSASignatureResult>()))
            .Returns(Task.FromResult(new MLDSAVerificationResult()));
    }
    
    [Test]
    public void ShouldRejectWithNullSignature()
    {
        var serverTestGroup = TestDataMother.GetTestGroups().TestGroups[0];
        var serverTestCase = serverTestGroup.Tests[0];
        
        var clientTestGroup = TestDataMother.GetTestGroups().TestGroups[0];
        var clientTestCase = clientTestGroup.Tests[0];
        clientTestCase.Signature = null;

        var deferredResolver = new DeferredTestCaseResolver(_mock.Object);
        var validator = new TestCaseValidatorGdt(serverTestCase, serverTestGroup, deferredResolver);
        var result = validator.ValidateAsync(clientTestCase).Result;
        
        Assert.AreEqual(Disposition.Failed, result.Result);
        Assert.IsTrue(result.Reason.Contains("signature"));
    }

    [Test]
    public void ShouldRejectWithZeroSignature()
    {
        var serverTestGroup = TestDataMother.GetTestGroups().TestGroups[0];
        var serverTestCase = serverTestGroup.Tests[0];
        
        var clientTestGroup = TestDataMother.GetTestGroups().TestGroups[0];
        var clientTestCase = clientTestGroup.Tests[0];
        clientTestCase.Signature = new BitString(0);

        var deferredResolver = new DeferredTestCaseResolver(_mock.Object);
        var validator = new TestCaseValidatorGdt(serverTestCase, serverTestGroup, deferredResolver);
        var result = validator.ValidateAsync(clientTestCase).Result;
        
        Assert.AreEqual(Disposition.Failed, result.Result);
        Assert.IsTrue(result.Reason.Contains("signature", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public void ShouldAcceptWithValidProperties()
    {
        var serverTestGroup = TestDataMother.GetTestGroups().TestGroups[0];
        var serverTestCase = serverTestGroup.Tests[0];
        
        var clientTestGroup = TestDataMother.GetTestGroups().TestGroups[0];
        var clientTestCase = clientTestGroup.Tests[0];
        clientTestCase.Signature = new BitString(2420 * 8);     // Set it to an actual length, not the fake value from the TestDataMother

        var deferredResolver = new DeferredTestCaseResolver(_mock.Object);
        var validator = new TestCaseValidatorGdt(serverTestCase, serverTestGroup, deferredResolver);
        var result = validator.ValidateAsync(clientTestCase).Result;
        
        Assert.AreEqual(Disposition.Passed, result.Result, result.Reason);
    }
}
