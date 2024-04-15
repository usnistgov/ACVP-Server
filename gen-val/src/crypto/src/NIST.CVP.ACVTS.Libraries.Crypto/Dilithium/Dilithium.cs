using System;
using System.Collections;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Helpers;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;

public class Dilithium : IMLDSA
{
    private readonly DilithiumParameters _param;
    private readonly IShake _h;
    private readonly IShake _h128;
    private readonly IEntropyProvider _entropyProvider;

    private readonly int[] _zeta = 
    {
        1, 1753, 3073009, 6757063, 3602218, 4234153, 5801164, 3994671, 5010068, 8352605, 1528066, 5346675, 3415069,
        2998219, 1356448, 6195333, 7778734, 1182243, 2508980, 6903432, 394148, 3747250, 7062739, 3105558, 5152541,
        6695264, 4213992, 3980599, 5483103, 7921677, 348812, 8077412, 5178923, 2660408, 4183372, 586241, 5269599,
        2387513, 3482206, 3363542, 4855975, 6400920, 7814814, 5767564, 3756790, 7025525, 4912752, 5365997, 3764867,
        4423672, 2811291, 507927, 2071829, 3195676, 3901472, 860144, 7737789, 4829411, 1736313, 1665318, 2917338,
        2039144, 4561790, 1900052, 3765607, 5720892, 5744944, 6006015, 2740543, 2192938, 5989328, 7009900, 2663378,
        1009365, 1148858, 2647994, 7562881, 8291116, 2683270, 2358373, 2682288, 636927, 1937570, 2491325, 1095468,
        1239911, 3035980, 508145, 2453983, 2678278, 1987814, 6764887, 556856, 4040196, 1011223, 4405932, 5234739,
        8321269, 5258977, 527981, 3704823, 8111961, 7080401, 545376, 676590, 4423473, 2462444, 749577, 6663429,
        7070156, 7727142, 2926054, 557458, 5095502, 7270901, 7655613, 3241972, 1254190, 2925816, 140244, 2815639,
        8129971, 5130263, 1163598, 3345963, 7561656, 6143691, 1054478, 4808194, 6444997, 1277625, 2105286, 3182878,
        6607829, 1787943, 8368538, 4317364, 822541, 482649, 8041997, 1759347, 141835, 5604662, 3123762, 3542485,
        87208, 2028118, 1994046, 928749, 2296099, 2461387, 7277073, 1714295, 4969849, 4892034, 2569011, 3192354,
        6458423, 8052569, 3531229, 5496691, 6600190, 5157610, 7200804, 2101410, 4768667, 4197502, 214880, 7946292,
        1596822, 169688, 4148469, 6444618, 613238, 2312838, 6663603, 7375178, 6084020, 5396636, 7192532, 4361428,
        2642980, 7153756, 3430436, 4795319, 635956, 235407, 2028038, 1853806, 6500539, 6458164, 7598542, 3761513,
        6924527, 3852015, 6346610, 4793971, 6653329, 6125690, 3020393, 6705802, 5926272, 5418153, 3009748, 4805951,
        2513018, 5601629, 6187330, 2129892, 4415111, 4564692, 6987258, 4874037, 4541938, 621164, 7826699, 1460718,
        4611469, 5183169, 1723229, 3870317, 4908348, 6026202, 4606686, 5178987, 2772600, 8106357, 5637006, 1159875,
        5199961, 6018354, 7609976, 7044481, 4620952, 5046034, 4357667, 4430364, 6161950, 7921254, 7987710, 7159240,
        4663471, 4158088, 6545891, 2156050, 8368000, 3374250, 6866265, 2283733, 5925040, 3258457, 5011144, 1858416,
        6201452, 1744507, 7648983
    };
    
    public Dilithium(DilithiumParameters param, IShaFactory shaFactory, IEntropyProvider entropyProvider)
    {
        _param = param;
        _h = shaFactory.GetShakeInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
        _h128 = shaFactory.GetShakeInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d128));
        _entropyProvider = entropyProvider;
    }

    /// <summary>
    /// Generates a key pair (pk, sk)
    /// </summary>
    /// <param name="seed">Random 256-bit array</param>
    /// <returns>Tuple (byte[] pk, byte[] sk) containing the public key and secret key</returns>
    public (byte[] pk, byte[] sk) GenerateKey(BitArray seed)
    {
        var seedBytes = BitsToBytes(seed).Reverse().ToArray();
        var seedMaterial = new byte[128];
        
        _h.Init();
        _h.Update(seedBytes, 256);
        _h.Final(seedMaterial, 1024);

        var rho = BytesToBits(seedMaterial[..32]);
        var rhoPrime = BytesToBits(seedMaterial[32..96]);
        var k = BytesToBits(seedMaterial[96..]);

        // Console.WriteLine($"Key Generation -- {EnumHelpers.GetEnumDescriptionFromEnum(_param.ParameterSet)}");
        // Console.WriteLine("seed: " + IntermediateValueHelper.Print(seed.ToBytes()));
        // Console.WriteLine("rho: " + IntermediateValueHelper.Print(rho.ToBytes()));
        // Console.WriteLine("rhoPrime: " + IntermediateValueHelper.Print(rhoPrime.ToBytes()));
        // Console.WriteLine("k: " + IntermediateValueHelper.Print(k.ToBytes()));
        // Console.WriteLine();

        var aHat = ExpandA(rho);
        var (s1, s2) = ExpandS(rhoPrime);

        // Console.WriteLine("aHat: " + IntermediateValueHelper.Print3dArray(aHat));
        // Console.WriteLine("s1: " + IntermediateValueHelper.Print2dArray(s1));
        // Console.WriteLine("s2: " + IntermediateValueHelper.Print2dArray(s2));
        // Console.WriteLine();
        
        var s1Hat = s1.Select(NTT).ToArray();
        var t = MatrixAdd(MatrixMultiply(aHat, s1Hat).Select(NTTInverse).ToArray(), s2);    // t = NTTInverse(aHat * NTT(s1)) + s2
        // Console.WriteLine("s1Hat: " + IntermediateValueHelper.Print2dArray(s1Hat));
        // Console.WriteLine("aHat * s1Hat: " + IntermediateValueHelper.Print2dArray(MatrixMultiply(aHat, s1Hat)));
        // Console.WriteLine("NTTInverse(aHat * s1Hat): " + IntermediateValueHelper.Print2dArray(MatrixMultiply(aHat, s1Hat).Select(NTTInverse).ToArray()));
        // Console.WriteLine("t: " + IntermediateValueHelper.Print2dArray(t));
        // Console.WriteLine();
        
        // Run Power2Round on each element of the ring of polynomials t and decompose the resulting array of arrays tuples into two arrays of arrays of ints
        var t0 = new int[_param.K][];
        var t1 = new int[_param.K][];

        for (var i = 0; i < _param.K; i++)
        {
            t0[i] = new int[256];
            t1[i] = new int[256];
            
            for (var j = 0; j < 256; j++)
            {
                var (r1, r0) = Power2Round(t[i][j]);
                t0[i][j] = r0;
                t1[i][j] = r1;
            }
        }
        
        // Console.WriteLine("t0: " + IntermediateValueHelper.Print2dArray(t0));
        // Console.WriteLine("t1: " + IntermediateValueHelper.Print2dArray(t1));
        // Console.WriteLine();
        
        var pk = PkEncode(rho, t1);
        
        var tr = new byte[64];
        _h.Init();
        _h.Update(pk, pk.Length * 8);
        _h.Final(tr, 512);
        // Console.WriteLine("tr: " + IntermediateValueHelper.Print(tr));

        var sk = SkEncode(rho, k, BytesToBits(tr), s1, s2, t0);
        // Console.WriteLine("pk: " + IntermediateValueHelper.Print(pk));
        // Console.WriteLine("sk: " + IntermediateValueHelper.Print(sk));
        
        return (pk, sk);
    }

    /// <summary>
    /// Signs a message with a given secret key
    /// </summary>
    /// <param name="sk">Secret key.</param>
    /// <param name="message">Arbitrary set of bits.</param>
    /// <param name="deterministic">Determines if the signature incorporates any randomness. Opposite is "hedged".</param>
    /// <returns>Signature</returns>
    public byte[] Sign(byte[] sk, BitArray message, bool deterministic)
    {
        var (rho, k, tr, s1, s2, t0) = SkDecode(sk);
        var s1Hat = s1.Select(NTT).ToArray();
        var s2Hat = s2.Select(NTT).ToArray();
        var t0Hat = t0.Select(NTT).ToArray();
        var aHat = ExpandA(rho);

        // Set up message representative
        var mu = new byte[64];
        _h.Init();
        _h.Update(BitsToBytes(tr), tr.Length);
        _h.Update(BitsToBytes(message).Reverse().ToArray(), message.Length);
        _h.Final(mu, 512);
        
        // rnd is either 256 random bits, or 256 0-bits. 
        var rnd = new BitArray(256);
        if (!deterministic)
        {
            rnd = _entropyProvider.GetEntropy(256).Bits;
        }

        // Console.WriteLine($"Signature Generation -- {EnumHelpers.GetEnumDescriptionFromEnum(_param.ParameterSet)}");
        // Console.WriteLine("message: " + IntermediateValueHelper.Print(message.ToBytes()));
        // Console.WriteLine("deterministic: " + (deterministic ? "true" : "false"));
        // Console.WriteLine("sk: " + IntermediateValueHelper.Print(sk));
        //
        // Console.WriteLine("rho: " + IntermediateValueHelper.Print(rho.ToBytes()));
        // Console.WriteLine("k: " + IntermediateValueHelper.Print(k.ToBytes()));
        // Console.WriteLine("tr: " + IntermediateValueHelper.Print(tr.ToBytes()));
        // Console.WriteLine("mu: " + IntermediateValueHelper.Print(mu));
        // Console.WriteLine("rnd: " + IntermediateValueHelper.Print(rnd.ToBytes()));
        // Console.WriteLine("aHat: " + IntermediateValueHelper.Print3dArray(aHat));
        // Console.WriteLine("s1Hat: " + IntermediateValueHelper.Print2dArray(s1Hat));
        // Console.WriteLine("s2Hat: " + IntermediateValueHelper.Print2dArray(s2Hat));
        // Console.WriteLine("t0Hat: " + IntermediateValueHelper.Print2dArray(t0Hat));
        // Console.WriteLine();
        
        var rhoPrime = new byte[64];
        _h.Init();
        _h.Update(BitsToBytes(k), k.Length);
        _h.Update(BitsToBytes(rnd), rnd.Length);
        _h.Update(mu, mu.Length * 8);
        _h.Final(rhoPrime, 512);
        var rhoPrimeBits = BytesToBits(rhoPrime);
        // Console.WriteLine("rhoPrime: " + IntermediateValueHelper.Print(rhoPrimeBits.ToBytes()));
        
        var kappa = 0;
        var cTilde = new byte[(2 * _param.Lambda) / 8];
        int[][] z;
        int[][] h;

        do
        {
            var y = ExpandMask(rhoPrimeBits, kappa);
            var yHat = y.Select(NTT).ToArray();
            var w = MatrixMultiply(aHat, yHat).Select(NTTInverse).ToArray();
            // Console.WriteLine("y: " + IntermediateValueHelper.Print2dArray(y));
            // Console.WriteLine("NTT(y): " + IntermediateValueHelper.Print2dArray(yHat));
            // Console.WriteLine("aHat * NTT(y): " + IntermediateValueHelper.Print2dArray(MatrixMultiply(aHat, yHat)));
            // Console.WriteLine("w = NTTInverse(aHat * NTT(y)): " + IntermediateValueHelper.Print2dArray(w));
            // Console.WriteLine();
            
            var w1 = w.Select(polynomial => polynomial.Select(HighBits).ToArray()).ToArray();
            var w1Encode = W1Encode(w1);
            // Console.WriteLine("w1: " + IntermediateValueHelper.Print2dArray(w1));
            // Console.WriteLine("w1Encode: " + IntermediateValueHelper.Print(w1Encode.ToBytes()));
            // Console.WriteLine();
            
            _h.Init();
            _h.Update(mu, mu.Length * 8);
            _h.Update(BitsToBytes(w1Encode), w1Encode.Length);
            _h.Final(cTilde, 2 * _param.Lambda);
            // Console.WriteLine("cTilde: " + IntermediateValueHelper.Print(cTilde));

            var c1Tilde = cTilde[..32]; // Don't need c2Tilde from cTilde
            var c = SampleInBall(BytesToBits(c1Tilde));
            // Console.WriteLine("c1Tilde: " + IntermediateValueHelper.Print(c1Tilde));
            // Console.WriteLine("c: " + IntermediateValueHelper.PrintArray(c));
            
            var cHat = NTT(c);
            // Console.WriteLine("cHat: " + IntermediateValueHelper.PrintArray(cHat));

            var cs1 = PairwiseMultiply(cHat, s1Hat).Select(NTTInverse).ToArray();
            var cs2 = PairwiseMultiply(cHat, s2Hat).Select(NTTInverse).ToArray();
            z = MatrixAdd(y, cs1);
            // Console.WriteLine("cs1: " + IntermediateValueHelper.Print2dArray(cs1));
            // Console.WriteLine("cs2: " + IntermediateValueHelper.Print2dArray(cs2));
            // Console.WriteLine("z: " + IntermediateValueHelper.Print2dArray(z));
            // Console.WriteLine("||z||: " + InfinityNorm(z) + ", " + (InfinityNorm(z) >= _param.Gamma1 - _param.Beta ? "||z|| too large" : "||z|| check passed"));
            
            var r0 = MatrixSubtract(w, cs2).Select(polynomial => polynomial.Select(LowBits).ToArray()).ToArray();
            // Console.WriteLine("r0: " + IntermediateValueHelper.Print2dArray(r0));
            // Console.WriteLine("||r0||" + InfinityNorm(r0) + ", " + (InfinityNorm(r0) >= _param.Gamma2 - _param.Beta ? "||r0|| too large" : "||r0|| check passed"));
            
            if (InfinityNorm(z) >= _param.Gamma1 - _param.Beta || InfinityNorm(r0) >= _param.Gamma2 - _param.Beta)
            {
                z = null;
                h = null;
                // Console.WriteLine("Need new candidate round. ||z|| too large or ||r0|| too large.");
                // Console.WriteLine();
            }
            else
            {
                var ct0 = PairwiseMultiply(cHat, t0Hat).Select(NTTInverse).ToArray();
                // Console.WriteLine("cHat * t0Hat: " + IntermediateValueHelper.Print2dArray(PairwiseMultiply(cHat, t0Hat)));
                // Console.WriteLine("ct0: " + IntermediateValueHelper.Print2dArray(ct0));
                
                var negatedCt0 = NegateMatrix(ct0);
                // Console.WriteLine("-ct0: " + IntermediateValueHelper.Print2dArray(negatedCt0));
                
                var wMinusCs2PlusCt0 = MatrixAdd(MatrixSubtract(w, cs2), ct0);
                // Console.WriteLine("w - cs2 + ct0: " + IntermediateValueHelper.Print2dArray(wMinusCs2PlusCt0));
                
                h = new int[ct0.Length][];
                var sumH = 0;
                
                for (var i = 0; i < ct0.Length; i++)
                {
                    h[i] = new int[ct0[i].Length];
                    for (var j = 0; j < ct0[i].Length; j++)
                    {
                        // Build H and sum at the same time
                        if (MakeHint(negatedCt0[i][j], wMinusCs2PlusCt0[i][j]))
                        {
                            h[i][j] = 1;
                            sumH++;
                        }
                    }
                }

                // Console.WriteLine("h: " + IntermediateValueHelper.Print2dArray(h));
                // Console.WriteLine("||h||: " + sumH + ", " + (sumH > _param.Omega ? "Too many hint values" : "Hint check passed"));
                // Console.WriteLine("||ct0||: " + InfinityNorm(ct0) + ", " + (InfinityNorm(ct0) >= _param.Gamma2 ? "||ct0|| too large" : "||ct0|| check passed"));
                // Console.WriteLine();
                
                if (InfinityNorm(ct0) >= _param.Gamma2 || sumH > _param.Omega)
                {
                    z = null;
                    h = null;
                    // Console.WriteLine("Need new candidate round. ||ct0|| too large, or too many hints required.");
                    // Console.WriteLine();
                }
            }

            kappa += _param.L;

        } while (z == null && h == null);

        // z plus/minus mod Q is performed within SigEncode because it is easier to apply as the elements are iterated
        var sig = SigEncode(BytesToBits(cTilde), z, h);
        // Console.WriteLine("signature: " + IntermediateValueHelper.Print(sig));
        // Console.WriteLine();
        return sig;
    }

    public bool Verify(byte[] pk, byte[] signature, BitArray message)
    {
        var (rho, t1) = PkDecode(pk);
        var (cTilde, z, h) = SigDecode(signature);

        // Console.WriteLine($"Signature Verification -- {EnumHelpers.GetEnumDescriptionFromEnum(_param.ParameterSet)}");
        // Console.WriteLine("pk: " + IntermediateValueHelper.Print(pk));
        // Console.WriteLine("signature: " + IntermediateValueHelper.Print(signature));
        // Console.WriteLine("message: " + IntermediateValueHelper.Print(message.ToBytes()));
        //
        // Console.WriteLine("rho: " + IntermediateValueHelper.Print(rho.ToBytes()));
        // Console.WriteLine("t1: " + IntermediateValueHelper.Print2dArray(t1));
        // Console.WriteLine("cTilde: " + IntermediateValueHelper.Print(cTilde.ToBytes()));
        // Console.WriteLine("z: " + IntermediateValueHelper.Print2dArray(z));
        // Console.WriteLine("h: " + IntermediateValueHelper.Print2dArray(h));
        // Console.WriteLine();
        
        if (h == null)
        {
            return false;
        }

        var hintsProvided = h.Sum(valueArr => valueArr.Count(value => value == 1));
        if (hintsProvided > _param.Omega)
        {
            // Console.WriteLine("Too many hints provided. Provided: " + hintsProvided + ", expected: <=" + _param.Omega);
            return false;
        }
        else
        {
            // Console.WriteLine("Proper number of hints provided. Provided: " + hintsProvided + ", expected: <=" + _param.Omega);
        }

        // Console.WriteLine("||z||: " + InfinityNorm(z) + ", " + (InfinityNorm(z) >= _param.Gamma1 - _param.Beta ? "||z|| too large" : "||z|| check passed"));
        if (InfinityNorm(z) >= _param.Gamma1 - _param.Beta)
        {
            return false;
        }

        var aHat = ExpandA(rho);
        // Console.WriteLine("aHat: " + IntermediateValueHelper.Print3dArray(aHat));

        var tr = new byte[64];
        _h.Init();
        _h.Update(pk, pk.Length * 8);
        _h.Final(tr, 512);
        // Console.WriteLine("trCandidate: " + IntermediateValueHelper.Print(tr));
        
        var mu = new byte[64];
        _h.Init();
        _h.Update(tr, 512);
        _h.Update(BitsToBytes(message).Reverse().ToArray(), message.Length);
        _h.Final(mu, 512);
        // Console.WriteLine("muCandidate: " + IntermediateValueHelper.Print(mu));
        // Console.WriteLine();
        
        var cTilde1 = cTilde.SubArray(0, 256);
        var c = SampleInBall(cTilde1);
        // Console.WriteLine("c: " + IntermediateValueHelper.PrintArray(c));
        // Console.WriteLine();
        
        // Compute wPrimeApprox
        var aHatDotNttZ = MatrixMultiply(aHat, z.Select(NTT).ToArray());
        // Console.WriteLine("NTT(z): " + IntermediateValueHelper.Print2dArray(z.Select(NTT).ToArray()));
        // Console.WriteLine("aHat * NTT(z): " + IntermediateValueHelper.Print2dArray(aHatDotNttZ));
        
        var nttT1TwoD = ScalarMultiply(t1, 1 << _param.D).Select(NTT).ToArray();
        // Console.WriteLine("NTT(t1): " + IntermediateValueHelper.Print2dArray(t1.Select(NTT).ToArray()));
        // Console.WriteLine("NTT(t1) * 2^d: " + IntermediateValueHelper.Print2dArray(nttT1TwoD));
        
        var nttCDotNttT1TwoD = PairwiseMultiply(NTT(c), nttT1TwoD);
        // Console.WriteLine("NTT(c): " + IntermediateValueHelper.PrintArray(NTT(c)));
        // Console.WriteLine("NTT(c) * (NTT(t1) * 2^d): " + IntermediateValueHelper.Print2dArray(nttCDotNttT1TwoD));
        
        var wPrimeApprox = MatrixSubtract(aHatDotNttZ, nttCDotNttT1TwoD).Select(NTTInverse).ToArray();
        // Console.WriteLine("wPrimeApprox: " + IntermediateValueHelper.Print2dArray(wPrimeApprox));
        
        var w1Prime = new int[h.Length][];
        for (var i = 0; i < h.Length; i++)
        {
            w1Prime[i] = new int[h[0].Length];
            for (var j = 0; j < h[0].Length; j++)
            {
                w1Prime[i][j] = UseHint(h[i][j] == 1, wPrimeApprox[i][j]);
            }
        }

        // Console.WriteLine("w1Prime: " + IntermediateValueHelper.Print2dArray(w1Prime));
        
        var cTildePrime = new byte[2 * _param.Lambda / 8];
        var w1EncodeTemp = W1Encode(w1Prime);
        // Console.WriteLine("w1EncodeTemp: " + IntermediateValueHelper.Print(w1EncodeTemp.ToBytes()));

        _h.Init();
        _h.Update(mu, mu.Length * 8);
        _h.Update(BitsToBytes(w1EncodeTemp), w1EncodeTemp.Length);
        _h.Final(cTildePrime, 2 * _param.Lambda);

        // Console.WriteLine("cTilde: " + IntermediateValueHelper.Print(cTilde.ToBytes()));
        // Console.WriteLine("cTildePrime: " + IntermediateValueHelper.Print(cTildePrime));

        if (BitsToBytes(cTilde).SequenceEqual(cTildePrime))
        {
            // Console.WriteLine("cTilde == cTildePrime, signature verified");
            return true;
        }
        else
        {
            // Console.WriteLine("cTilde != cTildePrime, signature rejected");
            return false;
        }
    }
    
    /// <summary>
    /// Algorithm 4
    /// </summary>
    /// <param name="x">Integer value</param>
    /// <param name="alpha">Number of bits to use to express x</param>
    /// <returns></returns>
    public BitArray IntegerToBits(int x, int alpha)
    {
        var y = new BitArray(alpha);
        for (int i = 0; i < alpha; i++)
        {
            y[i] = (x % 2) == 1;
            x /= 2;
        }

        return y;
    }

    /// <summary>
    /// Algorithm 5
    /// </summary>
    /// <param name="y">Array of bits</param>
    /// <param name="alpha">Number of bits in y</param>
    /// <returns></returns>
    public int BitsToInteger(BitArray y, int alpha)
    {
        int x = 0;
        for (var i = 1; i <= alpha; i++)
        {
            x = (2 * x) + (y[alpha - i] ? 1 : 0);
        }

        return x;
    }

    /// <summary>
    /// Algorithm 6
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public byte[] BitsToBytes(BitArray y)
    {
        var c = y.Length;
        var z = new byte[c.CeilingDivide(8)];
        for (var i = 0; i < c; i++)
        {
            z[i / 8] = (byte)(z[i / 8] + ((y[i] ? 1 : 0) * (i % 8).Exp2()));
        }

        return z;
    }

    /// <summary>
    /// Algorithm 7
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public BitArray BytesToBits(byte[] z)
    {
        var d = z.Length;
        var y = new BitArray(d * 8);

        for (int i = 0; i < d; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                y[8 * i + j] = z[i] % 2 == 1;
                z[i] /= 2;
            }
        }

        return y;
    }

    /// <summary>
    /// Algorithm 8
    /// </summary>
    /// <param name="b0"></param>
    /// <param name="b1"></param>
    /// <param name="b2"></param>
    /// <returns>Generates a value from 0 to q-1</returns>
    public int? CoeffFromThreeBytes(byte b0, byte b1, byte b2)
    {
        int z = ((b2 & 127) << 16) | (b1 << 8) | b0;

        if (z < _param.Q)
        {
            return z;
        }

        return null;
    }

    /// <summary>
    /// Algorithm 9
    /// </summary>
    /// <param name="b">4-bit value, 0-15</param>
    /// <returns>Generates a value from -eta to +eta</returns>
    public int? CoeffFromHalfByte(byte b)
    {
        if (_param.Eta == 2 && b < 15)
        {
            return 2 - (b % 5);
        }

        if (_param.Eta == 4 && b < 9)
        {
            return 4 - b;
        }

        return null;
    }

    /// <summary>
    /// Algorithm 10
    /// </summary>
    /// <param name="w">polynomial with coefficients in a 256-length array with values [0, b]</param>
    /// <param name="b"></param>
    /// <returns>byte string representing polynomial w</returns>
    public byte[] SimpleBitPack(int[] w, int b)
    {
        var z = new BitArray(0);
        var bitlen = b.GetExactBitLength();
        for (int i = 0; i < 256; i++)
        {
            z = z.Concatenate(IntegerToBits(w[i], bitlen));
        }

        return BitsToBytes(z);
    }

    /// <summary>
    /// Algorithm 11
    /// </summary>
    /// <param name="w">a polynomial with coefficients in a 256-length array with values [-a, b]</param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public byte[] BitPack(int[] w, int a, int b)
    {
        var z = new BitArray(0);
        var bitlen = (a + b).GetExactBitLength();
        for (var i = 0; i < 256; i++)
        {
            z = z.Concatenate(IntegerToBits(b - w[i], bitlen));
        }

        return BitsToBytes(z);
    }

    /// <summary>
    /// Algorithm 12
    /// </summary>
    /// <param name="v"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public int[] SimpleBitUnpack(byte[] v, int b)
    {
        var w = new int[256];
        var c = b.GetExactBitLength();
        var z = BytesToBits(v);
        for (var i = 0; i < 256; i++)
        {
            w[i] = BitsToInteger(z.SubArray(i * c, c), c);
        }

        return w;
    }

    /// <summary>
    /// Algorithm 13
    /// </summary>
    /// <param name="v"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public int[] BitUnpack(byte[] v, int a, int b)
    {
        var c = (a + b).GetExactBitLength();
        var z = BytesToBits(v);
        var w = new int[256];
        for (var i = 0; i < 256; i++)
        {
            w[i] = b - BitsToInteger(z.SubArray(i * c, c), c);
        }

        return w;
    }

    /// <summary>
    /// Algorithm 14
    /// </summary>
    /// <param name="h">A K x 256 vector of polynomials with binary coefficients and at most Omega coefficients set to 1</param>
    /// <returns></returns>
    public byte[] HintBitPack(int[][] h)
    {
        var y = new byte[_param.Omega + _param.K];
        var index = 0;
        for (var i = 0; i < _param.K; i++)
        {
            for (var j = 0; j < 256; j++)
            {
                if (h[i][j] != 0)
                {
                    y[index] = (byte)j;
                    index++;
                }
            }

            y[_param.Omega + i] = (byte)index; // safe because of an assumption on the structure of h
        }

        return y;
    }

    /// <summary>
    /// Algorithm 15
    /// </summary>
    /// <param name="y"></param>
    /// <returns>null or a K x 256 vector of polynomials with binary coefficients and at most Omega coefficients set to 1</returns>
    public int[][] HintBitUnpack(byte[] y)
    {
        var h = new int[_param.K][];
        var index = 0;

        for (var i = 0; i < _param.K; i++)
        {
            h[i] = new int[256];
            if (y[_param.Omega + i] < index || y[_param.Omega + i] > _param.Omega)
            {
                return null;
            }

            while (index < y[_param.Omega + i])
            {
                h[i][y[index]] = 1;
                index++;
            }
        }

        // Count the remaining unused elements of y to ensure exactly Omega 1 coefficients
        while (index < _param.Omega)
        {
            if (y[index] != 0)
            {
                return null;
            }

            index++;
        }

        return h;
    }

    /// <summary>
    /// Algorithm 16
    /// </summary>
    /// <param name="rho">Public key bits</param>
    /// <param name="t1"></param>
    /// <returns></returns>
    public byte[] PkEncode(BitArray rho, int[][] t1)
    {
        var pk = BitsToBytes(rho);
        var bitlen = (_param.Q - 1).GetExactBitLength() - _param.D;
        for (var i = 0; i < _param.K; i++)
        {
            pk = pk.Concatenate(SimpleBitPack(t1[i], bitlen.Exp2() - 1));
        }

        return pk;
    }

    /// <summary>
    /// Algorithm 17
    /// </summary>
    /// <param name="pk"></param>
    /// <returns>(rho, t1) tuple containing the BitArray rho and vector of polynomials t1</returns>
    public (BitArray rho, int[][] t1) PkDecode(byte[] pk)
    {
        // First 32 bytes are rho
        var y = pk[..32];
        var rho = BytesToBits(y);

        // Remainder form t1
        var t1 = new int[_param.K][];
        var bitlen = (_param.Q - 1).GetExactBitLength() - _param.D;
        var bitsToUnpack = bitlen * 32;
        for (var i = 0; i < _param.K; i++)
        {
            // Grab the next bitlen * 32 bits at a time and unpack them
            var startIndex = 32 + (i * bitsToUnpack);
            var endIndex = startIndex + bitsToUnpack;
            t1[i] = SimpleBitUnpack(pk[startIndex..endIndex], bitlen.Exp2() - 1);
        }

        return (rho, t1);
    }

    /// <summary>
    /// Algorithm 18. There are a lot of concatenations here which may not be too fast, the relative size of the arrays
    /// shouldn't make this a problem though.
    /// </summary>
    /// <param name="rho"></param>
    /// <param name="k"></param>
    /// <param name="tr"></param>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <param name="t0"></param>
    /// <returns></returns>
    public byte[] SkEncode(BitArray rho, BitArray k, BitArray tr, int[][] s1, int[][] s2, int[][] t0)
    {
        var sk = BitsToBytes(rho).Concatenate(BitsToBytes(k)).Concatenate(BitsToBytes(tr));
        
        for (var i = 0; i < _param.L; i++)
        {
            sk = sk.Concatenate(BitPack(s1[i], _param.Eta, _param.Eta));
        }

        for (var i = 0; i < _param.K; i++)
        {
            sk = sk.Concatenate(BitPack(s2[i], _param.Eta, _param.Eta));
        }

        for (var i = 0; i < _param.K; i++)
        {
            sk = sk.Concatenate(BitPack(t0[i], (_param.D - 1).Exp2() - 1, (_param.D - 1).Exp2()));
        }

        return sk;
    }

    /// <summary>
    /// Algorithm 19
    /// </summary>
    /// <param name="sk"></param>
    /// <returns></returns>
    public (BitArray rho, BitArray k, BitArray tr, int[][] s1, int[][] s2, int[][] t0) SkDecode(byte[] sk)
    {
        var rho = BytesToBits(sk[..32]);
        var k = BytesToBits(sk[32..64]);
        var tr = BytesToBits(sk[64..128]);

        var bitlen = 32 * (2 * _param.Eta).GetExactBitLength();
        var yEndIndex = bitlen * _param.L + 128;
        var zEndIndex = yEndIndex + bitlen * _param.K;
        
        var y = sk[128..yEndIndex].Partition(bitlen);
        var z = sk[yEndIndex..zEndIndex].Partition(bitlen);
        var w = sk[zEndIndex..].Partition(32 * _param.D);

        var s1 = new int[_param.L][];
        for (var i = 0; i < _param.L; i++)
        {
            s1[i] = BitUnpack(y[i], _param.Eta, _param.Eta);
        }

        var s2 = new int[_param.K][];
        var t0 = new int[_param.K][];
        for (var i = 0; i < _param.K; i++)
        {
            s2[i] = BitUnpack(z[i], _param.Eta, _param.Eta);
            t0[i] = BitUnpack(w[i], (_param.D - 1).Exp2() - 1, (_param.D - 1).Exp2());
        }

        return (rho, k, tr, s1, s2, t0);
    }

    /// <summary>
    /// Algorithm 20
    /// </summary>
    /// <param name="cTilde"></param>
    /// <param name="z"></param>
    /// <param name="h"></param>
    /// <returns></returns>
    public byte[] SigEncode(BitArray cTilde, int[][] z, int[][] h)
    {
        var sigma = BitsToBytes(cTilde);

        for (var i = 0; i < _param.L; i++)
        {
            sigma = sigma.Concatenate(BitPack(z[i].Select(value => value.PlusMinusMod(_param.Q)).ToArray(), _param.Gamma1 - 1, _param.Gamma1));
        }

        sigma = sigma.Concatenate(HintBitPack(h));

        return sigma;
    }

    /// <summary>
    /// Algorithm 21
    /// </summary>
    /// <param name="sigma"></param>
    /// <returns></returns>
    public (BitArray cTilde, int[][] z, int[][] h) SigDecode(byte[] sigma)
    {
        var bitlen = 32 * (1 + _param.Gamma1 - 1).GetExactBitLength();
        var wEndIndex = _param.Lambda / 4;
        var xEndIndex = wEndIndex + _param.L * bitlen;
        
        var w = sigma[..wEndIndex];
        var x = sigma[wEndIndex..xEndIndex].Partition(bitlen);
        var y = sigma[xEndIndex..];

        var cTilde = BytesToBits(w);

        var z = new int[_param.L][];
        for (var i = 0; i < _param.L; i++)
        {
            z[i] = BitUnpack(x[i], _param.Gamma1 - 1, _param.Gamma1);
        }

        var h = HintBitUnpack(y);

        return (cTilde, z, h);
    }

    /// <summary>
    /// Algorithm 22
    /// </summary>
    /// <param name="w1"></param>
    /// <returns></returns>
    public BitArray W1Encode(int[][] w1)
    {
        // We know that SimpleBitPack will produce 'bitlen' bits for each run, so to avoid repeated concatenations, allocate all at once
        var upperBound = ((_param.Q - 1) / (2 * _param.Gamma2)) - 1;
        var bitlen = 32 * upperBound.GetExactBitLength();
        var w1Tilde = new BitArray(8 * _param.K * bitlen);
        
        for (var i = 0; i < _param.K; i++)
        {
            var packedBits = BytesToBits(SimpleBitPack(w1[i], upperBound));
            for (var j = 0; j < packedBits.Length; j++)
            {
                w1Tilde[(i * 8 * bitlen) + j] = packedBits[j];
            }
        }

        return w1Tilde;
    }

    /// <summary>
    /// Algorithm 23
    /// </summary>
    /// <param name="rho">Random 256-bit array</param>
    /// <returns>A polynomial with coefficients in {-1, 0, 1}</returns>
    public int[] SampleInBall(BitArray rho)
    {
        var c = new int[256];
        var rhoBytes = BitsToBytes(rho);
        var samplingBytes = new byte[8];
        var k = 8;

        _h.Init();
        _h.Absorb(rhoBytes, 256);
        _h.Squeeze(samplingBytes, 64);
        var samplingBits = BytesToBits(samplingBytes);  // These bits don't change despite extending the length of the Squeeze
        
        for (var i = 256 - _param.Tau; i < 256; i++)
        {
            byte[] candidateBytes;

            do
            {
                k++;
                candidateBytes = new byte[k];
                _h.Squeeze(candidateBytes, k * 8);
            } while (candidateBytes[k - 1] > i);
            
            var j = candidateBytes[k - 1];
            c[i] = c[j];
            c[j] = samplingBits[i + _param.Tau - 256] ? -1 : 1;
        }

        return c;
    }

    /// <summary>
    /// Algorithm 24. Comment: May be obvious to an implementer but the pseudocode could be cleaned up to retrieve all 3 bytes at the same time rather than through separate H calls
    /// </summary>
    /// <param name="rho">Random 272-bit array</param>
    /// <returns></returns>
    public int[] RejNTTPoly(BitArray rho)
    {
        var j = 0;
        var c = 0;
        var aHat = new int[256];
        var rhoBytes = BitsToBytes(rho);

        _h128.Init();
        _h128.Absorb(rhoBytes, 272);
        
        do
        {
            var candidateBytes = new byte[c + 3];
            _h128.Squeeze(candidateBytes, (c + 3) * 8);
            
            var retVal = CoeffFromThreeBytes(candidateBytes[c], candidateBytes[c + 1], candidateBytes[c + 2]);
            c += 3;
            
            if (retVal != null)
            {
                aHat[j] = retVal.Value;
                j++;
            }
            
        } while (j < 256);

        return aHat;
    }

    /// <summary>
    /// Algorithm 25
    /// </summary>
    /// <param name="rho">Random 528-bit array</param>
    /// <returns></returns>
    public int[] RejBoundedPoly(BitArray rho)
    {
        var j = 0;
        var c = 0;
        var squeezeFactor = 1;
        var rhoBytes = BitsToBytes(rho);
        var zCandidates = new byte[256];
        var a = new int[256];

        _h.Init();
        _h.Absorb(rhoBytes, 528);
        _h.Squeeze(zCandidates, 256 * 8);
        
        do
        {
            // If we miss enough checks, we need to squeeze more bits, more efficient to do in a batch than per-byte
            if (c == 256)
            {
                c = 0;
                squeezeFactor++;
                var tempZ = new byte[256 * squeezeFactor];
                
                _h.Squeeze(tempZ, 256 * 8 * squeezeFactor);
                zCandidates = tempZ[(256 * (squeezeFactor - 1))..];
            }
            
            var z = zCandidates[c];
            var z0 = CoeffFromHalfByte((byte) (z % 16));    // Least significant 4 bits in z
            var z1 = CoeffFromHalfByte((byte) (z >> 4));    // Most significant 4 bits in z

            if (z0 != null)
            {
                a[j] = z0.Value;
                j++;
            }

            if (z1 != null && j < 256)
            {
                a[j] = z1.Value;
                j++;
            }

            c++;
            
        } while (j < 256);

        return a;
    }

    /// <summary>
    /// Algorithm 26. Comment: Missing return statement for consistency, but the return value is obvious.
    /// </summary>
    /// <param name="rho">Random 256-bit array</param>
    /// <returns></returns>
    public int[][][] ExpandA(BitArray rho)
    {
        var A = new int[_param.K][][];

        for (var r = 0; r < _param.K; r++)
        {
            A[r] = new int[_param.L][];
            
            for (var s = 0; s < _param.L; s++)
            {
                A[r][s] = RejNTTPoly(rho.Concatenate(IntegerToBits(s, 8)).Concatenate(IntegerToBits(r, 8)));
            }
        }

        return A;
    }

    /// <summary>
    /// Algorithm 27
    /// </summary>
    /// <param name="rho">Random 512-bit array</param>
    /// <returns></returns>
    public (int[][] s1, int[][] s2) ExpandS(BitArray rho)
    {
        var s1 = new int[_param.L][];
        var s2 = new int[_param.K][];
        
        for (var r = 0; r < _param.L; r++)
        {
            s1[r] = RejBoundedPoly(rho.Concatenate(IntegerToBits(r, 16)));
        }

        for (var r = 0; r < _param.K; r++)
        {
            s2[r] = RejBoundedPoly(rho.Concatenate(IntegerToBits(r + _param.L, 16)));
        }

        return (s1, s2);
    }

    /// <summary>
    /// Algorithm 28
    /// </summary>
    /// <param name="rho">Random 512-bit array</param>
    /// <param name="mu"></param>
    /// <returns></returns>
    public int[][] ExpandMask(BitArray rho, int mu)
    {
        var c = 1 + (_param.Gamma1 - 1).GetExactBitLength();
        var rhoBytes = BitsToBytes(rho);
        var s = new int[_param.L][];
        
        for (var r = 0; r < _param.L; r++)
        {
            var n = BitsToBytes(IntegerToBits(mu + r, 16));

            var hashBitLength = 32 * c * 8;
            var v = new byte[hashBitLength / 8];
            
            _h.Init();
            _h.Update(rhoBytes, 512);
            _h.Update(n, n.Length * 8);
            _h.Final(v, hashBitLength);

            // Standard pulls bits from the opposite end here, we can take advantage by not generating the full SHAKE
            s[r] = BitUnpack(v, _param.Gamma1 - 1, _param.Gamma1);
        }

        return s;
    }

    /// <summary>
    /// Algorithm 29
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public (int r1, int r0) Power2Round(int r)
    {
        var twoPowD = 1 << _param.D;
        
        var rPlus = r % _param.Q;
        var r0 = rPlus.PlusMinusMod(twoPowD);

        return ((rPlus - r0) / twoPowD, r0);
    }

    /// <summary>
    /// Algorithm 30
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public (int r1, int r0) Decompose(int r)
    {
        var rPlus = r % _param.Q;
        var r0 = rPlus.PlusMinusMod(2 * _param.Gamma2);
        int r1;

        if (rPlus - r0 == _param.Q - 1)
        {
            r1 = 0;
            r0--;
        }
        else
        {
            r1 = (rPlus - r0) / (2 * _param.Gamma2);
        }

        return (r1, r0);
    }

    /// <summary>
    /// Algorithm 31
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public int HighBits(int r)
    {
        return Decompose(r).r1;
    }

    /// <summary>
    /// Algorithm 32
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public int LowBits(int r)
    {
        return Decompose(r).r0;
    }

    /// <summary>
    /// Algorithm 33
    /// </summary>
    /// <param name="z"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public bool MakeHint(int z, int r)
    {
        var r1 = HighBits(r);
        var v1 = HighBits(r + z);
        return r1 != v1;
    }

    /// <summary>
    /// Algorithm 34
    /// </summary>
    /// <param name="h"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public int UseHint(bool h, int r)
    {
        var m = (_param.Q - 1) / (2 * _param.Gamma2);
        var (r1, r0) = Decompose(r);

        if (h)
        {
            if (r0 > 0)
            {
                return (r1 + 1).PosMod(m);
            }
            
            // else r0 <= 0
            return (r1 - 1).PosMod(m);
        }

        // else !h
        return r1;
    }

    /// <summary>
    /// Algorithm 35
    /// </summary>
    /// <param name="w"></param>
    /// <returns></returns>
    public int[] NTT(int[] w)
    {
        var wHat = new int[256];
        w.CopyTo(wHat, 0);
        var k = 0;
        var len = 128;

        while (len >= 1)
        {
            var start = 0;
            while (start < 256)
            {
                k++;
                var zeta = ZetaFunction(k); // zeta = Z( bit-reverse(k) ) mod Q
                for (var j = start; j < start + len; j++)
                {
                    // Need some weird casting to avoid overflow on a 32-bit integer when multiplying
                    var t = (int) ((long)zeta * wHat[j + len]).PosMod(_param.Q);
                    wHat[j + len] = (wHat[j] - t).PosMod(_param.Q);
                    wHat[j] = (wHat[j] + t).PosMod(_param.Q);
                }

                start += (2 * len);
            }

            len /= 2;
        }

        return wHat;
    }

    /// <summary>
    /// Algorithm 36
    /// </summary>
    /// <param name="wHat"></param>
    /// <returns></returns>
    public int[] NTTInverse(int[] wHat)
    {
        var w = new int[256];
        wHat.CopyTo(w, 0);
        var k = 256;
        var len = 1;

        while (len < 256)
        {
            var start = 0;
            while (start < 256)
            {
                k--;
                var zeta = (-ZetaFunction(k)).PosMod(_param.Q);
                for (var j = start; j < start + len; j++)
                {
                    // Need some weird casting to avoid overflow on a 32-bit integer when multiplying
                    var t = w[j];
                    w[j] = (t + w[j + len]).PosMod(_param.Q);
                    w[j + len] = (t - w[j + len]).PosMod(_param.Q);
                    w[j + len] = (int) ((long)w[j + len] * zeta).PosMod(_param.Q);
                }

                start += (2 * len);
            }

            len *= 2;
        }

        // Multiply all values by 256^-1 mod q
        return w.Select(value => (int) (((long)value * 8347681).PosMod(_param.Q))).ToArray();
    }
    
    /// <summary>
    /// Helper function for NTTs that performs a bit-reversal on k and retrieves that index from the precomputed zeta array
    /// </summary>
    /// <param name="k"></param>
    /// <returns></returns>
    private int ZetaFunction(int k)
    {
        // Bit reverse k, and return zeta[rev(k)] from precomputed array
        var kBitsReversed = IntegerToBits(k, 8).Reverse();
        var zetaIndex = BitsToInteger(kBitsReversed, 8);
        return _zeta[zetaIndex];
    }

    private int[][] ScalarMultiply(int[][] a, int b)
    {
        var product = new int[a.Length][];
        for (var i = 0; i < a.Length; i++)
        {
            product[i] = new int[a[0].Length];
            for (var j = 0; j < a[0].Length; j++)
            {
                product[i][j] = (int) ((long)a[i][j] * b).PosMod(_param.Q);
            }
        }

        return product;
    }

    private int[][] PairwiseMultiply(int[] a, int[][] b)
    {
        var product = new int[b.Length][];
        for (var i = 0; i < b.Length; i++)
        {
            product[i] = new int[a.Length];
            for (var j = 0; j < a.Length; j++)
            {
                product[i][j] = (int) ((long)a[j] * b[i][j]).PosMod(_param.Q);
            }
        }

        return product;
    }

    private int[][] MatrixMultiply(int[][][] a, int[][] b)
    {
        var aRows = a.Length;
        var aCols = a[0].Length;

        var product = new int[aRows][];

        for (var i = 0; i < aRows; i++)
        {
            product[i] = new int[256];
            
            for (var j = 0; j < aCols; j++)
            {
                for (var k = 0; k < 256; k++)
                {
                    product[i][k] = (int) (product[i][k] + (long)a[i][j][k] * b[j][k]).PosMod(_param.Q);
                }
            }
        }

        return product;
    }

    private int[][] MatrixAdd(int[][] a, int[][] b)
    {
        var rows = a.Length;
        var cols = a[0].Length;

        var sum = new int[rows][];

        for (var i = 0; i < rows; i++)
        {
            sum[i] = new int[cols];
            for (var j = 0; j < cols; j++)
            {
                sum[i][j] = (a[i][j] + b[i][j]).PosMod(_param.Q);
            }
        }

        return sum;
    }

    private int[][] MatrixSubtract(int[][] a, int[][] b)
    {
        var rows = a.Length;
        var cols = a[0].Length;

        var difference = new int[rows][];

        for (var i = 0; i < rows; i++)
        {
            difference[i] = new int[cols];
            for (var j = 0; j < cols; j++)
            {
                difference[i][j] = (a[i][j] - b[i][j]).PosMod(_param.Q);
            }
        }

        return difference;
    }

    private int[][] NegateMatrix(int[][] a)
    {
        var negated = new int[a.Length][];

        for (var i = 0; i < a.Length; i++)
        {
            negated[i] = new int[a[i].Length];
            for (var j = 0; j < a[i].Length; j++)
            {
                negated[i][j] = _param.Q - a[i][j];
            }
        }

        return negated;
    }

    private int InfinityNorm(int[][] a)
    {
        return a.Max(polynomial => polynomial.Max(value => System.Math.Abs(value.PlusMinusMod(_param.Q))));
    }
}
