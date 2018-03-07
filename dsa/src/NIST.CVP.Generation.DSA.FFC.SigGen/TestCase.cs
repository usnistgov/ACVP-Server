using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public ITestGroup Parent { get; set; }

        public FfcKeyPair Key { get; set; }
        public BitString Message { get; set; }
        public FfcSignature Signature { get; set; }

        // Needed for FireHoseTests
        public BigInteger K;

        // Needed for SetString, FireHoseTests
        private BigInteger _xSetString;
        private BigInteger _ySetString;
        private BigInteger _rSetString;
        private BigInteger _sSetString;

        public TestCase() { }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            var expandoSource = (ExpandoObject) source;

            Message = expandoSource.GetBitStringFromProperty("message");

            BigInteger x, y;
            x = expandoSource.GetBigIntegerFromProperty("x");
            y = expandoSource.GetBigIntegerFromProperty("y");
            Key = new FfcKeyPair(x, y);
            
            BigInteger r, s;
            r = expandoSource.GetBigIntegerFromProperty("r");
            s = expandoSource.GetBigIntegerFromProperty("s");
            Signature = new FfcSignature(r, s);
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "x":
                    _xSetString = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "y":
                    _ySetString = new BitString(value).ToPositiveBigInteger();
                    Key = new FfcKeyPair(_xSetString, _ySetString);
                    return true;

                case "msg":
                    Message = new BitString(value);
                    return true;

                case "r":
                    _rSetString = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    _sSetString = new BitString(value).ToPositiveBigInteger();
                    Signature = new FfcSignature(_rSetString, _sSetString);
                    return true;

                case "k":
                    K = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
        }
    }
}
