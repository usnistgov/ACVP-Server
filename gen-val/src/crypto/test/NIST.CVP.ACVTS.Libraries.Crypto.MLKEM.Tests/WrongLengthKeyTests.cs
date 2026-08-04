using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.MLKEM.Tests;

[TestFixture]
[FastCryptoTest]
public class WrongLengthKeyTests
{
    [Test]
    [TestCase(MLKEMParameterSet.ML_KEM_512)]
    [TestCase(MLKEMParameterSet.ML_KEM_768)]
    [TestCase(MLKEMParameterSet.ML_KEM_1024)]
    public void ShouldFailEncapsulationKeyCheckWhenLengthIsWrong(MLKEMParameterSet mlkemParameterSet)
    {
        var rand = new Random800_90();

        var z = rand.GetRandomBitString(256).ToBytes();
        var d = rand.GetRandomBitString(256).ToBytes();

        var mlkemParameters = new MLKEMParameters(mlkemParameterSet);
        var mlkem = new MLKEM(mlkemParameters, new NativeShaFactory());

        var goodKey = mlkem.GenerateKey(z, d);

        Assert.That(mlkem.EncapsulationKeyCheck(goodKey.ek), Is.True, "unmodified ek");
        Assert.That(mlkem.EncapsulationKeyCheck(goodKey.ek[..^1]), Is.False, "ek too short");
        Assert.That(mlkem.EncapsulationKeyCheck(goodKey.ek.Concatenate(new byte[] { 0xAA })), Is.False, "ek too long");
    }

    [Test]
    [TestCase(MLKEMParameterSet.ML_KEM_512)]
    [TestCase(MLKEMParameterSet.ML_KEM_768)]
    [TestCase(MLKEMParameterSet.ML_KEM_1024)]
    public void ShouldFailDecapsulationKeyCheckWhenLengthIsWrong(MLKEMParameterSet mlkemParameterSet)
    {
        var rand = new Random800_90();

        var z = rand.GetRandomBitString(256).ToBytes();
        var d = rand.GetRandomBitString(256).ToBytes();

        var mlkemParameters = new MLKEMParameters(mlkemParameterSet);
        var mlkem = new MLKEM(mlkemParameters, new NativeShaFactory());

        var goodKey = mlkem.GenerateKey(z, d);

        Assert.That(mlkem.DecapsulationKeyCheck(goodKey.dk), Is.True, "unmodified dk");
        Assert.That(mlkem.DecapsulationKeyCheck(goodKey.dk[..^1]), Is.False, "dk too short");
        Assert.That(mlkem.DecapsulationKeyCheck(goodKey.dk.Concatenate(new byte[] { 0xAA })), Is.False, "dk too long");
    }
}
