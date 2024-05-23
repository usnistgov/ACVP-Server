using System;
using System.Collections;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Kyber;

public class Kyber : IMLKEM
{
    private readonly KyberParameters _param;
    private readonly IShake _shake256;
    private readonly IShake _shake128;
    private readonly ISha _sha3_512;
    private readonly ISha _sha3_256;

    private readonly int[] _zeta =
    {
        1, 17, 289, 1584, 296, 1703, 2319, 2804, 1062, 1409, 
        650, 1063, 1426, 939, 2647, 1722, 2642, 1637, 1197, 
        375, 3046, 1847, 1438, 1143, 2786, 756, 2865, 2099, 
        2393, 733, 2474, 2110, 2580, 583, 3253, 2037, 1339, 
        2789, 807, 403, 193, 3281, 2513, 2773, 535, 2437, 1481, 
        1874, 1897, 2288, 2277, 2090, 2240, 1461, 1534, 2775, 
        569, 3015, 1320, 2466, 1974, 268, 1227, 885, 1729, 2761, 
        331, 2298, 2447, 1651, 1435, 1092, 1919, 2662, 1977,
        319, 2094, 2308, 2617, 1212, 630, 723, 2304, 2549, 56,
        952, 2868, 2150, 3260, 2156, 33, 561, 2879, 2337, 3110,
        2935, 3289, 2649, 1756, 3220, 1476, 1789, 452, 1026, 797,
        233, 632, 757, 2882, 2388, 648, 1029, 848, 1100, 2055, 
        1645, 1333, 2687, 2402, 886, 1746, 3050, 1915, 2594, 821,
        641, 910, 2154, 3328, 3312, 3040, 1745, 3033, 1626, 1010,
        525, 2267, 1920, 2679, 2266, 1903, 2390, 682, 1607, 687, 
        1692, 2132, 2954, 283, 1482, 1891, 2186, 543, 2573, 464, 
        1230, 936, 2596, 855, 1219, 749, 2746, 76, 1292, 1990, 540,
        2522, 2926, 3136, 48, 816, 556, 2794, 892, 1848, 1455, 1432,
        1041, 1052, 1239, 1089, 1868, 1795, 554, 2760, 314, 2009, 
        863, 1355, 3061, 2102, 2444, 1600, 568, 2998, 1031, 882, 
        1678, 1894, 2237, 1410, 667, 1352, 3010, 1235, 1021, 712, 
        2117, 2699, 2606, 1025, 780, 3273, 2377, 461, 1179, 69, 
        1173, 3296, 2768, 450, 992, 219, 394, 40, 680, 1573, 109, 
        1853, 1540, 2877, 2303, 2532, 3096, 2697, 2572, 447, 941, 
        2681, 2300, 2481, 2229, 1274, 1684, 1996, 642, 927, 2443, 
        1583, 279, 1414, 735, 2508, 2688, 2419, 1175
    };
    
    public Kyber(KyberParameters param, IShaFactory shaFactory)
    {
        _param = param;
        _shake128 = shaFactory.GetShakeInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d128));
        _shake256 = shaFactory.GetShakeInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
        _sha3_256 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d256));
        _sha3_512 = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d512));
    }

    /// <summary>
    /// Generate an encapsulation and decapsulation key pair
    /// </summary>
    /// <param name="z">Random concatenation on decapsulation key</param>
    /// <param name="d">Random seed provided to K-PKE.KeyGen</param>
    /// <returns>Tuple containing (encapsulation key ek, decapsulation key dk)</returns>
    public (byte[] ek, byte[] dk) GenerateKey(byte[] z, byte[] d)
    {
        // Console.WriteLine($"Key Generation -- {EnumHelpers.GetEnumDescriptionFromEnum(_param.ParameterSet)}");
        // Console.WriteLine("z: " + IntermediateValueHelper.Print(z));
        // Console.WriteLine("d: " + IntermediateValueHelper.Print(d));
        // Console.WriteLine();
        
        var (ek_pke, dk_pke) = K_Pke_KeyGen(d);
        var dk = dk_pke.Concatenate(ek_pke).Concatenate(H(ek_pke)).Concatenate(z);
        
        // Console.WriteLine("ek: " + IntermediateValueHelper.Print(ek_pke));
        // Console.WriteLine("dk: " + IntermediateValueHelper.Print(dk));
        
        return (ek_pke, dk);
    }

    /// <summary>
    /// Derive and encapsulate a shared secret
    /// </summary>
    /// <param name="ek">Encapsulation key</param>
    /// <param name="m">Random seed, 32 bytes</param>
    /// <returns>Tuple containing (shared secret K, ciphertext c)</returns>
    public (byte[] K, byte[] c) Encapsulate(byte[] ek, byte[] m)
    {
        Console.WriteLine($"Encapsulation -- {EnumHelpers.GetEnumDescriptionFromEnum(_param.ParameterSet)}");
        Console.WriteLine("ek: " + IntermediateValueHelper.Print(ek));
        Console.WriteLine("m: " + IntermediateValueHelper.Print(m));
        Console.WriteLine();

        var (K, r) = G(m.Concatenate(H(ek)));
        
        Console.WriteLine("K: " + IntermediateValueHelper.Print(K));
        Console.WriteLine("r: " + IntermediateValueHelper.Print(r));
        
        var c = K_Pke_Encrypt(ek, m, r);
        
        Console.WriteLine("c: " + IntermediateValueHelper.Print(c));
        
        return (K, c);
    }

    /// <summary>
    /// Decapsulate a shared secret
    /// </summary>
    /// <param name="dk">Decapsulation key</param>
    /// <param name="c">Encapsulated shared secret</param>
    /// <returns>Decapsulated shared secret</returns>
    public (byte[] sharedKey, bool implicitRejection) Decapsulate(byte[] dk, byte[] c)
    {
        var k384 = 384 * _param.K;
        
        // Console.WriteLine($"Decapsulation -- {EnumHelpers.GetEnumDescriptionFromEnum(_param.ParameterSet)}");
        // Console.WriteLine("dk: " + IntermediateValueHelper.Print(dk));
        // Console.WriteLine("c: " + IntermediateValueHelper.Print(c));
        // Console.WriteLine();

        var dk_pke = dk[..k384];
        var ek_pke = dk[k384..(2 * k384 + 32)];
        var h = dk[(2 * k384 + 32)..(2 * k384 + 64)];
        var z = dk[(2 * k384 + 64)..];

        // Console.WriteLine("dk_pke: " + IntermediateValueHelper.Print(dk_pke));
        // Console.WriteLine("ek_pke: " + IntermediateValueHelper.Print(ek_pke));
        // Console.WriteLine("h: " + IntermediateValueHelper.Print(h));
        // Console.WriteLine("z: " + IntermediateValueHelper.Print(z));
        // Console.WriteLine();

        var mPrime = K_Pke_Decrypt(dk_pke, c);
        
        // Console.WriteLine("mPrime: " + IntermediateValueHelper.Print(mPrime));
        // Console.WriteLine();

        var (KPrime, rPrime) = G(mPrime.Concatenate(h));
        
        // Console.WriteLine("KPrime: " + IntermediateValueHelper.Print(KPrime));
        // Console.WriteLine("rPrime: " + IntermediateValueHelper.Print(rPrime));
        // Console.WriteLine();

        var KBar = J(z.Concatenate(c));
        var cPrime = K_Pke_Encrypt(ek_pke, mPrime, rPrime);

        // Console.WriteLine("KBar: " + IntermediateValueHelper.Print(KBar));
        // Console.WriteLine("cPrime: " + IntermediateValueHelper.Print(cPrime));
        // Console.WriteLine();

        var implicitRejection = false;
        if (!c.SequenceEqual(cPrime))
        {
            // Console.WriteLine("Rejected, returning KBar");
            KPrime = KBar;
            implicitRejection = true;
        }
        else
        {
            // Console.WriteLine("Accepted, returning KPrime");
        }

        return (KPrime, implicitRejection);
    }
    
    /// <summary>
    /// PRF = SHAKE256(s || b, 64 * eta)
    /// </summary>
    /// <param name="eta">Output length modifier</param>
    /// <param name="s">32 bytes</param>
    /// <param name="b">Single byte</param>
    /// <returns>byte[] with 64 * eta bytes</returns>
    public byte[] Prf(int eta, byte[] s, byte b)
    {
        var result = new byte[64 * eta];
        
        _shake256.Init();
        _shake256.Update(s, s.Length * 8);
        _shake256.Update(new [] { b }, 8);
        _shake256.Final(result, result.Length * 8);

        return result;
    }

    /// <summary>
    /// XOF = SHAKE128(rho || i || j, outputLength)
    /// </summary>
    /// <param name="rho">32 bytes</param>
    /// <param name="i">Single byte</param>
    /// <param name="j">Single byte</param>
    /// <param name="outputLength">Output length</param>
    /// <returns>byte[] with outputLength bytes</returns>
    public byte[] Xof(byte[] rho, byte i, byte j, int outputLength)
    {
        // NOTE moved to calling function
        var result = new byte[outputLength];

        _shake128.Init();
        _shake128.Update(rho, rho.Length * 8);
        _shake128.Update(new [] { i, j }, 16);
        _shake128.Final(result, result.Length * 8);

        return result;
    }

    /// <summary>
    /// SHA3-256(s)
    /// </summary>
    /// <param name="s"></param>
    /// <returns>32 bytes</returns>
    public byte[] H(byte[] s)
    {
        var result = new byte[32];
        
        _sha3_256.Init();
        _sha3_256.Update(s, s.Length * 8);
        _sha3_256.Final(result, result.Length * 8);

        return result;
    }

    /// <summary>
    /// SHAKE-256(s)
    /// </summary>
    /// <param name="s"></param>
    /// <returns>32 bytes</returns>
    public byte[] J(byte[] s)
    {
        var result = new byte[32];
        
        _shake256.Init();
        _shake256.Update(s, s.Length * 8);
        _shake256.Final(result, result.Length * 8);

        return result;
    }

    /// <summary>
    /// a || b = SHA3-512(c) 
    /// </summary>
    /// <param name="c"></param>
    /// <returns>(a, b) where both elements are 32 bytes</returns>
    public (byte[] a, byte[] b) G(byte[] c)
    {
        var result = new byte[64];
        
        _sha3_512.Init();
        _sha3_512.Update(c, c.Length * 8);
        _sha3_512.Final(result, result.Length * 8);

        return (result[..32], result[32..]);
    }
    
    /// <summary>
    /// Algorithm 2
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
    /// Not specified but helpful for zeta computation
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
    /// Not specified but helpful for zeta computation
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
    /// Algorithm 3
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

    public int Compress(int d, int x)
    {
        return ((x * d.Exp2() + (_param.Q / 2)) / _param.Q);
    }

    public int Decompress(int d, int y)
    {
        return ((y * _param.Q + (d.Exp2() / 2)) / d.Exp2());
    }
    
    /// <summary>
    /// Algorithm 4
    /// </summary>
    /// <param name="d">d = [1, 12]</param>
    /// <param name="F">Array of 256 integers modulo 2^d, or modulo q if d = 12</param>
    /// <returns></returns>
    public byte[] ByteEncode(int d, int[] F)
    {
        var b = new BitArray(256 * d);
        
        for (var i = 0; i < 256; i++)
        {
            var a = F[i];
            for (var j = 0; j < d; j++)
            {
                b[(i * d) + j] = (a % 2 == 1);  // Want this to be true (1) when there is a remainder
                a = (a - (b[(i * d) + j] ? 1 : 0)) / 2;
            }
        }

        return BitsToBytes(b);
    }

    /// <summary>
    /// Algorithm 5
    /// </summary>
    /// <param name="d">d = [1, 12]</param>
    /// <param name="B">Encoded byte[] with 32 * d elements</param>
    /// <returns></returns>
    public int[] ByteDecode(int d, byte[] B)
    {
        var b = BytesToBits(B);
        var F = new int[256];
        var m = d == 12 ? _param.Q : d.Exp2();

        for (var i = 0; i < 256; i++)
        {
            for (var j = 0; j < d; j++)
            {
                F[i] = (F[i] + (b[(i * d) + j] ? j.Exp2() : 0)) % m;
            }
        }

        return F;
    }

    /// <summary>
    /// Algorithm 6
    /// </summary>
    /// <param name="seed"></param>
    /// <param name="l"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    //public int[] SampleNTT(byte[] B)
    public int[] SampleNTT(byte[] seed, byte l, byte k)
    {
        var i = 0;
        var j = 0;
        var aHat = new int[256];
        var B = new byte[168];
        var squeezeFactor = 1;
        
        _shake128.Init();
        _shake128.Absorb(seed, 256);
        _shake128.Absorb(new [] { l, k }, 16);
        _shake128.Squeeze(B, 168 * 8);
        
        while (j < 256)
        {
            if (i == 168)
            {
                squeezeFactor++;
                var output = new byte[168 * squeezeFactor];
                _shake128.Squeeze(output, 168 * 8 * squeezeFactor);
                Array.Copy(output, 168 * (squeezeFactor - 1), B, 0, 168);
                i = 0;
            }
            
            var d1 = B[i] + (256 * (B[i + 1] % 16));
            var d2 = (B[i + 1] / 16) + (16 * B[i + 2]);

            if (d1 < _param.Q)
            {
                aHat[j] = d1;
                j++;
            }

            if (d2 < _param.Q && j < 256)
            {
                aHat[j] = d2;
                j++;
            }

            i += 3;
        }

        return aHat;
    }

    /// <summary>
    /// Algorithm 7
    /// </summary>
    /// <param name="eta"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    public int[] SamplePolyCBD(int eta, byte[] B)
    {
        var b = BytesToBits(B);
        var f = new int[256];

        for (var i = 0; i < 256; i++)
        {
            var x = 0;
            var y = 0;
            
            for (var j = 0; j < eta; j++)
            {
                x += b[(i * eta * 2) + j] ? 1 : 0;
                y += b[(i * eta * 2) + eta + j] ? 1 : 0;
            }

            f[i] = (x - y).PosMod(_param.Q);
        }

        return f;
    }
    
    /// <summary>
    /// Algorithm 8
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public int[] NTT(int[] f)
    {
        var fHat = new int[256];
        f.CopyTo(fHat, 0);
        var k = 1;

        for (var len = 128; len >= 2; len /= 2)
        {
            for (var start = 0; start < 256; start += 2 * len)
            {
                var zeta = ZetaFunction(k); // zeta = Z( bit-reverse(k) ) mod Q
                k++;
                for (var j = start; j < start + len; j++)
                {
                    var t = (zeta * fHat[j + len]).PosMod(_param.Q);
                    fHat[j + len] = (fHat[j] - t).PosMod(_param.Q);
                    fHat[j] = (fHat[j] + t).PosMod(_param.Q);
                }
            }
        }

        return fHat;
    }
    
    /// <summary>
    /// Algorithm 9
    /// </summary>
    /// <param name="fHat"></param>
    /// <returns></returns>
    public int[] NTTInverse(int[] fHat)
    {
        var f = new int[256];
        fHat.CopyTo(f, 0);
        var k = 127;

        for (var len = 2; len <= 128; len *= 2)
        {
            for (var start = 0; start < 256; start += 2 * len)
            {
                var zeta = ZetaFunction(k);
                k--;
                for (var j = start; j < start + len; j++)
                {
                    var t = f[j];
                    f[j] = (t + f[j + len]).PosMod(_param.Q);
                    f[j + len] = ((f[j + len] - t) * zeta).PosMod(_param.Q);
                }
            }
        }

        // Multiply all values by 128^-1 mod q
        return f.Select(value => ((value * 3303).PosMod(_param.Q))).ToArray();
    }

    /// <summary>
    /// Algorithm 10
    /// </summary>
    /// <param name="fHat"></param>
    /// <param name="gHat"></param>
    /// <returns></returns>
    public int[] MultiplyNTTs(int[] fHat, int[] gHat)
    {
        var hHat = new int[256];

        for (var i = 0; i < 128; i++)
        {
            (hHat[2 * i], hHat[(2 * i) + 1]) = BaseCaseMultiply(fHat[2 * i], fHat[(2 * i) + 1], gHat[2 * i], gHat[(2 * i) + 1], ZetaFunction2(i));
        }

        return hHat;
    }

    /// <summary>
    /// Algorithm 11
    /// </summary>
    /// <param name="a0"></param>
    /// <param name="a1"></param>
    /// <param name="b0"></param>
    /// <param name="b1"></param>
    /// <param name="gamma"></param>
    /// <returns></returns>
    public (int c0, int c1) BaseCaseMultiply(long a0, long a1, long b0, long b1, long gamma)
    {
        // long casting in parameters needed because 3300 * 3300 * 3300 exceeds a 32-bit integer
        var c0 = ((a0 * b0) + (a1 * b1 * gamma)).PosMod(_param.Q);
        var c1 = ((a0 * b1) + (a1 * b0)).PosMod(_param.Q);
        return ((int) c0, (int) c1);
    }

    /// <summary>
    /// Algorithm 12
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public (byte[] ek, byte[] dk) K_Pke_KeyGen(byte[] d)
    {
        var (rho, sigma) = G(d);
        byte n = 0;
        var aHat = new int[_param.K][][];
        
        // Console.WriteLine("rho: " + IntermediateValueHelper.Print(rho));
        // Console.WriteLine("sigma: " + IntermediateValueHelper.Print(sigma));
        // Console.WriteLine();
        
        for (var i = 0; i < _param.K; i++)
        {
            aHat[i] = new int[_param.K][];
            
            for (var j = 0; j < _param.K; j++)
            {
                aHat[i][j] = SampleNTT(rho, (byte) j, (byte) i);
            }
        }

        // Console.WriteLine("aHat: " + IntermediateValueHelper.Print3dArray(aHat));
        // Console.WriteLine();

        var s = new int[_param.K][];
        for (var i = 0; i < _param.K; i++)
        {
            s[i] = SamplePolyCBD(_param.Eta1, Prf(_param.Eta1, sigma, n));
            n++;
        }
        
        // Console.WriteLine("s: " + IntermediateValueHelper.Print2dArray(s));
        // Console.WriteLine();

        var e = new int[_param.K][];
        for (var i = 0; i < _param.K; i++)
        {
            e[i] = SamplePolyCBD(_param.Eta1, Prf(_param.Eta1, sigma, n));
            n++;
        }
        
        // Console.WriteLine("e: " + IntermediateValueHelper.Print2dArray(e));
        // Console.WriteLine();

        var sHat = s.Select(NTT).ToArray();
        var eHat = e.Select(NTT).ToArray();
        
        var tHat = MatrixAdd(MatrixMultiply(aHat, sHat), eHat);

        // Console.WriteLine("sHat: " + IntermediateValueHelper.Print2dArray(sHat));
        // Console.WriteLine("eHat: " + IntermediateValueHelper.Print2dArray(eHat));

        // Console.WriteLine("aHat * sHat: " + IntermediateValueHelper.Print2dArray(MatrixMultiply(aHat, sHat)));
        // Console.WriteLine("tHat = aHat * sHat + eHat: " + IntermediateValueHelper.Print2dArray(tHat));
        // Console.WriteLine();

        var ek = Array.Empty<byte>();
        for (var i = 0; i < _param.K; i++)
        {
            ek = ek.Concatenate(ByteEncode(12, tHat[i]));
        }
        ek = ek.Concatenate(rho);

        var dk = Array.Empty<byte>();
        for (var i = 0; i < _param.K; i++)
        {
            dk = dk.Concatenate(ByteEncode(12, sHat[i]));
        }

        return (ek, dk);
    }

    public byte[] K_Pke_Encrypt(byte[] ek, byte[] m, byte[] rand)
    {
        byte n = 0;
        var tHat = new int[_param.K][];
        for (var i = 0; i < _param.K; i++)
        {
            tHat[i] = ByteDecode(12, ek[(i * 384)..(i * 384 + 384)]);
        }

        Console.WriteLine("tHat: " + IntermediateValueHelper.Print2dArray(tHat));
        Console.WriteLine();

        var rho = ek[(_param.K * 384)..];

        var bHat = new int[_param.K][][];
        for (var i = 0; i < _param.K; i++)
        {
            bHat[i] = new int[_param.K][];
            
            for (var j = 0; j < _param.K; j++)
            {
                bHat[i][j] = SampleNTT(rho, (byte) i, (byte) j);    // Note the indexes are inverted from K_Pke_KeyGen to form B = A^T
            }
        }

        Console.WriteLine("bHat = aHat^T: " + IntermediateValueHelper.Print3dArray(bHat));
        Console.WriteLine();

        var r = new int[_param.K][];
        for (var i = 0; i < _param.K; i++)
        {
            r[i] = SamplePolyCBD(_param.Eta1, Prf(_param.Eta1, rand, n));
            n++;
        }
        
        Console.WriteLine("r: " + IntermediateValueHelper.Print2dArray(r));
        Console.WriteLine();

        var e1 = new int[_param.K][];
        for (var i = 0; i < _param.K; i++)
        {
            e1[i] = SamplePolyCBD(_param.Eta2, Prf(_param.Eta2, rand, n));
            n++;
        }

        Console.WriteLine("e1: " + IntermediateValueHelper.Print2dArray(e1));
        Console.WriteLine();

        var e2 = SamplePolyCBD(_param.Eta2, Prf(_param.Eta2, rand, n));
        var rHat = r.Select(NTT).ToArray();
        Console.WriteLine("e2: " + IntermediateValueHelper.PrintArray(e2));
        Console.WriteLine("rHat: " + IntermediateValueHelper.Print2dArray(rHat));
        Console.WriteLine();
        
        var u = MatrixAdd(MatrixMultiply(bHat, rHat).Select(NTTInverse).ToArray(), e1);
        Console.WriteLine("BHat * rHat: " + IntermediateValueHelper.Print2dArray(MatrixMultiply(bHat, rHat)));
        Console.WriteLine("NTTInverse(BHat * rHat): " + IntermediateValueHelper.Print2dArray(MatrixMultiply(bHat, rHat).Select(NTTInverse).ToArray()));
        Console.WriteLine("u = NTTInverse(BHat * rHat) + e1: " + IntermediateValueHelper.Print2dArray(u));
        Console.WriteLine();
        
        var mu = ByteDecode(1, m).Select(x => Decompress(1, x)).ToArray();
        Console.WriteLine("mu: " + IntermediateValueHelper.PrintArray(mu));

        var v = ArrayAdd(ArrayAdd(NTTInverse(VectorTransposeMultiply(tHat, rHat)), e2), mu);
        Console.WriteLine("tHat^T * rHat: " + IntermediateValueHelper.PrintArray(VectorTransposeMultiply(tHat, rHat)));
        Console.WriteLine("NTTInverse(tHat^T * rHat): " + IntermediateValueHelper.PrintArray(NTTInverse(VectorTransposeMultiply(tHat, rHat))));
        Console.WriteLine("e2 + mu: " + IntermediateValueHelper.PrintArray(ArrayAdd(e2, mu)));
        Console.WriteLine("v = NTTInverse(tHat^T * rHat) + e2 + mu: " + IntermediateValueHelper.PrintArray(v));
        Console.WriteLine();

        var c = Array.Empty<byte>();
        for (var i = 0; i < u.Length; i++)
        {
            c = c.Concatenate(ByteEncode(_param.Du, u[i].Select(val => Compress(_param.Du, val)).ToArray()));
        }
        
        c = c.Concatenate(ByteEncode(_param.Dv,  v.Select(val => Compress(_param.Dv, val)).ToArray()));

        return c;
    }

    public byte[] K_Pke_Decrypt(byte[] dk, byte[] c)
    {
        var c1 = c[..(32 * _param.Du * _param.K)];
        var c2 = c[(32 * _param.Du * _param.K)..];
        // Console.WriteLine("c1: " + IntermediateValueHelper.Print(c1));
        // Console.WriteLine("c2: " + IntermediateValueHelper.Print(c2));
        
        var u = new int[_param.K][];
        for (var i = 0; i < _param.K; i++)
        {
            // u[i] = Decompress_du(ByteDecode( each set of 32 * du values in u )
            u[i] = ByteDecode(_param.Du, c1[(i * 32 * _param.Du)..((i * 32 * _param.Du) + (32 * _param.Du))]).Select(b => Decompress(_param.Du, b)).ToArray();
        }
        
        // Console.WriteLine("u: " + IntermediateValueHelper.Print2dArray(u));

        var v = ByteDecode(_param.Dv, c2).Select(b => Decompress(_param.Dv, b)).ToArray();
        // Console.WriteLine("v: " + IntermediateValueHelper.PrintArray(v));
        
        var sHat = new int[_param.K][];
        for (var i = 0; i < _param.K; i++)
        {
            sHat[i] = ByteDecode(12, dk[(i * 384)..((i * 384) + 384)]);
        }
        
        // Console.WriteLine("sHat: " + IntermediateValueHelper.Print2dArray(sHat));

        var w = ArraySubtract(v, NTTInverse(VectorTransposeMultiply(sHat, u.Select(NTT).ToArray())));
        // Console.WriteLine("w: " + IntermediateValueHelper.PrintArray(w));
        
        var m = ByteEncode(1, w.Select(val => Compress(1, val)).ToArray());
        return m;
    }
    
    /// <summary>
    /// Helper function for NTTs that performs a bit-reversal on k and retrieves that index from the precomputed zeta array
    /// </summary>
    /// <param name="k"></param>
    /// <returns></returns>
    private int ZetaFunction(int k)
    {
        // Bit reverse k, and return zeta[rev(k)] from precomputed array
        var kBitsReversed = IntegerToBits(k, 7).Reverse();
        var zetaIndex = BitsToInteger(kBitsReversed, 7);
        return _zeta[zetaIndex];
    }

    /// <summary>
    /// Helper function for NTT multiplication that performs 2 * bit-reversal(k) + 1 and retrieves that index from the precomputed zeta array
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private int ZetaFunction2(int i)
    {
        var iBitsReversed = IntegerToBits(i, 7).Reverse();
        var zetaIndex = (2 * BitsToInteger(iBitsReversed, 7)) + 1;
        return _zeta[zetaIndex];
    }

    private int[] VectorTransposeMultiply(int[][] a, int[][] b)
    {
        var product = new int[256];
        for (var i = 0; i < a.Length; i++)
        {
            var nttProduct = MultiplyNTTs(a[i], b[i]);
            for (var j = 0; j < 256; j++)
            {
                product[j] = (product[j] + nttProduct[j]) % _param.Q;
            }
        }

        return product;
    }
    
    private int[][] MatrixMultiply(int[][][] a, int[][] b)
    {
        // No bounds checking is done to ensure that the proper sized matrices are used
        var aRows = a.Length;
        var aCols = a[0].Length;
    
        var product = new int[aRows][];
    
        for (var i = 0; i < aRows; i++)
        {
            product[i] = new int[256];
            
            for (var j = 0; j < aCols; j++)
            {
                var nttProduct = MultiplyNTTs(a[i][j], b[j]);
                for (var k = 0; k < 256; k++)
                {
                    product[i][k] = (product[i][k] + nttProduct[k]) % _param.Q;
                }
            }
        }
    
        return product;
    }

    private int[] ArrayAdd(int[] a, int[] b)
    {
        var sum = new int[a.Length];
        for (var i = 0; i < a.Length; i++)
        {
            sum[i] = (a[i] + b[i]) % _param.Q;
        }

        return sum;
    }
    
    private int[] ArraySubtract(int[] a, int[] b)
    {
        var sum = new int[a.Length];
        for (var i = 0; i < a.Length; i++)
        {
            sum[i] = (a[i] - b[i]).PosMod(_param.Q);
        }

        return sum;
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
                sum[i][j] = (a[i][j] + b[i][j]) % _param.Q;
            }
        }

        return sum;
    }
}
