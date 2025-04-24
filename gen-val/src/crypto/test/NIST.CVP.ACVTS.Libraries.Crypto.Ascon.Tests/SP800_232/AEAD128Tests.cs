using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Crypto.Ascon;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using static NIST.CVP.ACVTS.Libraries.Crypto.Ascon.Ascon;


namespace NIST.CVP.ACVTS.Libraries.Crypto.Ascon.Tests.SP800_232;

[TestFixture]
[FastCryptoTest]
public class AEAD128Tests
{
    private Crypto.Ascon.Ascon ascon = new Crypto.Ascon.Ascon();

    [Test]
    //1
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278211BEC9316BF68F46EE8B2EC6")]
    //2
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "30", "", "CCCB674FE18A09A285D6AB11B35675C0")]
    //10
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "303132333435363738", "", "24F13284A0F90F906B18C7E4061C0896")]
    //34
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "20", "", "E8", "DD576ABA1CD3E6FC704DE02AEDB79588")]
    //35
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "20", "30", "96", "2B8016836C75A7D86866588CA245D886")]
    //298
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "202122232425262728", "", "E8C3DEEE246CC5EAE3", "329BC950AE101B9247F3605EBEDCBF6C")]
    //1016
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D", "303132333435363738393A3B3C3D3E3F404142434445464748", "16CB13E5853541E2F36B38240366ACEE48E0B21D8A3424739FCD62AC2DDE", "B2D36A792BA208D4158CDF4E968FA99F")]
    public void ShouldEncryptByteOrientedCorrectly(string keyString, string nonceString, string plaintextString, string adString, string ciphertextString, string tagString)
    {
        byte[] key = StringToHexBytes(keyString);
        byte[] nonce = StringToHexBytes(nonceString);
        byte[] ad = StringToHexBytes(adString);
        byte[] plaintext = StringToHexBytes(plaintextString);
        int plaintextBitLength = plaintext.Length * 8;
        int adBitLength = ad.Length * 8;

        (byte[] c, byte[] tag) encryptReturn = ascon.Aead128Encrypt(key, nonce, ad, plaintext, plaintextBitLength, adBitLength);

        string ciphertext = "";
        string tag = "";

        for (int i = 0; i < encryptReturn.c.Length; i++)
        {
            ciphertext = ciphertext + encryptReturn.c[i].ToString("X2");
        }
        for (int i = 0; i < encryptReturn.tag.Length; i++)
        {
            tag = tag + encryptReturn.tag[i].ToString("X2");
        }

        Assert.That(ciphertext, Is.EqualTo(ciphertextString));
        Assert.That(tag, Is.EqualTo(tagString));
    }

    [Test]
    //1
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278211BEC9316BF68F46EE8B2EC6")]
    //2
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "30", "", "CCCB674FE18A09A285D6AB11B35675C0")]
    //10
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "303132333435363738", "", "24F13284A0F90F906B18C7E4061C0896")]
    //34
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "20", "", "E8", "DD576ABA1CD3E6FC704DE02AEDB79588")]
    //35
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "20", "30", "96", "2B8016836C75A7D86866588CA245D886")]
    //298
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "202122232425262728", "", "E8C3DEEE246CC5EAE3", "329BC950AE101B9247F3605EBEDCBF6C")]
    //1016
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D", "303132333435363738393A3B3C3D3E3F404142434445464748", "16CB13E5853541E2F36B38240366ACEE48E0B21D8A3424739FCD62AC2DDE", "B2D36A792BA208D4158CDF4E968FA99F")]
    public void ShouldDecryptByteOrientedCorrectly(string keyString, string nonceString, string plaintextString, string adString, string ciphertextString, string tagString)
    {
        byte[] key = StringToHexBytes(keyString);
        byte[] nonce = StringToHexBytes(nonceString);
        byte[] ad = StringToHexBytes(adString);
        byte[] ciphertext = StringToHexBytes(ciphertextString);
        byte[] tag = StringToHexBytes(tagString);
        int ciphertextBitLength = ciphertext.Length * 8;
        int adBitLength = ad.Length * 8;

        var decryptResult = ascon.Aead128Decrypt(key, nonce, ad, ciphertext, tag, ciphertextBitLength, adBitLength);

        string plaintext = "";

        for (int i = 0; i < decryptResult.Result.Length && decryptResult.HasResult; i++)
        {
            plaintext = plaintext + decryptResult.Result[i].ToString("X2");
        }

        Assert.That(plaintext, Is.EqualTo(plaintextString));
    }

    [Test]
    //1
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278211BEC931", 64)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278211BEC93100", 65)]
    //35
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "20", "30", "96", "2B8016836C75A7D8", 64)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "20", "30", "96", "2B8016836C75A7D860", 67)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "20", "30", "96", "2B8016836C75A7D860", 68)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "20", "30", "96", "2B8016836C75A7D868", 69)]
    //1016
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D", "303132333435363738393A3B3C3D3E3F404142434445464748", "16CB13E5853541E2F36B38240366ACEE48E0B21D8A3424739FCD62AC2DDE", "B2D36A792BA208D4", 64)]
    public void ShouldEncryptTagTruncationCorrectly(string keyString, string nonceString, string plaintextString, string adString, string ciphertextString, string tagString, int tagLength)
    {
        byte[] key = StringToHexBytes(keyString);
        byte[] nonce = StringToHexBytes(nonceString);
        byte[] ad = StringToHexBytes(adString);
        byte[] plaintext = StringToHexBytes(plaintextString);
        int plaintextBitLength = plaintext.Length * 8;
        int adBitLength = ad.Length * 8;

        (byte[] c, byte[] tag) encryptReturn = ascon.Aead128Encrypt(key, nonce, ad, plaintext, plaintextBitLength, adBitLength, tagLength);

        string ciphertext = "";
        string tag = "";
        
        for (int i = 0; i < encryptReturn.c.Length; i++)
        {
            ciphertext += encryptReturn.c[i].ToString("X2");
        }
        
        for (int i = 0; i < encryptReturn.tag.Length; i++)
        {
            tag += encryptReturn.tag[i].ToString("X2");
        }

        Assert.That(ciphertext, Is.EqualTo(ciphertextString));
        Assert.That(tag, Is.EqualTo(tagString));
    }

    [Test]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "FF", 8, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0001", 9, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0003", 10, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0007", 11, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "000F", 12, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "001F", 13, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "003F", 14, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "007F", 15, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF01", 121, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 128, "", 128)]
    
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "00", 8, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 9, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 10, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 11, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 12, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 13, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 14, "", 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 15, "", 128)]

    public void ShouldEncryptDecryptCorrectly(string keyString, string nonceString, string plaintextString, int ptLen, string adString, int tagLength)
    {
        byte[] key = StringToHexBytes(keyString);
        byte[] nonce = StringToHexBytes(nonceString);
        byte[] ad = StringToHexBytes(adString);
        byte[] plaintext = StringToHexBytes(plaintextString);
        int adBitLength = ad.Length * 8;

        (byte[] c, byte[] tag) encryptReturn = ascon.Aead128Encrypt(key, nonce, ad, plaintext, ptLen, adBitLength, tagLength);
        var decryptResult = ascon.Aead128Decrypt(key, nonce, ad, encryptReturn.c, encryptReturn.tag, ptLen, adBitLength, tagLength);

        Assert.That(decryptResult.HasResult, Is.True);
        Assert.That(plaintext, Is.EqualTo(decryptResult.Result));
    }

    [Test]
    public void ShouldEncryptDecryptCorrectlyWithVariablePT()
    {
        byte[] key = StringToHexBytes("000102030405060708090A0B0C0D0E0F");
        byte[] nonce = StringToHexBytes("101112131415161718191A1B1C1D1E1F");
        byte[] ad = [];

        var badLengths = new List<int>();
        
        for (var i = 1; i <= 256; i++)
        {
            var pt = new BitArray(i, true);
            
            (byte[] c, byte[] tag) encryptReturn = ascon.Aead128Encrypt(key, nonce, ad, pt.ToBytes(), pt.Length, 0, 128);
            var decryptResult = ascon.Aead128Decrypt(key, nonce, ad, encryptReturn.c, encryptReturn.tag, pt.Length, 0, 128);

            Assert.That(decryptResult.HasResult, Is.True, $"{i}");

            if (!decryptResult.Result.SequenceEqual(pt.ToBytes()))
            {
                badLengths.Add(i);
            }
            Assert.That(decryptResult.Result, Is.EqualTo(pt.ToBytes()));
        }
        
        Console.WriteLine($"{string.Join(",", badLengths)}");
    }

    [Test]
    public void ShouldEncryptDecryptCorrectlyWithVariableAD()
    {
        byte[] key = StringToHexBytes("000102030405060708090A0B0C0D0E0F");
        byte[] nonce = StringToHexBytes("101112131415161718191A1B1C1D1E1F");
        byte[] pt = [];

        var badLengths = new List<int>();

        for (var i = 1; i <= 256; i++)
        {
            var ad = new BitArray(i, true);

            (byte[] c, byte[] tag) encryptReturn = ascon.Aead128Encrypt(key, nonce, ad.ToBytes(), pt, 0, ad.Length, 128);
            var decryptResult = ascon.Aead128Decrypt(key, nonce, ad.ToBytes(), encryptReturn.c, encryptReturn.tag, 0, ad.Length, 128);

            Assert.That(decryptResult.HasResult, Is.True, $"{i}");

            if (!decryptResult.Result.SequenceEqual(pt))
            {
                badLengths.Add(i);
            }
            Assert.That(decryptResult.Result, Is.EqualTo(pt));
        }

        Console.WriteLine($"{string.Join(",", badLengths)}");
    }


    private byte[] StringToHexBytes(string input)
    {
        byte[] output = new byte[input.Length / 2];
        for (int i = 0; i < input.Length; i = i + 2)
        {
            int num = Convert.ToInt32(input.Substring(i, 2), 16);
            output[i / 2] = (byte)num;
        }
        return output;
    }
}
