using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public class HssKeyPairFactory : IHssKeyPairFactory
    {
        private readonly ILmsKeyPairFactory _lmsKeyPairFactory;
        private readonly ILmsSigner _lmsSigner;
        private readonly ISeedIdRotator _seedIdRotator;
        private readonly IShaFactory _shaFactory;

        public HssKeyPairFactory(ILmsKeyPairFactory lmsKeyPairFactory, ILmsSigner lmsSigner, IShaFactory shaFactory, ISeedIdRotator seedIdRotator)
        {
            _lmsKeyPairFactory = lmsKeyPairFactory;
            _lmsSigner = lmsSigner;
            _shaFactory = shaFactory;
            _seedIdRotator = seedIdRotator;
        }

        public async Task<IHssKeyPair> GetKeyPair(HssLevelParameter[] hssLevelParameters, ILmOtsRandomizerC randomizerC, byte[] i, byte[] seed)
        {
            var privateKey = GetPrivateKey(hssLevelParameters, i, seed);
            var publicKey = await GetPublicKey(privateKey, randomizerC);

            return new HssKeyPair
            {
                Levels = hssLevelParameters.Length,
                PrivateKey = privateKey,
                PublicKey = publicKey
            };
        }

        public async Task<bool> RegenerateLmsTreesWhereRequired(IHssKeyPair keyPair, ILmOtsRandomizerC randomizerC, int incrementQ)
        {
            var levels = keyPair.Levels;
            var lmsKeys = await keyPair.PrivateKey.Keys;

            if (incrementQ == 0)
            {
                // There's no need to regenerate at the point of the lowest level tree not being exhausted.
                if (!lmsKeys[levels - 1].PrivateKey.IsExhausted)
                    return true;

                return await RegenerateLmsTreesWherePossible(keyPair, randomizerC, keyPair.Levels - 1);
            }

            for (var i = 0; i < incrementQ; i++)
            {
                lmsKeys[levels - 1].PrivateKey.GetQ();

                if (lmsKeys[levels - 1].PrivateKey.IsExhausted)
                {
                    var validKey = await RegenerateLmsTreesWherePossible(keyPair, randomizerC, keyPair.Levels - 1);
                    if (!validKey)
                        return false;
                }
            }

            return true;
        }

        public  IHssPrivateKey GetPrivateKey(HssLevelParameter[] hssLevelParameters, byte[] i, byte[] seed)
        {
            if (hssLevelParameters == null || hssLevelParameters.Length < 1)
            {
                throw new ArgumentException(
                    "GetPrivateKey generation request much contain at least one level.", nameof(hssLevelParameters));
            }

            var levels = hssLevelParameters.Length;
            if (levels > 8)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(hssLevelParameters), "The maximum allowed levels for HSS keys is 8.");
            }

            ValidateUnderlyingOutputFunction(hssLevelParameters);

            var lmsTrees = new ILmsKeyPair[levels];
            lmsTrees[0] = _lmsKeyPairFactory.GetKeyPair(hssLevelParameters[0].LmsMode, hssLevelParameters[0].LmOtsMode, i, seed);

            var idSeedResult = new IdSeedResult(i, seed);

            // TODO this is not *necessarily* needed at this point, only at the point of signing messages.
            for (var j = 1; j < levels; j++)
            {
                var sha = LmsHelpers.GetSha(_shaFactory, hssLevelParameters[j].LmOtsMode);

                var lmsAttribute = AttributesHelper.GetLmsAttribute(hssLevelParameters[j].LmsMode);
                idSeedResult = _seedIdRotator.GetNewSeedId(sha, lmsAttribute, idSeedResult.I, idSeedResult.Seed, j);

                lmsTrees[j] = _lmsKeyPairFactory.GetKeyPair(hssLevelParameters[j].LmsMode, hssLevelParameters[j].LmOtsMode, idSeedResult.I, idSeedResult.Seed);
            }

            return new HssPrivateKey
            {
                Keys = Task.FromResult(lmsTrees),
                Levels = levels
            };
        }

        public async Task<IHssPublicKey> GetPublicKey(IHssPrivateKey privateKey, ILmOtsRandomizerC randomizerC)
        {
            var lmsKeys = await privateKey.Keys;
            var lmsRootPublicKey = lmsKeys[0].PublicKey.Key;
            var hssPublicKey = new byte[4 + lmsRootPublicKey.Length];
            var levels = privateKey.Levels;
            var levelsBytes = levels.GetBytes();

            var sigs = new byte[levels][];
            for (var i = 1; i < privateKey.Levels; i++)
            {
                var signature = _lmsSigner.Sign(lmsKeys[i - 1].PrivateKey, randomizerC, lmsKeys[i].PublicKey.Key);
                sigs[i - 1] = signature.Signature;
            }

            // Return u32str(L) || pub[0]
            Array.Copy(levelsBytes, 0, hssPublicKey, 0, levelsBytes.Length);
            Array.Copy(lmsRootPublicKey, 0, hssPublicKey, 4, lmsRootPublicKey.Length);

            return new HssPublicKey
            {
                Levels = levels,
                Key = Task.FromResult(hssPublicKey),
                Signatures = Task.FromResult(sigs)
            };
        }

        private async Task<bool> RegenerateLmsTreesWherePossible(IHssKeyPair keyPair, ILmOtsRandomizerC randomizerC, int level)
        {
            var lmsKeys = await keyPair.PrivateKey.Keys;
            if (level == 0 && lmsKeys[0].PrivateKey.IsExhausted)
            {
                keyPair.IsExhausted = true;
                return false;
            }

            // If the current level's key pair is exhausted, we need to go up a level
            if (lmsKeys[level].PrivateKey.IsExhausted)
            {
                await RegenerateLmsTreesWherePossible(keyPair, randomizerC, level - 1);
            }

            // If the current level isn't the "lowest level", and the level "below this one" is exhausted, regenerate it.
            if (level != keyPair.Levels - 1 && lmsKeys[level + 1].PrivateKey.IsExhausted)
            {
                // The current level lms tree is not exhausted, regenerate the tree below it
                var keyToRegen = lmsKeys[level + 1];
                var sha = LmsHelpers.GetSha(_shaFactory, keyToRegen.LmsAttribute.Mode);
                var newIdSeed = _seedIdRotator.GetNewSeedId(sha, keyToRegen.LmsAttribute, keyToRegen.PrivateKey.I, keyToRegen.PrivateKey.Seed, level);
                lmsKeys[level + 1] = _lmsKeyPairFactory.GetKeyPair(keyToRegen.LmsAttribute.Mode, keyToRegen.LmOtsAttribute.Mode, newIdSeed.I, newIdSeed.Seed);

                // Finally, generate a new signature for the current level based on the new tree's public key.
                var signatures = await keyPair.PublicKey.Signatures;
                var signature = _lmsSigner.Sign(lmsKeys[level].PrivateKey, randomizerC, lmsKeys[level + 1].PublicKey.Key);
                signatures[level] = signature.Signature;
            }

            return true;
        }

        private void ValidateUnderlyingOutputFunction(HssLevelParameter[] hssLevelParameters)
        {
            var lmsAttributes = new List<LmsAttribute>();
            var lmOtsAttributes = new List<LmOtsAttribute>();

            foreach (var hssLevelParameter in hssLevelParameters)
            {
                lmsAttributes.Add(AttributesHelper.GetLmsAttribute(hssLevelParameter.LmsMode));
                lmOtsAttributes.Add(AttributesHelper.GetLmOtsAttribute(hssLevelParameter.LmOtsMode));
            }

            var m = lmsAttributes[0].M;
            var shaMode = lmsAttributes[0].ShaMode;

            // All output lengths must be equal
            if (lmsAttributes.Any(a => a.M != m) || lmOtsAttributes.Any(a => a.N != m))
            {
                throw new ArgumentException("All output lengths for the HSS must be equal.");
            }

            // All underlying hash functions must be equal
            if (lmsAttributes.Any(a => a.ShaMode != shaMode) || lmOtsAttributes.Any(a => a.ShaMode != shaMode))
            {
                throw new ArgumentException("All hash functions for the HSS must be equal.");
            }
        }
    }
}
