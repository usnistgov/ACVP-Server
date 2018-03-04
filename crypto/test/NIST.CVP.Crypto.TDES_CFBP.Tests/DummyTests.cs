using System;
using System.Collections.Generic;
using NIST.CVP.Common;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_CFBP.Tests
{
    public class DummyTests
    {
        [Test]
        [TestCase(
            AlgoMode.TDES_CFBP64, // _algo, 
            "010101010101010101010101010101010101010101010101", // _key, 
            "8000000000000000", // _iv, 
            "95f8a5e5dd31d900",  // _data, 
            TestName = "dummy1"
        )]
        public void ShouldEncryptSuccessfully(AlgoMode _algo, string _key, string _iv, string _data)
        {
            var mode = ModeFactory.GetMode(_algo);
            var key = new BitString(_key);
            var iv = new BitString(_iv);
            var data = new BitString(_data);

            var result = mode.BlockEncrypt(key, iv, data);
            Assert.IsTrue(result.Success);
        }
    }
}
