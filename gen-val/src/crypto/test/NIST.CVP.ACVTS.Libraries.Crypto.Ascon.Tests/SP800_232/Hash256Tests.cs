using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using NIST.CVP.ACVTS.Libraries.Crypto.Ascon;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using static NIST.CVP.ACVTS.Libraries.Crypto.Ascon.Ascon;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Ascon.Tests.SP800_232;

[TestFixture]
[FastCryptoTest]
public class Hash256Tests
{
    private Crypto.Ascon.Ascon ascon = new Crypto.Ascon.Ascon();

    [Test]
    //1
    [TestCase("", "0B3BE5850F2F6B98CAF29F8FDEA89B64A1FA70AA249B8F839BD53BAA304D92B2")]
    //2
    [TestCase("00", "0728621035AF3ED2BCA03BF6FDE900F9456F5330E4B5EE23E7F6A1E70291BC80")]
    //10
    [TestCase("000102030405060708", "94269C30E0296E1EC86655041841823EFA1927F520FD58C8E9BCE6197878C1A6")]
    //87
    [TestCase("000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D3E3F404142434445464748494A4B4C4D4E4F505152535455", "5629F63E46BE5470DD6682EA8E0A3D942DE273BCAEDD8084F2EE9212B29053E2")]
    public void ShouldHashByteOrientedCorrectly(string messageString, string digestString)
    {
        byte[] message = StringToHexBytes(messageString);
        int messageBitLength = message.Length * 8;

        byte[] digestBytes = ascon.Hash256(message, messageBitLength);

        string digest = "";

        for (int i = 0; i < digestBytes.Length; i++)
        {
            digest = digest + digestBytes[i].ToString("X2");
        }

        Assert.That(digest, Is.EqualTo(digestString));
    }

    [Test]
    [TestCase("01", "74230844926CB943951AF83596F8450EDA360A7958B37823DF8F74BCD89EB592", 1)]
    [TestCase("05", "E6A8975A7A19F527A5C86E6860DC46463196E9C10E40A796A30CFABE67759336", 3)]
    [TestCase("0001", "251A4DC11BF530DD1F2D6D29B0BB8DDC26F5479D0C0AE840AA26E24D419C9B75", 9)]
    [TestCase("000102030405060701", "64627A7C81AE4C17592FC0A9628FC0DAEB4404DA27DAC0F58575FD9FB49FEE24", 65)]
    public void ShouldHashBitOrientedCorrectly(string messageString, string digestString, int messageBitLength)
    {
        byte[] message = StringToHexBytes(messageString);

        byte[] digestBytes = ascon.Hash256(message, messageBitLength);

        string digest = "";

        for (int i = 0; i < digestBytes.Length; i++)
        {
            digest = digest + digestBytes[i].ToString("X2");
        }

        Assert.That(digest, Is.EqualTo(digestString));
    }

    [Test]
    public void ShouldHashAllValuesNoAnswers()
    {
        for(int i = 1; i <= 128; i++)
        {
            var m = new BitArray(i, true);
            ascon.Hash256(m.ToBytes(), i);
        }
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

