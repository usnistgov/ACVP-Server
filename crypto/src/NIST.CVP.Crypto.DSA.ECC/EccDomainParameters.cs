using System.Numerics;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccDomainParameters : IDsaDomainParameters
    {
        /// <summary>
        /// The polynomial representing the curve, contains a, b and the curve type
        /// </summary>
        public IEccCurve CurveE { get; }

        /// <summary>
        /// How secrets are generated used by these <see cref="EccDomainParameters"/>.
        /// Generally this field doesn't matter but is needed for some group properties in gen/vals.
        /// Theoretically it should factor into KeyGen, but our tests are agnostic to this property.
        /// </summary>
        public SecretGenerationMode SecretGeneration { get; }

        public EccDomainParameters(IEccCurve e)
        {
            CurveE = e;
        }

        public EccDomainParameters(IEccCurve e, SecretGenerationMode secretMode)
        {
            CurveE = e;
            SecretGeneration = secretMode;
        }
    }
}