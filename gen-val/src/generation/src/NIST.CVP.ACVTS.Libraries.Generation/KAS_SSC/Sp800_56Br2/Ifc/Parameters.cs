using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        /// <summary>
        /// The KAS schemes to test
        /// </summary>
        public Schemes Scheme { get; set; }

        /// <summary>
        /// The key generation methods to test within this scheme.
        /// </summary>
        public IfcKeyGenerationMethod[] KeyGenerationMethods { get; set; }

        /// <summary>
        /// The common modulo lengths supported.
        /// </summary>
        public int[] Modulo { get; set; }

        /// <summary>
        /// The Fixed public exponent used for certain key generation methods
        /// </summary>
        [JsonProperty("fixedPubExp")]
        public BigInteger FixedPublicExponent { get; set; }

        /// <summary>
        /// The hash function to apply to "Z", if not supplied return Z in the clear.
        /// </summary>
        public HashFunctions HashFunctionZ { get; set; }
    }

    public class Schemes
    {
        [JsonProperty(PropertyName = "KAS1")]
        public Kas1 Kas1 { get; set; }
        [JsonProperty(PropertyName = "KAS2")]
        public Kas2 Kas2 { get; set; }

        public IEnumerable<SchemeBase> GetRegisteredSchemes()
        {
            var list = new List<SchemeBase>();

            list.AddIfNotNull(Kas1);
            list.AddIfNotNull(Kas2);

            return list;
        }
    }

    public abstract class SchemeBase
    {
        /// <summary>
        /// The enum scheme type
        /// </summary>
        public abstract SscIfcScheme Scheme { get; }
        /// <summary>
        /// Additional operations the scheme supports.
        /// </summary>
        public abstract KasMode KasMode { get; }
        /// <summary>
        /// The Key Agreement Role (initiator or responder)
        /// </summary>
        public KeyAgreementRole[] KasRole { get; set; }
    }

    public class Kas1 : SchemeBase
    {
        public override SscIfcScheme Scheme => SscIfcScheme.Kas1;
        public override KasMode KasMode => KasMode.NoKdfNoKc;
    }

    public class Kas2 : SchemeBase
    {
        public override SscIfcScheme Scheme => SscIfcScheme.Kas2;
        public override KasMode KasMode => KasMode.NoKdfNoKc;
    }
}
