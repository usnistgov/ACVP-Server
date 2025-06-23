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
using Microsoft.AspNetCore.DataProtection.KeyManagement;


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
    [TestCase("B42A7BA29738AA5FBC09F4D21513C6D3", "8d86e3fcfcaa055f71063e3bd0f10fc1", "677C6FF6D7C23D32AB29973ACEA37094BFA1B5CC490CBF7C23AB2C4CA07B4398AD5B69BFCF109A9DC87851D34B4CBEC6E645EFCE72954D5ABCF3D32B95F1912FE5FF8C733AB1C5AF071704D0BBEC30D7EC65665A8C7426180E0F02DC3C6E08D6E621ED4CBF4A8C34803FC75A907C9F50", "6A56DAAF70286196", 64, "38B7CBFF6716228BF53CAE6EFAF76C608C91EBEB26753481C9F2921C1BBDF9CE41DE66FD7E072CE726FE6F7F06E243A32D75D982B6BCB1756BBF05D094266FA50F14A84E2A9DBF2F43CCCB7BAF818B39472E918F8665F2CDB61CAD2C8BDDD8B0D29C92E856CAD99ECA084FBB3C876C2B", 895, "C3049B1FE260F8EC", 64, "9872E8AF2B6574D870A3368B2F3FFBC2")]
    public void ShouldDecryptBitOrientedCorrectlyWithMasking(string keyString, string nonceString, string plaintextString, string adString, int adLen, string ciphertextString, int ctLen, string tagString, int tagLen, string secondKeyString)
    {
        byte[] key = StringToHexBytes(keyString);
        byte[] nonce = StringToHexBytes(nonceString);
        byte[] ad = StringToHexBytes(adString);
        byte[] ciphertext = StringToHexBytes(ciphertextString);
        byte[] tag = StringToHexBytes(tagString);
        byte[] secondKey = StringToHexBytes(secondKeyString);
        byte[] nonceCopy = StringToHexBytes(nonceString);
        var decryptResult = ascon.Aead128Decrypt(key, nonce, ad, ciphertext, tag, ctLen, adLen, tagLen);
        var decryptResult2 = ascon.Aead128Decrypt(key, nonce, ad, ciphertext, tag, ctLen, adLen, tagLen, secondKey);
    }

    [Test]
    //1
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C2782", 32)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278200", 33)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278200", 34)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278200", 35)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278210", 36)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278211BEC931", 64)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278211BEC93100", 65)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "", "", "", "4F9C278211BEC93140", 66)]
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
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "FF", 8, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0001", 9, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0003", 10, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0007", 11, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "000F", 12, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "001F", 13, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "003F", 14, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "007F", 15, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF01", 121, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", 128, "", 0, 128)]

    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "00", 8, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 9, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 10, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 11, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 12, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 13, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 14, "", 0, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "0000", 15, "", 0, 128)]
    public void ShouldEncryptDecryptCorrectly(string keyString, string nonceString, string plaintextString, int ptLen, string adString, int adLen, int tagLength)
    {
        byte[] key = StringToHexBytes(keyString);
        byte[] nonce = StringToHexBytes(nonceString);
        byte[] ad = StringToHexBytes(adString);
        byte[] plaintext = StringToHexBytes(plaintextString);
        int adBitLength = adLen;

        (byte[] c, byte[] tag) encryptReturn = ascon.Aead128Encrypt(key, nonce, ad, plaintext, ptLen, adBitLength, tagLength);
        var decryptResult = ascon.Aead128Decrypt(key, nonce, ad, encryptReturn.c, encryptReturn.tag, ptLen, adBitLength, tagLength);

        Assert.That(decryptResult.HasResult, Is.True);
        Assert.That(plaintext, Is.EqualTo(decryptResult.Result));
    }

    [Test, Ignore("Debugging only")]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", 256, "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", 256, 128)]
    [TestCase("000102030405060708090A0B0C0D0E0F", "101112131415161718191A1B1C1D1E1F", "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F01", 257, "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F01", 257, 128)]
    public void GenerateIntermediateValuesEncryptDecrypt(string keyString, string nonceString, string plaintextString, int ptLen, string adString, int adLen, int tagLen)
    {
        byte[] key = StringToHexBytes(keyString);
        byte[] nonce = StringToHexBytes(nonceString);
        byte[] ad = StringToHexBytes(adString);
        byte[] plaintext = StringToHexBytes(plaintextString);
        int adBitLength = adLen;

        Console.WriteLine("Ascon AEAD128 encryption\n");

        Console.WriteLine("key = " + keyString);
        Console.WriteLine("nonce = " + nonceString);
        Console.WriteLine("associatedData = " + adString);
        Console.WriteLine("plaintext = " + plaintextString);
        Console.WriteLine("plaintextLen = " + ptLen);
        Console.WriteLine("associatedDataLen = " + adLen);
        Console.WriteLine("tagLen = " + tagLen + "\n");

        (byte[] c, byte[] tag) encryptReturn = ascon.Aead128Encrypt(key, nonce, ad, plaintext, ptLen, adBitLength, tagLen);

        string ciphertextString = "";
        string tagString = "";
        for (int i = 0; i < encryptReturn.c.Length; i++)
        {
            ciphertextString += encryptReturn.c[i].ToString("X2");
        }
        for (int i = 0; i < encryptReturn.tag.Length; i++)
        {
            tagString += encryptReturn.tag[i].ToString("X2");
        }
        Console.WriteLine("\nciphertext = " + ciphertextString);
        Console.WriteLine("tag = " + tagString + "\n\n\n");



        Console.WriteLine("Ascon AEAD128 decryption\n");

        Console.WriteLine("key = " + keyString);
        Console.WriteLine("nonce = " + nonceString);
        Console.WriteLine("associatedData = " + adString);
        Console.WriteLine("ciphertext = " + ciphertextString);
        Console.WriteLine("tag = " + tagString);
        Console.WriteLine("plaintextLen = " + ptLen);
        Console.WriteLine("associatedDataLen = " + adLen);
        Console.WriteLine("tagLen = " + tagLen + "\n");

        var decryptResult = ascon.Aead128Decrypt(key, nonce, ad, encryptReturn.c, encryptReturn.tag, ptLen, adBitLength, tagLen);

        Console.WriteLine("\nplaintext = " + plaintextString);

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
