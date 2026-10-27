using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Tests.Native
{
    /// <summary>
    /// Deterministic test vectors generated with the RFC 8391 reference implementation
    /// (https://github.com/XMSS/xmss-reference), test/vectors.c seed schedule: the key pair is
    /// derived from seed bytes 0, 1, 2, ..., 3n-1 and messages are signed at fixed leaf
    /// indices. Each TestVectors/*.jsonl line carries the full seed, public key, and
    /// signed messages of one parameter set, plus 10-byte shake128 fingerprints matching the
    /// output of the reference's own test/vectors.c harness.
    /// </summary>
    public class XmssReferenceVector
    {
        public int Oid { get; set; }
        public int N { get; set; }
        public int FullHeight { get; set; }
        public string Seed { get; set; }
        public string Pk { get; set; }
        public List<XmssReferenceSignature> Sigs { get; set; }

        [JsonIgnore]
        public XmssMode Mode => AttributesHelper.GetXmssModeFromTypeCode(Oid.GetBytes());
    }

    public class XmssReferenceSignature
    {
        public long Idx { get; set; }
        public string Msg { get; set; }
        public string Sm { get; set; }
    }

    /// <summary>
    /// One WOTS+ vector from the same harness: key generation, signing and leaf computation
    /// with fully specified seeds and hash addresses.
    /// </summary>
    public class WotsReferenceVector
    {
        public int Oid { get; set; }
        public string SkSeed { get; set; }
        public string PubSeed { get; set; }
        public string M { get; set; }
        public uint[] Addr { get; set; }
        public uint[] Addr2 { get; set; }
        public string Pk { get; set; }
        public string Sig { get; set; }
        public string Leaf { get; set; }

        [JsonIgnore]
        public XmssMode Mode => AttributesHelper.GetXmssModeFromTypeCode(Oid.GetBytes());
    }

    public static class ReferenceVectors
    {
        public static List<XmssReferenceVector> LoadXmss(string file)
        {
            return File.ReadAllLines(TestVectorPath(file))
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(JsonConvert.DeserializeObject<XmssReferenceVector>)
                .ToList();
        }

        public static List<WotsReferenceVector> LoadWots(string file)
        {
            return File.ReadAllLines(TestVectorPath(file))
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(JsonConvert.DeserializeObject<WotsReferenceVector>)
                .ToList();
        }

        public static string TestVectorPath(string file)
        {
            return Path.Combine(
                Utilities.GetConsistentTestingStartPath(typeof(ReferenceVectors), @"TestVectors\"), file);
        }

        /// <summary>
        /// The reference emits the core public key root || SEED; the wire format prepends the OID.
        /// </summary>
        public static byte[] PublicKeyFromVector(XmssReferenceVector vector)
        {
            var attribute = AttributesHelper.GetXmssAttribute(vector.Mode);
            var publicKey = new byte[4 + 2 * attribute.N];
            Array.Copy(attribute.NumericIdentifier, 0, publicKey, 0, 4);
            Array.Copy(new BitString(vector.Pk).ToBytes(), 0, publicKey, 4, 2 * attribute.N);
            return publicKey;
        }

        /// <summary>
        /// The reference emits signed messages sm = signature || message.
        /// </summary>
        public static (byte[] signature, byte[] message) SignatureAndMessageFromVector(
            XmssReferenceVector vector, XmssReferenceSignature sig)
        {
            var attribute = AttributesHelper.GetXmssAttribute(vector.Mode);
            var signatureLength = 4 + attribute.N * (1 + attribute.Len + attribute.H);
            var sm = new BitString(sig.Sm).ToBytes();
            return (sm.Take(signatureLength).ToArray(), sm.Skip(signatureLength).ToArray());
        }
    }
}
