﻿using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace NIST.CVP.Generation.DSA.v1_0.SigGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public int L { get; set; }
        public int N { get; set; }

        [JsonIgnore]
        public bool IsMessageRandomized => "SP800-106".Equals(Conformance, StringComparison.OrdinalIgnoreCase);

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Conformance { get; set; } = string.Empty;
        [JsonIgnore] public FfcDomainParameters DomainParams { get; set; } = new FfcDomainParameters();

        [JsonProperty(PropertyName = "p", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString P
        {
            get => DomainParams?.P != 0 ? new BitString(DomainParams.P, L) : null;
            set => DomainParams.P = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Q
        {
            get => DomainParams?.Q != 0 ? new BitString(DomainParams.Q, N) : null;
            set => DomainParams.Q = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "g", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString G
        {
            get => DomainParams?.G != 0 ? new BitString(DomainParams.G, L) : null;
            set => DomainParams.G = value.ToPositiveBigInteger();
        }

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        /// <summary>
        /// Ignoring for (De)Serialization as KeyPairs are flattened
        /// </summary>
        [JsonIgnore]
        public FfcKeyPair Key { get; set; } = new FfcKeyPair();

        [JsonProperty(PropertyName = "x", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString X
        {
            get => Key?.PrivateKeyX != 0 ? new BitString(Key.PrivateKeyX, N) : null;
            set => Key.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "y", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Y
        {
            get => Key?.PublicKeyY != 0 ? new BitString(Key.PublicKeyY, L) : null;
            set => Key.PublicKeyY = value.ToPositiveBigInteger();
        }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "p":
                    DomainParams.P = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "q":
                    DomainParams.Q = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "g":
                    DomainParams.G = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "n":
                    N = int.Parse(value);
                    return true;

                case "hashalg":
                    HashAlgName = value;
                    return true;
            }

            return false;
        }
    }
}