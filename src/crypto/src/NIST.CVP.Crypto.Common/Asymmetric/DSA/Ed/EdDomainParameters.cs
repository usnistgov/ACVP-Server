using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// The polynomial representing the curve, contains a, b and the curve type
        /// </summary>
        public IEdwardsCurve CurveE { get; }

        private IShaFactory _shaFactory;

        public ISha Hash
        {
            get
            {
                if (CurveE.CurveName == Curve.Ed25519)
                {
                    return _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512));
                }
                else
                {
                    return _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
                }
                
            }
        }

        /// <summary>
        /// How secrets are generated used by these <see cref="EdDomainParameters"/>.
        /// Generally this field doesn't matter but is needed for some group properties in gen/vals.
        /// Theoretically it should factor into KeyGen, but our tests are agnostic to this property.
        /// </summary>
        public SecretGenerationMode SecretGeneration { get; }

        public EdDomainParameters(IEdwardsCurve e, IShaFactory shaFactory)
        {
            CurveE = e;
            _shaFactory = shaFactory;
        }

        public EdDomainParameters(IEdwardsCurve e, IShaFactory shaFactory, SecretGenerationMode secretMode)
        {
            CurveE = e;
            SecretGeneration = secretMode;
            _shaFactory = shaFactory;
        }
    }
}