using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Ascon;

public class Ascon
{
    private const UInt64 _aead128Iv = 0x00001000808c0001;
    private const UInt64 _hash256Iv = 0x0000080100cc0002;
    private const UInt64 _xof128Iv = 0x0000080000cc0003;
    private const UInt64 _cxof128Iv = 0x0000080000cc0004;

    private readonly UInt64[] _c = { 
        0x000000000000003c, 0x000000000000002d, 
        0x000000000000001e, 0x000000000000000f, 
        0x00000000000000f0, 0x00000000000000e1, 
        0x00000000000000d2, 0x00000000000000c3, 
        0x00000000000000b4, 0x00000000000000a5, 
        0x0000000000000096, 0x0000000000000087,
        0x0000000000000078, 0x0000000000000069,
        0x000000000000005a, 0x000000000000004b,
    };
    
    private readonly byte[] _sBox =
    {
        0x04, 0x0b, 0x1f, 0x14, 0x1a, 0x15, 0x09, 0x02, 0x1b, 0x05, 0x08, 0x12, 0x1d, 0x03, 0x06, 0x1c,
        0x1e, 0x13, 0x07, 0x0e, 0x00, 0x0d, 0x11, 0x18, 0x10, 0x0c, 0x01, 0x19, 0x16, 0x0a, 0x0f, 0x17
    };

    /// <summary>
    /// Perform AEAD encryption
    /// </summary>
    /// <param name="k"></param>
    /// <param name="n"></param>
    /// <param name="a"></param>
    /// <param name="p"></param>
    /// <param name="PBitLength"></param>
    /// <param name="ABitLength"></param>
    /// <param name="tagLen"></param>
    /// <param name="secondK"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public (byte[] c, byte[] tag) Aead128Encrypt(byte[] k, byte[] n, byte[] a, byte[] p, int PBitLength, int ABitLength, int tagLen = 128, byte[] secondK = null)
    {
        
        if (k.Length != 16)
        {
            throw new ArgumentException("Invalid key length provided");
        }

        if (n.Length != 16)
        {
            throw new ArgumentException("Invalid nonce length provided");
        }

        if (secondK != null && secondK.Length != 16)
        {
            throw new ArgumentException("Invalid second key length provided");
        }

        if (!(tagLen >= 64 && tagLen <= 128))
        {
            throw new ArgumentException("Invalid tag length provided");
        }

        if (PBitLength > p.Length * 8)
        {
            throw new ArgumentException("Invalid plaintext bit length provided");
        }

        if (ABitLength > a.Length * 8)
        {
            throw new ArgumentException("Invalid AD bit length provided");
        }

        if (secondK != null)
        {
            for (int i = 0; i < 16; i++)
            {
                n[i] ^= secondK[i];
            }
        }

        // 1.0 Initialization
        var s = new UInt64[5];
        
        s[0] = _aead128Iv;
       
        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        for (var i = 0; i < 8; i++)
        {
            s[1] |= (((UInt64) k[i]) << i * 8);
            s[3] |= (((UInt64) n[i]) << i * 8);
        }

        for (var i = 8; i < 16; i++)
        {
            s[2] |= (((UInt64) k[i]) << i * 8);
            s[4] |= (((UInt64) n[i]) << i * 8);
        }
        
        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        Permute(s, 12);
        
        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        for (var i = 0; i < 8; i++)
        {
            s[3] ^= (((UInt64) k[i]) << i * 8);
            s[4] ^= (((UInt64) k[i+8]) << i * 8);
        }
        
        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 2.0 Processing associated data
        if (a.Length != 0)
        {
            var parsedAad = Parse128(a, ABitLength);

            parsedAad[^1] = Pad128(parsedAad[^1], (ABitLength % 128));

            foreach (var aadChunk in parsedAad)
            {
                s[0] ^= (UInt64)aadChunk;
                s[1] ^= (UInt64)(aadChunk >> 64);
                Permute(s, 8);
            }
        }
        
        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        s[4] ^= 0x8000000000000000;
        
        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 3.0 Processing plaintext
        var parsedPlaintext = Parse128(p, PBitLength);

        var c = new byte[p.Length];

        for (int i = 0; i < PBitLength / 128; i++)
        {
            s[0] ^= (UInt64)parsedPlaintext[i];
            s[1] ^= (UInt64)(parsedPlaintext[i] >> 64);
            for (int j = 0; j < 16; j++)
            {
                if(j < 8)
                {
                    c[(16 * i) + j] = (byte)(s[0] >> (j * 8));
                }
                else
                {
                    c[(16 * i) + j] = (byte)(s[1] >> ((j - 8) * 8));
                }
            }
            Permute(s, 8);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        parsedPlaintext[^1] = Pad128(parsedPlaintext[^1], (PBitLength % 128));

        s[0] ^= (UInt64)parsedPlaintext[^1];
        s[1] ^= (UInt64)(parsedPlaintext[^1] >> 64);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        int remainingBits = PBitLength % 128;

        for (int i = 0; i < remainingBits; i++)
        {
            byte tempByte = 0;
            ulong tempBit = 0;
            if (i < 64)
            {
                tempBit = 1 & (s[0] >> i);
            }
            else
            {
                tempBit = 1 & (s[1] >> (i - 64));
            }
            tempByte |= (byte)(tempBit << (i % 8));
            c[(i / 8) + ((PBitLength / 128) * 16)] |= tempByte;
        }

        // 4.0 Finalization
        for (int i = 0; i < 8; i++)
        {
            s[2] ^= ((UInt64)k[i]) << (i * 8);
            s[3] ^= ((UInt64)k[i+8]) << (i * 8);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        int tagByteLength = tagLen / 8;
        
        if (tagLen % 8 != 0)
        {
            tagByteLength++;
        }
            
        var t = new byte[tagByteLength];
        
        for (int i = 0; i < tagByteLength; i++)
        {
            if(i < 8)
            {
                t[i] = (byte)(((s[3] & ((UInt64)0xff << (i * 8))) >> (8 * i)) ^ k[i]);
            }
            else
            {
                int j = i - 8;
                t[i] = (byte)(((s[4] & ((UInt64)0xff << (j * 8))) >> (8 * j)) ^ k[i]);
            }
        }

        int partial = (128 - tagLen) % 8;
        
        for (int i = 0; i < partial; i++)
        {
            t[15 - ((128 - tagLen) / 8)] &= (byte)~(1 << i);
        }

        return (c, t);
    }

    /// <summary>
    /// Perform AEAD decryption
    /// </summary>
    /// <param name="k"></param>
    /// <param name="n"></param>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <param name="t"></param>
    /// <param name="CBitLength"></param>
    /// <param name="ABitLength"></param>
    /// <param name="tagLen"></param>
    /// <param name="secondK"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public AsconDecryptResult Aead128Decrypt(byte[] k, byte[] n, byte[] a, byte[]c, byte[] t, int CBitLength, int ABitLength, int tagLen = 128, byte[] secondK = null)
    {

        if (k.Length != 16)
        {
            throw new ArgumentException("Invalid key length provided");
        }

        if (n.Length != 16)
        {
            throw new ArgumentException("Invalid nonce length provided");
        }

        if(secondK != null && secondK.Length != 16)
        {
            throw new ArgumentException("Invalid second key length provided");
        }

        if (!(tagLen >= 64 && tagLen <= 128))
        {
            throw new ArgumentException("Invalid tag length provided");
        }

        if (CBitLength > c.Length * 8)
        {
            throw new ArgumentException("Invalid ciphertext bit length provided");
        }

        if (ABitLength > a.Length * 8)
        {
            throw new ArgumentException("Invalid AD bit length provided");
        }

        if (secondK != null)
        {
            for (int i = 0; i < 16; i++)
            {
                n[i] ^= secondK[i];
            }
        }

        // 1.0 Initialization
        var s = new UInt64[5];
        
        s[0] = _aead128Iv;

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        for (var i = 0; i < 8; i++)
        {
            s[1] |= ((UInt64)k[i]) << (i * 8);
            s[3] |= ((UInt64)n[i]) << (i * 8);
        }

        for (var i = 8; i < 16; i++)
        {
            s[2] |= ((UInt64)k[i]) << (i * 8);
            s[4] |= ((UInt64)n[i]) << (i * 8);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        for (var i = 0; i < 8; i++)
        {
            s[3] ^= ((UInt64)k[i]) << (i * 8);
            s[4] ^= ((UInt64)k[i + 8]) << (i * 8);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 2.0 Processing associated data
        if (a.Length != 0)
        {
            var parsedAad = Parse128(a, ABitLength);

            parsedAad[^1] = Pad128(parsedAad[^1], ABitLength % 128);

            foreach (var aadChunk in parsedAad)
            {
                s[0] ^= (UInt64)aadChunk;
                s[1] ^= (UInt64)(aadChunk >> 64);
                Permute(s, 8);
            }
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        s[4] ^= 0x8000000000000000;

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 3.0 Processing plaintext
        var parsedCiphertext = Parse128(c, CBitLength);

        byte[] p = new byte[c.Length];

        for (int i = 0; i < CBitLength / 128; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                if (j < 8)
                {
                    p[(16 * i) + j] = (byte)((s[0] >> (j * 8)) ^ c[(16 * i) + j]);
                }
                else
                {
                    p[(16 * i) + j] = (byte)((s[1] >> ((j - 8) * 8)) ^ c[(16 * i) + j]);
                }
            }
            s[0] = (UInt64)parsedCiphertext[i];
            s[1] = (UInt64)(parsedCiphertext[i] >> 64);
            Permute(s, 8);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        int remainingBits = CBitLength % 128;

        for(int i = 0; i < remainingBits; i++)
        {
            byte tempByte = 0;
            byte tempBit = 0;
            int cIndex = (i / 8) + ((CBitLength / 128) * 16);
            if (i < 64)
            {
                tempBit = (byte)(1 & ((byte)(s[0] >> i) ^ (c[cIndex] >> (i % 8))));
            }
            else
            {
                tempBit = (byte)(1 & ((byte)(s[1] >> (i - 64)) ^ (c[cIndex] >> (i % 8))));
            }
            tempByte |= (byte)(tempBit << (i % 8));
            int pIndex = (i / 8) + ((CBitLength / 128) * 16);
            p[pIndex] |= tempByte;
        }

        if(remainingBits == 0)
        {
            s[0] ^= (UInt64)1;
        }
        else if (remainingBits < 64)
        {
            s[0] ^= ((UInt64)1 << remainingBits);
        }
        else if(remainingBits == 64)
        {
            s[1] ^= (UInt64)1;
        }
        else
        {
            s[1] ^= ((UInt64)1 << (remainingBits - 64));
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        for (int i = 0; i < remainingBits; i++)
        {
            int tempBit = (int)(parsedCiphertext[CBitLength / 128] >> i) & 1;
            if (i < 64)
            {
                s[0] &= ~((UInt64)1 << i);
                s[0] |= (UInt64)tempBit << i;
            }
            else
            {
                s[1] &= ~((UInt64)1 << (i - 64));
                s[1] |= (UInt64)tempBit << (i - 64);
            }
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 4.0 Finalization
        for (int i = 0; i < 8; i++)
        {
            s[2] ^= ((UInt64)k[i]) << (i * 8);
            s[3] ^= ((UInt64)k[i + 8]) << (i * 8);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        int tagByteLength = tagLen / 8;
        
        if (tagLen % 8 != 0)
        {
            tagByteLength++;
        }
        
        var tNew = new byte[tagByteLength];
        
        for (int i = 0; i < tagByteLength; i++)
        {
            if(i < 8)
            {
                UInt64 b1 = s[3] >> (8 * i);
                b1 = b1 ^ k[i];
                tNew[i] = (byte)b1;
            }
            else
            {
                int j = i - 8;
                UInt64 b2 = s[4] >> (8 * j);
                b2 = b2 ^ k[i];
                tNew[i] = (byte)b2;
            }
        }

        int partial = (128 - tagLen) % 8;
        
        for (int i = 0; i < partial; i++)
        {
            tNew[15 - ((128 - tagLen) / 8)] &= (byte)~(1 << i);
        }

        if (t.SequenceEqual(tNew))
        {
            return new AsconDecryptResult(p);
        }
        else
        {
            return new AsconDecryptResult();
        }
    }

    /// <summary>
    /// Hashes into 256 bit digest
    /// </summary>
    /// <param name="m"></param>
    /// <param name="MBitLength"></param>
    /// <returns></returns>
    public byte[] Hash256(byte[] m, int MBitLength)
    {
        // 1.0 Initialization
        var s = new UInt64[5];
        
        s[0] = _hash256Iv;

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 2.0 Absorbing
        var parsedM = Parse64(m, MBitLength);
        
        parsedM[^1] = Pad64(parsedM[^1], MBitLength % 64);
       
        for(int i = 0; i < parsedM.Length - 1; i++)
        {
            s[0] ^= parsedM[i];
            Permute(s, 12);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        s[0] ^= parsedM[^1];

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));
        
        // 3.0 Squeezing
        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        var h64 = new UInt64[4];
       
        for (int i = 0; i < 3; i++)
        {
            h64[i] = s[0];
            Permute(s, 12);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        h64[3] = s[0];
        
        var h = new byte[32];
        
        for(int i = 0; i < 32; i++)
        {
            byte b = (byte)(h64[i / 8] >> 56 - ((i % 8) * 8));
            //Flipping the byte ordering of h64 for output
            //((i / 8) * 8) determines which "chunk" of 8 bytes the byte will go into
            //(7 - (i % 8)) determines where in that chunk the byte goes
            h[((i / 8) * 8) + (7 - (i % 8))] = b;
        }
        
        return h;
    }

    /// <summary>
    /// Hashes into arbitrary length digest
    /// </summary>
    /// <param name="m"></param>
    /// <param name="MBitLength"></param>
    /// <param name="outputLength"></param>
    /// <returns></returns>
    public byte[] Xof128(byte[] m, int MBitLength, int outputLength)
    {
        // 1.0 Initialization
        var s = new UInt64[5];
        
        s[0] = _xof128Iv;

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 2.0 Absorbing
        var parsedM = Parse64(m, MBitLength);
        parsedM[^1] = Pad64(parsedM[^1], MBitLength % 64);
        
        for (int i = 0; i < parsedM.Length - 1; i++)
        {
            s[0] ^= parsedM[i];
            Permute(s, 12);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        s[0] ^= parsedM[^1];

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 3.0 Squeezing
        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        int h64Len = ((outputLength - 1) / 64) + 1;

        var h64 = new UInt64[h64Len];
        
        for (int i = 0; i < h64Len - 1; i++)
        {
            h64[i] = s[0];
            Permute(s, 12);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        h64[^1] = s[0];

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        int hLen = ((outputLength - 1) / 8) + 1;
        
        var h = new byte[hLen];
        
        for(int i = 0; i < outputLength; i++)
        {
            byte b = (byte)((h64[i / 64] >> (i % 64)) & 1);
            h[i / 8] |= (byte)((int)b << (i % 8));
        }
        
        return h;
    }

    /// <summary>
    /// Hashes into arbitrary length digest with customization string
    /// </summary>
    /// <param name="m"></param>
    /// <param name="MBitLength"></param>
    /// <param name="cs"></param>
    /// <param name="CSBitLength"></param>
    /// <param name="outputLength"></param>
    /// <returns></returns>
    public byte[] CXof128(byte[] m, int MBitLength, byte[] cs, int CSBitLength, int outputLength)
    {
        // 1.0 Initialization
        var s = new UInt64[5];
        
        s[0] = _cxof128Iv;

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 2.0 Customization
        var parsedCs = Parse64(cs, CSBitLength);
        var cs64 = new UInt64[parsedCs.Length + 1];
        cs64[0] = (UInt64)CSBitLength;
        
        for(int i = 0; i < parsedCs.Length; i++)
        {
            cs64[i + 1] = parsedCs[i];
        }
        
        cs64[^1] = Pad64(cs64[^1], CSBitLength % 64);
        
        for (int i = 0; i < cs64.Length; i++)
        {
            s[0] ^= cs64[i];
            Permute(s, 12);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 3.0 Absorbing
        var parsedM = Parse64(m, MBitLength);
        
        parsedM[^1] = Pad64(parsedM[^1], MBitLength % 64);

        for (int i = 0; i < parsedM.Length - 1; i++)
        {
            s[0] ^= parsedM[i];
            Permute(s, 12);
        }

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        s[0] ^= parsedM[^1];

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        // 4.0 Squeezing
        Permute(s, 12);

        Console.WriteLine("State:           " + s[0].ToString("X16") + "    " + s[1].ToString("X16") + "    " + s[2].ToString("X16") + "    " + s[3].ToString("X16") + "    " + s[4].ToString("X16"));

        int h64Len = ((outputLength - 1) / 64) + 1;

        var h64 = new UInt64[h64Len];

        for (int i = 0; i < h64Len - 1; i++)
        {
            h64[i] = s[0];
            Permute(s, 12);
        }

        h64[^1] = s[0];

        int hLen = ((outputLength - 1) / 8) + 1;
        
        var h = new byte[hLen];

        for (int i = 0; i < outputLength; i++)
        {
            byte b = (byte)((h64[i / 64] >> (i % 64)) & 1);
            h[i / 8] |= (byte)((int)b << (i % 8));
        }

        return h;
    }


    /// <summary>
    /// Chunks data into 128-bit blocks
    /// Flips input data ordering, see appendix A
    /// </summary>
    /// <param name="x">Arbitrary length input</param>
    /// <param name="bitLength"></param>
    /// <returns>Chunked data in 128-bit blocks</returns>
    private UInt128[] Parse128(byte[] x, int bitLength)
    {
        var parsedData = new UInt128[(int)System.Math.Ceiling(bitLength / 128.0)];
        var i = 0;
        var j = 0;
        var k = 0;

        while (i < x.Length)
        {
            parsedData[j] |= ((UInt128)x[i]) << k;
            k += 8;
            i++;

            if (i % 16 == 0)
            {
                j++;
                k = 0;
            }
        }

        if (bitLength % 128 == 0)
        {
            parsedData = parsedData.Append(new UInt128()).ToArray();
        }
        
        return parsedData;
    }

    /// <summary>
    /// Chunks data into 64-bit blocks
    /// Flips input data ordering, see appendix A
    /// </summary>
    /// <param name="x"></param>
    /// <param name="bitLength"></param>
    /// <returns>>Chunked data in 64-bit blocks</returns>
    private UInt64[] Parse64(byte[] x, int bitLength)
    {
        var parsedData = new UInt64[(int)System.Math.Ceiling(bitLength / 64.0)];
        var i = 0;
        var j = 0;
        var k = 0;
        
        while (i < x.Length)
        {
            parsedData[j] |= (UInt64)x[i] << k;
            k += 8;
            i++;
            
            if (i % 8 == 0)
            {
                j++;
                k = 0;
            }
        }

        if (bitLength % 64 == 0)
        {
            parsedData = parsedData.Append(0UL).ToArray();
        }
        
        return parsedData;
    }

    /// <summary>
    /// Pads short block to 128 bits
    /// </summary>
    /// <param name="x"></param>
    /// <param name="bitLength"></param>
    /// <returns></returns>
    private UInt128 Pad128(UInt128 x, int bitLength)
    {
        // Apply 1 before all bits in x
        return ((UInt128)1 << bitLength) | x;

        // 0s are default so no action needed on remainder
    }

    /// <summary>
    /// Pads short block to 64 bits
    /// </summary>
    /// <param name="x"></param>
    /// <param name="bitLength"></param>
    /// <returns></returns>
    private UInt64 Pad64(UInt64 x, int bitLength)
    {
        // Apply 1 before all bits in x
        return ((UInt64)1 << bitLength) | x;

        // 0s are default so no action needed on remainder
    }

    /// <summary>
    /// Apply the Ascon permutation to state s, pl -> ps -> pc
    /// </summary>
    /// <param name="s">320-bit state</param>
    /// <param name="rounds">Number of permutation rounds to perform</param>
    private void Permute(UInt64[] s, int rounds)
    {
        for (var i = 0; i < rounds; i++)
        {
            Pc(s, i, rounds);
            Ps(s);
            Pl(s);
        }
    }

    /// <summary>
    /// Linear Diffusion Layer
    /// </summary>
    /// <param name="s">320-bit state</param>
    private void Pl(UInt64[] s)
    {
        s[0] ^= ((s[0] << 45) | (s[0] >> 19)) ^ ((s[0] << 36) | (s[0] >> 28));
        s[1] ^= ((s[1] <<  3) | (s[1] >> 61)) ^ ((s[1] << 25) | (s[1] >> 39));
        s[2] ^= ((s[2] << 63) | (s[2] >>  1)) ^ ((s[2] << 58) | (s[2] >>  6));
        s[3] ^= ((s[3] << 54) | (s[3] >> 10)) ^ ((s[3] << 47) | (s[3] >> 17));
        s[4] ^= ((s[4] << 57) | (s[4] >>  7)) ^ ((s[4] << 23) | (s[4] >> 41));
    }

    /// <summary>
    /// Substitution layer
    /// </summary>
    /// <param name="s">320-bit state</param>
    private void Ps(UInt64[] s)
    {
        for (var i = 0; i < 64; i++)
        {
            int index = (int) (((s[0] >> i & 0x01) << 4) |
                               ((s[1] >> i & 0x01) << 3) |
                               ((s[2] >> i & 0x01) << 2) |
                               ((s[3] >> i & 0x01) << 1) |
                               ((s[4] >> i & 0x01) << 0));
            
            s[0] = (s[0] & ~(((UInt64)1) << i)) | ((UInt64)((_sBox[index] >> 4) & 0x01) << i);
            s[1] = (s[1] & ~(((UInt64)1) << i)) | ((UInt64)((_sBox[index] >> 3) & 0x01) << i);
            s[2] = (s[2] & ~(((UInt64)1) << i)) | ((UInt64)((_sBox[index] >> 2) & 0x01) << i);
            s[3] = (s[3] & ~(((UInt64)1) << i)) | ((UInt64)((_sBox[index] >> 1) & 0x01) << i);
            s[4] = (s[4] & ~(((UInt64)1) << i)) | ((UInt64)((_sBox[index] >> 0) & 0x01) << i);
        }
    }

    /// <summary>
    /// Constant addition layer
    /// </summary>
    /// <param name="s">320-bit state</param>
    /// <param name="round"></param>
    /// <param name="maxRounds"></param>
    private void Pc(UInt64[] s, int round, int maxRounds)
    {
        s[2] ^= _c[16 - maxRounds + round];
    }
}
