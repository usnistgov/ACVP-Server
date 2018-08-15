using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.Common.Hash.SHA2;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// The polynomial representing the curve, contains a, b and the curve type
        /// </summary>
        public IEdwardsCurve CurveE { get; }

        public Hash.SHA2.HashFunction SHA2Hash
        {
            get
            {
                if (CurveE.CurveName == Curve.Ed25519)
                {
                    return new Hash.SHA2.HashFunction(ModeValues.SHA2, DigestSizes.d512);
                }
                else
                {
                    return null;
                }
                
            }
        }

        public Hash.SHA3.HashFunction SHA3Hash
        {
            get
            {
                if (CurveE.CurveName == Curve.Ed25519)
                {
                    return new Hash.SHA3.HashFunction();    // this should never happen
                }
                else
                {
                    return new Hash.SHA3.HashFunction(512, 512, true);
                }
            }
        }

        /// <summary>
        /// How secrets are generated used by these <see cref="EdDomainParameters"/>.
        /// Generally this field doesn't matter but is needed for some group properties in gen/vals.
        /// Theoretically it should factor into KeyGen, but our tests are agnostic to this property.
        /// </summary>
        public SecretGenerationMode SecretGeneration { get; }

        public EdDomainParameters(IEdwardsCurve e)
        {
            CurveE = e;
        }

        public EdDomainParameters(IEdwardsCurve e, SecretGenerationMode secretMode)
        {
            CurveE = e;
            SecretGeneration = secretMode;
        }
    }
}