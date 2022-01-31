using System;
using System.Numerics;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper
{
    public class HashFunction
    {
        public ModeValues Mode { get; }
        public DigestSizes DigestSize { get; }

        /// <summary>
        /// Output length in bits.
        /// </summary>
        [JsonIgnore]
        public int OutputLen { get; }

        [JsonIgnore]
        public int BlockSize { get; }

        [JsonIgnore]
        public BigInteger MaxMessageLen { get; }

        [JsonIgnore]
        public int ProcessingLen { get; }

        [JsonIgnore]
        public string Name { get; }

        public HashFunction(ModeValues mode, DigestSizes digestSize)
        {
            Mode = mode;
            DigestSize = digestSize;

            var attributes = ShaAttributes.GetShaAttributes(mode, digestSize);
            OutputLen = attributes.outputLen;
            BlockSize = attributes.blockSize;
            MaxMessageLen = attributes.maxMessageSize;
            ProcessingLen = attributes.processingLen;
            Name = attributes.name;
        }

        public override bool Equals(object other)
        {
            if (other is HashFunction obj)
            {
                return GetHashCode() == obj.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Mode, DigestSize);
    }

    public enum ModeValues
    {
        [EnumMember(Value = "SHA-1")]
        SHA1,
        [EnumMember(Value = "SHA2")]
        SHA2,
        [EnumMember(Value = "SHA3")]
        SHA3,
        [EnumMember(Value = "SHAKE")]
        SHAKE
    }

    public enum DigestSizes
    {
        [EnumMember(Value = "128")]
        d128,
        [EnumMember(Value = "160")]
        d160,
        [EnumMember(Value = "224")]
        d224,
        [EnumMember(Value = "256")]
        d256,
        [EnumMember(Value = "384")]
        d384,
        [EnumMember(Value = "512")]
        d512,
        [EnumMember(Value = "512/224")]
        d512t224,
        [EnumMember(Value = "512/256")]
        d512t256,
        [EnumMember(Value = "NONE")]
        NONE
    }
}
