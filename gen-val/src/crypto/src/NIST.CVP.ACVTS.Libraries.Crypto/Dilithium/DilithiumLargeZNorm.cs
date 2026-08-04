using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;

/// <summary>
/// An intentionally non-conformant ML-DSA signer used only for generating negative sigVer test cases.
///
/// FIPS 204 Algorithm 7 rejects a candidate signature when ||z|| >= Gamma1 - Beta. This signer inverts
/// that half of the rejection: it keeps only candidates where ||z|| >= Gamma1 - Beta, while leaving the
/// r0, ct0 and hint conditions in place. The resulting signature is internally consistent, so the
/// commitment hash comparison in Verify passes, and the ||z|| bound in FIPS 204 Algorithm 8 becomes the
/// only condition that rejects it. A mutation of an existing signature cannot produce this situation,
/// because any change to z also changes w' and the commitment hash rejects the signature even in a
/// verifier that omits the ||z|| check. See https://github.com/usnistgov/ACVP-Server/issues/462.
///
/// One extra constraint applies: SigEncode packs z coefficients into [-(Gamma1 - 1), Gamma1], and with
/// the rejection disabled a candidate coefficient can reach Gamma1 + Beta. Candidates with any
/// coefficient outside the encodable range are skipped so that SigDecode(SigEncode(sig)) round trips.
///
/// A candidate has ||z|| out of bound roughly 40 percent of the time; combined with the r0 condition
/// and the representability guard, a usable candidate arrives within a handful of iterations.
/// </summary>
public class DilithiumLargeZNorm : Dilithium
{
    private readonly DilithiumParameters _param;
    private readonly IShake _h;

    public DilithiumLargeZNorm(DilithiumParameterSet param, IShaFactory shaFactory, IEntropyProvider entropyProvider = null)
        : base(param, shaFactory, entropyProvider)
    {
        _param = new DilithiumParameters(param);
        _h = shaFactory.GetShakeInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
    }

    public override byte[] Sign(byte[] sk, byte[] m, byte[] rnd)
    {
        var (_, _, tr, _, _, _) = SkDecode(sk);

        // Set up message representative, same as Dilithium.Sign()
        var mu = new byte[64];
        _h.Init();
        _h.Update(BitsToBytes(tr), tr.Length);
        _h.Update(m, m.Length * 8);
        _h.Final(mu, 512);

        return SignWithLargeZ(sk, mu, rnd);
    }

    public override byte[] SignExternalMu(byte[] sk, byte[] mu, byte[] rnd)
    {
        return SignWithLargeZ(sk, mu, rnd);
    }

    /// <summary>
    /// The Dilithium.Sign() candidate loop with the ||z|| rejection inverted. Structure and naming
    /// follow Dilithium.Sign() so the two are easy to diff; only the candidate acceptance differs.
    /// </summary>
    private byte[] SignWithLargeZ(byte[] sk, byte[] mu, byte[] rnd)
    {
        var (rho, k, _, s1, s2, t0) = SkDecode(sk);
        var s1Hat = s1.Select(NTT).ToArray();
        var s2Hat = s2.Select(NTT).ToArray();
        var t0Hat = t0.Select(NTT).ToArray();
        var aHat = ExpandA(rho);

        var rhoPrime = new byte[64];
        _h.Init();
        _h.Update(BitsToBytes(k), k.Length);
        _h.Update(rnd.ToArray(), rnd.Length * 8);
        _h.Update(mu, mu.Length * 8);
        _h.Final(rhoPrime, 512);
        var rhoPrimeBits = BytesToBits(rhoPrime);

        var kappa = 0;
        var cTilde = new byte[(2 * _param.Lambda) / 8];
        int[][] z;
        int[][] h;

        do
        {
            var y = ExpandMask(rhoPrimeBits, kappa);
            var yHat = y.Select(NTT).ToArray();
            var w = MatrixMultiply(aHat, yHat).Select(NTTInverse).ToArray();

            var w1 = w.Select(polynomial => polynomial.Select(HighBits).ToArray()).ToArray();
            var w1Encode = W1Encode(w1);

            _h.Init();
            _h.Update(mu, mu.Length * 8);
            _h.Update(BitsToBytes(w1Encode), w1Encode.Length);
            _h.Final(cTilde, 2 * _param.Lambda);

            var c = SampleInBall(cTilde);
            var cHat = NTT(c);

            var cs1 = PairwiseMultiply(cHat, s1Hat).Select(NTTInverse).ToArray();
            var cs2 = PairwiseMultiply(cHat, s2Hat).Select(NTTInverse).ToArray();
            z = MatrixAdd(y, cs1);

            var r0 = MatrixSubtract(w, cs2).Select(polynomial => polynomial.Select(LowBits).ToArray()).ToArray();

            // Inverted from Dilithium.Sign(): keep the candidate only when ||z|| is out of bound. The
            // representability guard and the r0 condition keep the rest of the signature well-formed.
            if (InfinityNorm(z) < _param.Gamma1 - _param.Beta ||
                !IsRepresentable(z) ||
                InfinityNorm(r0) >= _param.Gamma2 - _param.Beta)
            {
                z = null;
                h = null;
            }
            else
            {
                var ct0 = PairwiseMultiply(cHat, t0Hat).Select(NTTInverse).ToArray();
                var negatedCt0 = NegateMatrix(ct0);
                var wMinusCs2PlusCt0 = MatrixAdd(MatrixSubtract(w, cs2), ct0);

                h = new int[ct0.Length][];
                var sumH = 0;

                for (var i = 0; i < ct0.Length; i++)
                {
                    h[i] = new int[ct0[i].Length];
                    for (var j = 0; j < ct0[i].Length; j++)
                    {
                        if (MakeHint(negatedCt0[i][j], wMinusCs2PlusCt0[i][j]))
                        {
                            h[i][j] = 1;
                            sumH++;
                        }
                    }
                }

                if (InfinityNorm(ct0) >= _param.Gamma2 || sumH > _param.Omega)
                {
                    z = null;
                    h = null;
                }
            }

            kappa += _param.L;

        } while (z == null && h == null);

        return SigEncode(BytesToBits(cTilde), z, h);
    }

    /// <summary>
    /// SigEncode packs each z coefficient as Gamma1 - value into 1 + bitlen(Gamma1 - 1) bits, which
    /// covers exactly [-(Gamma1 - 1), Gamma1]. With the ||z|| rejection disabled a coefficient can
    /// land outside that range and would not survive an encode/decode round trip.
    /// </summary>
    private bool IsRepresentable(int[][] z)
    {
        // Coefficients are centered the same way SigEncode does before it packs them
        return z.All(polynomial => polynomial.All(coefficient =>
            coefficient.PlusMinusMod(_param.Q) >= -(_param.Gamma1 - 1) &&
            coefficient.PlusMinusMod(_param.Q) <= _param.Gamma1));
    }
}
