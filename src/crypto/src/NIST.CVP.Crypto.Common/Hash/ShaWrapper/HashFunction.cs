using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Hash.ShaWrapper
{
    public class HashFunction
    {
        public ModeValues Mode { get; }
        public DigestSizes DigestSize { get; }

        [JsonIgnore]
        public int OutputLen { get; }

        [JsonIgnore]
        public int BlockSize { get; }

        [JsonIgnore]
        public BigInteger MaxMessageLen { get; }

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
            Name = attributes.name;
        }
    }

    public enum ModeValues
    {
        SHA1,
        SHA2,
        SHA3,
        SHAKE
    }

    public enum DigestSizes
    {
        d128,
        d160,
        d224,
        d256,
        d384,
        d512,
        d512t224,
        d512t256,
        NONE
    }
}