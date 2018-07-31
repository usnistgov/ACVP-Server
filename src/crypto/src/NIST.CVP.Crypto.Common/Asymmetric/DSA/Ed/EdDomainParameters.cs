using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// The polynomial representing the curve, contains a, b and the curve type
        /// </summary>
        public IEdwardsCurve CurveE { get; }

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