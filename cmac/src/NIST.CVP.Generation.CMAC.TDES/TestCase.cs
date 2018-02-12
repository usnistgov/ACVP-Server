using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestCase : TestCaseBase
    {
        public TestCase() { }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public BitString Key1
        {
            get => Key?.MSBSubstring(0, 64);
            set => Key = Key != null ? value.ConcatenateBits(Key.Substring(64, 128)) : null;
        }

        public BitString Key2
        {
            get => Key?.MSBSubstring(64, 64);
            set => Key = Key?.Substring(0, 64).ConcatenateBits(value).ConcatenateBits(Key.Substring(128, 64));
        }
        public BitString Key3
        {
            get => Key?.MSBSubstring(128, 64);
            set => Key = Key?.Substring(0, 128).ConcatenateBits(value);
        }

        public override bool SetString(string name, string value)


        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "k":
                case "key":
                case "keys":
                    Key = new BitString(value);
                    return true;

                case "key1":
                    Key1 = new BitString(value);
                    return true;

                case "key2":
                    Key2 = new BitString(value);
                    return true;

                case "key3":
                    Key3 = new BitString(value);
                    return true;
                case "msg":
                    Message = new BitString(value);
                    return true;
                case "mac":
                    Mac = new BitString(value);
                    return true;
            }
            return false;
        }

        protected override void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ExpandoObject expandoSource = (ExpandoObject)source;

            if (expandoSource.ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }
            if (expandoSource.ContainsProperty("result"))
            {
                Result = source.result;
            }

            Key = expandoSource.GetBitStringFromProperty("key");
            Message = expandoSource.GetBitStringFromProperty("msg");
            Mac = expandoSource.GetBitStringFromProperty("mac");
        }
    }
}
