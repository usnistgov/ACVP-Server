using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Ascon.Tests.SP800_232;

[TestFixture]
[FastCryptoTest]
public class FirehoseTests
{
    private readonly Ascon _ascon = new();

    [Test]
    public void ShouldHashCorrectly()
    {
        string testPath = Utilities.GetConsistentTestingStartPath(GetType(), @".\SP800_232\KATFiles\LWC_HASH_KAT_128_256.txt");
        StreamReader sr = File.OpenText(testPath);
        
        for (int i = 0; i < 1025; i++)
        {
            var s = sr.ReadLine()!;
            int count = int.Parse(s[(s.IndexOf('=') + 2)..]);

            byte[] bytes = BuildHexBytes(sr);

            var ret = _ascon.Hash256(bytes, bytes.Length * 8);
            string output = BuildHexString(ret);
            
            s = sr.ReadLine()!;
            string digest = s[(s.IndexOf('=') + 2)..];
            
            Console.WriteLine("Hash " + count);
            Console.WriteLine("Output: " + output);
            Console.WriteLine("Target: " + digest + "\n");
            
            Assert.That(output, Is.EqualTo(digest), "Failure on Hash " + count);
            
            sr.ReadLine();
        }
    }

    [Test]
    public void ShouldXOFCorrectly()
    {
        string testPath = Utilities.GetConsistentTestingStartPath(GetType(), @".\SP800_232\KATFiles\LWC_XOF_KAT_128_512.txt");
        StreamReader sr = File.OpenText(testPath);
        
        for (int i = 0; i < 1025; i++)
        {
            var s = sr.ReadLine()!;
            int count = int.Parse(s.Substring(s.IndexOf('=') + 2));

            byte[] bytes = BuildHexBytes(sr);
            
            var ret = _ascon.Xof128(bytes, bytes.Length * 8, 512);

            string output = BuildHexString(ret);
            
            s = sr.ReadLine()!;
            string digest = s[(s.IndexOf('=') + 2)..];
            
            Console.WriteLine("XOF " + count);
            Console.WriteLine("Output: " + output);
            Console.WriteLine("Target: " + digest + "\n");
            
            Assert.That(output, Is.EqualTo(digest), "Failure on XOF " + count);

            sr.ReadLine();
        }
    }

    [Test]
    public void ShouldCXOFCorrectly()
    {
        string testPath = Utilities.GetConsistentTestingStartPath(GetType(), @".\SP800_232\KATFiles\LWC_CXOF_KAT_128_512.txt");
        StreamReader sr = File.OpenText(testPath);

        for (int i = 0; i < 1089; i++)
        {
            var s = sr.ReadLine()!;
           
            int count = int.Parse(s[(s.IndexOf('=') + 2)..]);

            byte[] bytes = BuildHexBytes(sr);
            byte[] csBytes = BuildHexBytes(sr);
           
            var ret = _ascon.CXof128(bytes, bytes.Length * 8, csBytes, csBytes.Length * 8, 512);
            
            string output = BuildHexString(ret);
            
            s = sr.ReadLine()!;
            
            string digest = s[(s.IndexOf('=') + 2)..];
            
            Console.WriteLine("CXOF " + count);
            Console.WriteLine("Output: " + output);
            Console.WriteLine("Target: " + digest + "\n");
            
            Assert.That(output, Is.EqualTo(digest), "Failure on CXOF " + count);
            
            sr.ReadLine();
        }
    }

    [Test]
    public void ShouldAEADCorrectly()
    {
        string testPath = Utilities.GetConsistentTestingStartPath(GetType(), @".\SP800_232\KATFiles\LWC_AEAD_KAT_128_128.txt");
        StreamReader sr = File.OpenText(testPath);
        
        for (int i = 0; i < 1089; i++)
        {
            var s = sr.ReadLine()!;
            int count = int.Parse(s[(s.IndexOf('=') + 2)..]);

            byte[] key = BuildHexBytes(sr);
            byte[] nonce = BuildHexBytes(sr);
            byte[] pt = BuildHexBytes(sr);
            byte[] ad = BuildHexBytes(sr);

            var ret = _ascon.Aead128Encrypt(key, nonce, ad, pt, pt.Length * 8, ad.Length * 8);
            
            string output = BuildHexString(ret.c);
            foreach (var t in ret.tag)
            {
                output += t.ToString("X2");
            }

            var ret2 = _ascon.Aead128Decrypt(key, nonce, ad, ret.c, ret.tag, ret.c.Length * 8, ad.Length * 8);
            
            string output2 = BuildHexString(ret2.Result);
            
            s = sr.ReadLine()!;
            string digest = s[(s.IndexOf('=') + 2)..];

            string ptString = BuildHexString(pt);

            Console.WriteLine("AEAD " + count);
            Console.WriteLine("Encrypt Output: " + output);
            Console.WriteLine("        Target: " + digest);
            Console.WriteLine("Decrypt Output: " + output2);
            Console.WriteLine("        Target: " + ptString + "\n");
            
            Assert.That(output, Is.EqualTo(digest), "Failure on Encryption " + count);
            
            Assert.That(ret2.HasResult, Is.True, "Failure on Decryption " + count);
            Assert.That(ret2.Result, Is.EqualTo(pt), "Failure on Decryption " + count);
            
            sr.ReadLine();
        }
    }

    private byte[] BuildHexBytes(StreamReader sr)
    {
        string s = sr.ReadLine()!;
        string inputLine = s[(s.IndexOf('=') + 2)..];
        byte[] output = new byte[inputLine.Length / 2];
        for (int j = 0; j < inputLine.Length; j = j + 2)
        {
            int num = Convert.ToInt32(inputLine.Substring(j, 2), 16);
            output[j / 2] = (byte)num;
        }
        
        return output;
    }

    private string BuildHexString(byte[] input)
    {
        string output = "";
        foreach (var t in input)
        {
            output += t.ToString("X2");
        }
        
        return output;
    }
}
