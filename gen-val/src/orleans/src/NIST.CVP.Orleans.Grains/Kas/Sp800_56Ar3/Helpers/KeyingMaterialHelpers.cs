using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Builders;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar3.Helpers
{
    public static class KeyingMaterialHelpers
    {
        /// <summary>
        /// Hydrates the provided <see cref="ISecretKeyingMaterialBuilder"/> with all needed requirements of a kas scheme.
        /// </summary>
        /// <param name="builder">The builder to hydrate.</param>
        /// <param name="requirements">The requirements of the party for the scheme.</param>
        /// <param name="dp">The domain parameters.</param>
        /// <param name="ephemeralKey">The party ephemeral key.</param>
        /// <param name="staticKey">The party static key.</param>
        /// <param name="entropyProvider">The entropy provider.</param>
        /// <param name="partyId">The party ID.</param>
        public static void SetSecretKeyingMaterialBuilderInformation(
            ISecretKeyingMaterialBuilder builder, 
            SchemeKeyNonceGenRequirement requirements, 
            FfcDomainParameters dp, 
            IDsaKeyPair ephemeralKey,
            IDsaKeyPair staticKey,
            IEntropyProvider entropyProvider,
            BitString partyId)
        {
            builder
                .WithDomainParameters(dp)
                .WithEphemeralKey(ephemeralKey)
                .WithStaticKey(staticKey);

            if (requirements.GeneratesEphemeralNonce)
            {
                builder.WithEphemeralNonce(
                    entropyProvider.GetEntropy(dp.P.ExactBitString().PadToModulusMsb(32).BitLength));
            }

            if (requirements.GeneratesDkmNonce)
            {
                builder.WithDkmNonce(
                    entropyProvider.GetEntropy(dp.Q.ExactBitString().PadToModulusMsb(32).BitLength));
            }
            
            builder.WithPartyId(partyId);
        }
        
        /// <summary>
        /// Hydrates the provided <see cref="ISecretKeyingMaterialBuilder"/> with all needed requirements of a kas scheme.
        /// </summary>
        /// <param name="builder">The builder to hydrate.</param>
        /// <param name="requirements">The requirements of the party for the scheme.</param>
        /// <param name="dp">The domain parameters.</param>
        /// <param name="ephemeralKey">The party ephemeral key.</param>
        /// <param name="staticKey">The party static key.</param>
        /// <param name="entropyProvider">The entropy provider.</param>
        /// <param name="partyId">The party ID.</param>
        public static void SetSecretKeyingMaterialBuilderInformation(
            ISecretKeyingMaterialBuilder builder, 
            SchemeKeyNonceGenRequirement requirements, 
            EccDomainParameters dp, 
            IDsaKeyPair ephemeralKey,
            IDsaKeyPair staticKey,
            IEntropyProvider entropyProvider,
            BitString partyId)
        {
            var curveAttributes = CurveAttributesHelper.GetCurveAttribute(dp.CurveE.CurveName);

            builder
                .WithDomainParameters(dp)
                .WithEphemeralKey(ephemeralKey)
                .WithStaticKey(staticKey);
            
            if (requirements.GeneratesEphemeralNonce)
            {
                builder.WithEphemeralNonce(
                    entropyProvider.GetEntropy(curveAttributes.LengthN.ValueToMod(BitString.BITSINBYTE)));
            }

            if (requirements.GeneratesDkmNonce)
            {
                builder.WithDkmNonce(
                    entropyProvider.GetEntropy(curveAttributes.LengthN.ValueToMod(BitString.BITSINBYTE)));
            }

            builder.WithPartyId(partyId);
        }
    }
}