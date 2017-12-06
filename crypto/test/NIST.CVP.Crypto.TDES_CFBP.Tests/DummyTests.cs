using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES_CFBP;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_CFBP.Tests
{
    public class DummyTests
    {
        [Test]
        [TestCase(
            Algo.TDES_CFBP64, // _algo, 
            "010101010101010101010101010101010101010101010101", // _key, 
            "8000000000000000", // _iv, 
            "95f8a5e5dd31d900",  // _data, 
            TestName = "dummy1"
        )]
        public void ShouldEncryptSuccessfully(Algo _algo, string _key, string _iv, string _data)
        {
            var mode = ModeFactory.GetMode(_algo);
            var key = new BitString(_key);
            var iv = new BitString(_iv);
            var data = new BitString(_data);

            var result = (EncryptionResultWithIv)mode.BlockEncrypt(key, iv, data);
        }
    }
}
