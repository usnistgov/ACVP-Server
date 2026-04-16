using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.MLKEM.Tests;

[TestFixture]
[FastCryptoTest]
public class BadDecapsulationKeyManipulatorTests
{
    [Test]
    [TestCase(MLKEMParameterSet.ML_KEM_512)]
    [TestCase(MLKEMParameterSet.ML_KEM_768)]
    [TestCase(MLKEMParameterSet.ML_KEM_1024)]
    public void ShouldGenerateKeyThatFailsDecapsulationKeyCheck(MLKEMParameterSet mlkemParameterSet)
    {
        // Random tests are not ideal, but this will work on all inputs
        var rand = new Random800_90();
        
        var z = rand.GetRandomBitString(256).ToBytes();
        var d = rand.GetRandomBitString(256).ToBytes();
    
        var mlkemParameters = new MLKEMParameters(mlkemParameterSet);
        var mlkem = new MLKEM(mlkemParameters, new NativeShaFactory());
        var keyManipulator = new BadDecapsulationKeyManipulator(mlkem);

        var goodKey = mlkem.GenerateKey(z, d);
        var badKey = keyManipulator.ManipulateDecapsulationKey(goodKey.dk);
        
        var badKeyShouldFailResult = mlkem.DecapsulationKeyCheck(badKey);
        var goodKeyShouldPassResult = mlkem.DecapsulationKeyCheck(goodKey.dk);
        
        Assert.That(badKeyShouldFailResult, Is.False);          
        Assert.That(goodKeyShouldPassResult, Is.True);
    }
}
