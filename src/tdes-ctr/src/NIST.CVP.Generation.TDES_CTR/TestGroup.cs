﻿using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string Direction { get; set; }
        public int NumberOfKeys { get; set; }

        // Properties for specific groups
        [JsonIgnore]
        public MathDomain DataLength { get; set; }

        // This is a vectorset / IUT property but it needs to be defined somewhere other than Parameter.cs
        public bool IncrementalCounter { get; set; }
        public bool OverflowCounter { get; set; }

        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "testtype":
                    TestType = value;
                    return true;
            }

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                case "numberofkeys":
                    NumberOfKeys = intVal;
                    return true;
            }

            return false;
        }
    }
}