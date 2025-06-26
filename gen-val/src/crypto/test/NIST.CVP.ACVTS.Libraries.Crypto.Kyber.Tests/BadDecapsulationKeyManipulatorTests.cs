using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Kyber.Tests;

[TestFixture]
[FastCryptoTest]
public class BadDecapsulationKeyManipulatorTests
{
    [Test]
    [TestCase(KyberParameterSet.ML_KEM_512)]
    [TestCase(KyberParameterSet.ML_KEM_768)]
    [TestCase(KyberParameterSet.ML_KEM_1024)]
    public void ShouldGenerateKeyThatFailsDecapsulationKeyCheck(KyberParameterSet kyberParameterSet)
    {
        // Random tests are not ideal, but this will work on all inputs
        var rand = new Random800_90();
        
        var z = rand.GetRandomBitString(256).ToBytes();
        var d = rand.GetRandomBitString(256).ToBytes();
    
        var kyberParameters = new KyberParameters(kyberParameterSet);
        var kyber = new Kyber(kyberParameters, new NativeShaFactory());
        var keyManipulator = new BadDecapsulationKeyManipulator(kyber);

        var goodKey = kyber.GenerateKey(z, d);
        var badKey = keyManipulator.ManipulateDecapsulationKey(goodKey.dk);
        
        var badKeyShouldFailResult = kyber.DecapsulationKeyCheck(badKey);
        var goodKeyShouldPassResult = kyber.DecapsulationKeyCheck(goodKey.dk);
        
        Assert.That(badKeyShouldFailResult, Is.False);          
        Assert.That(goodKeyShouldPassResult, Is.True);
    }
}
