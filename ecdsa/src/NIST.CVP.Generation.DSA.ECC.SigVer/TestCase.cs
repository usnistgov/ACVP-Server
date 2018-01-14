using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.ECC.SigVer.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public bool Result { get; set; }
        public SigFailureReasons Reason { get; set; }
        public EccKeyPair KeyPair { get; set; }
        public BitString Message { get; set; }
        public EccSignature Signature { get; set; }

        private BigInteger _rSetString;
        private BigInteger _sSetString;
        private BigInteger _qxSetString;
        private BigInteger _qySetString;

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

            ExpandoObject expandoSource = (ExpandoObject)source;

            var resultValue = expandoSource.GetTypeFromProperty<string>("result");
            if (resultValue != null)
            {
                Result = resultValue.ToLower() == EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed);
            }

            var reasonValue = expandoSource.GetTypeFromProperty<string>("reason");
            if (reasonValue != null)
            {
                Reason = EnumHelpers.GetEnumFromEnumDescription<SigFailureReasons>(expandoSource.GetTypeFromProperty<string>("reason"));
            }
        }

        public bool Merge(ITestCase otherTest)
        {
            // We don't need any properties from the prompt...
            return (TestCaseId == otherTest.TestCaseId);
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (value.Length % 2 != 0)
            {
                value = "0" + value;
            }

            switch (name.ToLower())
            {
                case "msg":
                    Message = new BitString(value);
                    return true;

                case "qx":
                    _qxSetString = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qy":
                    _qySetString = new BitString(value).ToPositiveBigInteger();
                    KeyPair = new EccKeyPair(new EccPoint(_qxSetString, _qySetString));
                    return true;

                case "r":
                    _rSetString = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    _sSetString = new BitString(value).ToPositiveBigInteger();
                    Signature = new EccSignature(_rSetString, _sSetString);
                    return true;

                case "result":
                    Result = value.ToLower()[0] == 'p';
                    return true;
            }

            return false;
        }
    }
}
