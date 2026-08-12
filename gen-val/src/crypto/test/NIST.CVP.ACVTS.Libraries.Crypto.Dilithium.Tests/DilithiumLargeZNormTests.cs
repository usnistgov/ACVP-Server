using System.Collections;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Dilithium.Tests;

[TestFixture]
[FastCryptoTest]
public class DilithiumLargeZNormTests
{
    private static readonly BitString Seed = new BitString("0000000000000000000000000000000000000000000000000000000000000000");
    private static readonly BitString Message = new BitString("0123456789ABCDEF");

    /// <summary>
    /// The signature has to be rejected by a conformant verifier, and the reason has to be the ||z||
    /// bound rather than anything else, so the decoded ||z|| must be at or above gamma1 - beta.
    /// </summary>
    [Test]
    [TestCase(DilithiumParameterSet.ML_DSA_44)]
    [TestCase(DilithiumParameterSet.ML_DSA_65)]
    [TestCase(DilithiumParameterSet.ML_DSA_87)]
    public void ShouldProduceSignatureWithZNormOutOfBound(DilithiumParameterSet parameterSet)
    {
        var param = new DilithiumParameters(parameterSet);
        var dilithium = new DilithiumExposed(parameterSet, new NativeShaFactory());
        var largeZNorm = new DilithiumLargeZNorm(parameterSet, new NativeShaFactory());

        var key = dilithium.GenerateKey(new BitArray(Seed.ToBytes(true)));
        var signature = largeZNorm.Sign(key.sk, Message.ToBytes(), new byte[32]);

        var (_, z, _) = dilithium.SigDecode(signature);

        Assert.That(signature.Length, Is.EqualTo(param.SignatureLength), "signature length");
        Assert.That(dilithium.InfinityNorm(z), Is.GreaterThanOrEqualTo(param.Gamma1 - param.Beta), "||z||");
        Assert.That(dilithium.Verify(key.pk, Message.ToBytes(), signature), Is.False, "verify");
    }

    /// <summary>
    /// The ||z|| bound has to be the only reason the signature fails. Everything else about the
    /// signature stays well formed, so re-deriving the commitment hash the way Verify does has to
    /// reproduce the c_tilde carried in the signature. A mutated signature would fail here, which is
    /// what makes this disposition different from ModifyZ.
    /// </summary>
    [Test]
    [TestCase(DilithiumParameterSet.ML_DSA_44)]
    [TestCase(DilithiumParameterSet.ML_DSA_65)]
    [TestCase(DilithiumParameterSet.ML_DSA_87)]
    public void ShouldProduceSignatureWhereOnlyZNormCheckFails(DilithiumParameterSet parameterSet)
    {
        var param = new DilithiumParameters(parameterSet);
        var dilithium = new DilithiumExposed(parameterSet, new NativeShaFactory());
        var largeZNorm = new DilithiumLargeZNorm(parameterSet, new NativeShaFactory());

        var key = dilithium.GenerateKey(new BitArray(Seed.ToBytes(true)));
        var signature = largeZNorm.Sign(key.sk, Message.ToBytes(), new byte[32]);

        var (cTilde, z, h) = dilithium.SigDecode(signature);

        // h decodes, so the hint block is well formed
        Assert.That(h, Is.Not.Null, "hint decode");

        // Recompute c_tilde' the way FIPS 204 Algorithm 8 does
        var (rho, t1) = dilithium.PkDecode(key.pk);
        var aHat = dilithium.ExpandA(rho);
        var c = dilithium.SampleInBall(dilithium.BitsToBytes(cTilde));
        var cHat = dilithium.NTT(c);
        var zHat = z.Select(dilithium.NTT).ToArray();

        var t1Shifted = t1.Select(polynomial => polynomial.Select(coefficient => coefficient << param.D).ToArray()).ToArray();
        var azMinusCt1 = dilithium.MatrixSubtractExposed(
            dilithium.MatrixMultiplyExposed(aHat, zHat),
            dilithium.PairwiseMultiplyExposed(cHat, t1Shifted.Select(dilithium.NTT).ToArray()));
        var wApprox = azMinusCt1.Select(dilithium.NTTInverse).ToArray();

        var w1 = new int[param.K][];
        for (var i = 0; i < param.K; i++)
        {
            w1[i] = new int[wApprox[i].Length];
            for (var j = 0; j < wApprox[i].Length; j++)
            {
                w1[i][j] = dilithium.UseHint(h[i][j] == 1, wApprox[i][j]);
            }
        }

        var mu = new byte[64];
        var tr = dilithium.SkDecode(key.sk).tr;
        var shake = new NativeShaFactory().GetShakeInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
        shake.Init();
        shake.Update(dilithium.BitsToBytes(tr), tr.Length);
        shake.Update(Message.ToBytes(), Message.ToBytes().Length * 8);
        shake.Final(mu, 512);

        var cTildePrime = new byte[(2 * param.Lambda) / 8];
        var w1Encode = dilithium.W1Encode(w1);
        shake.Init();
        shake.Update(mu, mu.Length * 8);
        shake.Update(dilithium.BitsToBytes(w1Encode), w1Encode.Length);
        shake.Final(cTildePrime, 2 * param.Lambda);

        Assert.That(new BitString(cTildePrime).ToHex(), Is.EqualTo(new BitString(dilithium.BitsToBytes(cTilde)).ToHex()),
            "commitment hash still matches, so ||z|| is the only failing check");
    }

    /// <summary>
    /// SigEncode packs z into [-(gamma1 - 1), gamma1]. With the rejection inverted a candidate
    /// coefficient can exceed that, so the signer skips those; the signature must round trip.
    /// </summary>
    [Test]
    [TestCase(DilithiumParameterSet.ML_DSA_44)]
    [TestCase(DilithiumParameterSet.ML_DSA_65)]
    [TestCase(DilithiumParameterSet.ML_DSA_87)]
    public void ShouldProduceEncodableSignature(DilithiumParameterSet parameterSet)
    {
        var dilithium = new DilithiumExposed(parameterSet, new NativeShaFactory());
        var largeZNorm = new DilithiumLargeZNorm(parameterSet, new NativeShaFactory());

        var key = dilithium.GenerateKey(new BitArray(Seed.ToBytes(true)));
        var signature = largeZNorm.Sign(key.sk, Message.ToBytes(), new byte[32]);

        var (cTilde, z, h) = dilithium.SigDecode(signature);
        var reEncoded = dilithium.SigEncode(cTilde, z, h);

        Assert.That(new BitString(reEncoded).ToHex(), Is.EqualTo(new BitString(signature).ToHex()), "round trip");
    }

    /// <summary>
    /// The polynomial arithmetic needed to re-derive the commitment hash is protected on Dilithium.
    /// Exposing it here keeps the test using the implementation under test rather than a second copy.
    /// </summary>
    private class DilithiumExposed : Dilithium
    {
        public DilithiumExposed(DilithiumParameterSet param, IShaFactory shaFactory) : base(param, shaFactory) { }

        public int[][] MatrixMultiplyExposed(int[][][] a, int[][] b) => MatrixMultiply(a, b);

        public int[][] PairwiseMultiplyExposed(int[] a, int[][] b) => PairwiseMultiply(a, b);

        public int[][] MatrixSubtractExposed(int[][] a, int[][] b) => MatrixSubtract(a, b);
    }
}
