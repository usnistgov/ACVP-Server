using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase>
    {
        public override int KeyLength
        {
            get => 192;
            set { } //there must be a better way to do this
        }

        [JsonProperty(PropertyName = "keyingOption")]
        public int KeyingOption { get; set; }

        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                case "klen":
                    if (intVal == 3) KeyingOption = 1;
                    else if (intVal == 2) KeyingOption = 2;
                    else throw new ArgumentException($"Cannon parse klen {intVal} to KeyingOption");
                    return true;
                case "msglen":
                case "mlen":
                    MessageLength = intVal;
                    return true;
                case "maclen":
                case "tlen":
                    MacLength = intVal;
                    return true;
            }
            return false;
        }

    }
}
