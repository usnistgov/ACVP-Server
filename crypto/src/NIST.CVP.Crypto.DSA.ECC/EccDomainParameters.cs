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
        /// How secrets are generated used by these <see cref="EccDomainParameters"/>
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