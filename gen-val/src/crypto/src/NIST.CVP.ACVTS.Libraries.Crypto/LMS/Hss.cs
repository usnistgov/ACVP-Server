using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS
{
    public class Hss : IHss
    {
        #region Fields

        private Lms[] _lms;
        private readonly LmsType[] _lmsTypes;
        private readonly LmotsType[] _lmotsTypes;
        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IEntropyProvider _entropyProvider;
        private readonly EntropyProviderTypes _entropyType;
        private readonly ISha _sha256;
        public BitString SEED { get; set; }
        public BitString RootI { get; set; }

        #endregion Fields

        #region Constructors

        // From email with Scott Fluhrer:
        // To generate the SEED for a child LMS tree, we will adapt algorithm A by using the 
        // I value of the parent LMS tree the q value to be the LMS index of the child, and the i value to be 65534.
        // Algorithm A:
        // x_q[i] = H(I || u32str(q) || u16str(i) || u8str(0xff) || SEED).
        // UPDATE: Generate child I from algorithm A using the I value of the parent LMS tree, the q value 
        // to be the LMS index of the child, and the i value to be 65535; the I value will be the first 128 bits of the hash.
        public Hss(int layers, LmsType[] lmsTypes, LmotsType[] lmotsTypes,
            EntropyProviderTypes entropyType = EntropyProviderTypes.Random, BitString seed = null, BitString rootI = null)
        {
            _entropyType = entropyType;
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
            _entropyProvider.AddEntropy(seed);
            SEED = _entropyProvider.GetEntropy(256);    // for now will only be 256 bits (m = 256)
            _entropyProvider.AddEntropy(rootI);
            RootI = _entropyProvider.GetEntropy(128);
            _sha256 = new NativeFastSha2_256();

            _lms = new Lms[layers];
            _lmsTypes = lmsTypes;
            _lmotsTypes = lmotsTypes;
            _lms[0] = new Lms(lmsTypes[0], lmotsTypes[0], entropyType, SEED, rootI);
            var parentSeed = SEED;
            var parentI = RootI;
            for (int i = 1; i < layers; i++)
            {
                var childSeed = _sha256.HashMessage(parentI
                    .ConcatenateBits(new BitString(0, 32))
                    .ConcatenateBits(new BitString(65534, 16))
                    .ConcatenateBits(new BitString("ff", 8))
                    .ConcatenateBits(parentSeed)).Digest;
                var I = _sha256.HashMessage(parentI
                    .ConcatenateBits(new BitString(0, 32))
                    .ConcatenateBits(new BitString(65535, 16))
                    .ConcatenateBits(new BitString("ff", 8))
                    .ConcatenateBits(parentSeed)).Digest.MSBSubstring(0, 128);
                _lms[i] = new Lms(lmsTypes[i], lmotsTypes[i], entropyType, childSeed, I);
                parentSeed = childSeed;
                parentI = I;
            }
        }

        #endregion Constructors

        #region Key Generation

        /// <summary>
        /// Generates a Hss Key Pair asynchronously. Safe to call from an Orleans grain.
        /// </summary>
        /// <returns>Valid HssKeyPair</returns>
        public async Task<HssKeyPair> GenerateHssKeyPairAsync()
        {
            LmsPrivateKey[] priv = new LmsPrivateKey[_lms.Length];
            BitString[] pub = new BitString[_lms.Length];
            BitString[] sig = new BitString[_lms.Length - 1];

            // Generate Key Pairs
            await GenerateHssKeyPairHelperAsync(priv, pub);

            // Generate sig list
            for (int i = 1; i < _lms.Length; i++)
            {
                sig[i - 1] = _lms[i - 1].GenerateLmsSignature(pub[i], priv[i - 1]);
            }

            // Return u32str(L) || pub[0] as the public key and the priv[], pub[], and sig[] arrays as the private key
            var publicKey = new BitString(_lms.Length, 32).ConcatenateBits(pub[0]);
            var privateKey = new HssPrivateKey(priv, pub, sig);
            return new HssKeyPair(privateKey, publicKey);
        }

        /// <summary>
        /// Creates multiple threads for generating lms key pairs.
        /// </summary>
        /// <returns></returns>
        private async Task GenerateHssKeyPairHelperAsync(LmsPrivateKey[] priv, BitString[] pub)
        {
            var tasks = new Task<LmsKeyPair>[_lms.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = _lms[i].GenerateLmsKeyPairAsync();
            }
            await Task.WhenAll(tasks);

            for (int i = 0; i < tasks.Length; i++)
            {
                priv[i] = tasks[i].Result.PrivateKey;
                pub[i] = tasks[i].Result.PublicKey;
            }
        }

        #endregion Key Generation

        #region UpdateKeyPair

        /// <summary>
        /// Updates the key pair one step. Should be called my MCT in order to safely advance key.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <returns></returns>
        public async Task<HssKeyPair> UpdateKeyPairOneStepAsync(HssKeyPair keyPair)
        {
            var keyPairCopy = keyPair.GetDeepCopy();

            int d = _lms.Length;
            while (keyPairCopy.PrivateKey.PrivateKeys[d - 1].Q == (keyPairCopy.PrivateKey.PrivateKeys[d - 1].OTS_PRIV.Length
                / (_lms[d - 1].GetLmotsN() * _lms[d - 1].GetLmotsP() + 24)) - 1)
            {
                d--;
                if (d == 0)
                {
                    keyPairCopy.Expired = true;
                    return keyPairCopy;
                }
            }
            var lowD = d;
            if (d < _lms.Length)
            {
                while (d < _lms.Length)
                {
                    var newSeed = _sha256.HashMessage(_lms[d - 1].GetI()
                            .ConcatenateBits(new BitString(keyPairCopy.PrivateKey.PrivateKeys[d - 1].Q + 1, 32))
                            .ConcatenateBits(new BitString(65534, 16))
                            .ConcatenateBits(new BitString("ff", 8))
                            .ConcatenateBits(_lms[d - 1].GetSeed())).Digest;
                    var newI = _sha256.HashMessage(_lms[d - 1].GetI()
                        .ConcatenateBits(new BitString(keyPairCopy.PrivateKey.PrivateKeys[d - 1].Q + 1, 32))
                        .ConcatenateBits(new BitString(65535, 16))
                        .ConcatenateBits(new BitString("ff", 8))
                        .ConcatenateBits(_lms[d - 1].GetSeed())).Digest;
                    newI = newI.MSBSubstring(0, 128);
                    _lms[d] = new Lms(_lmsTypes[d], _lmotsTypes[d], _entropyType, newSeed, newI);
                    var newkeyPairCopy = await _lms[d].GenerateLmsKeyPairAsync();
                    keyPairCopy.PrivateKey.PrivateKeys[d] = newkeyPairCopy.PrivateKey;
                    keyPairCopy.PrivateKey.PublicKeys[d] = newkeyPairCopy.PublicKey;
                    keyPairCopy.PrivateKey.PrivateKeys[d - 1].Q = (keyPairCopy.PrivateKey.PrivateKeys[d - 1].Q + 1) % (1 << _lms[d - 1].GetH());
                    keyPairCopy.PrivateKey.Signatures[d - 1] = _lms[d - 1].GenerateLmsSignature(
                        keyPairCopy.PrivateKey.PublicKeys[d], keyPairCopy.PrivateKey.PrivateKeys[d - 1]);

                    if (keyPairCopy.PrivateKey.Signatures[d - 1] == null)
                    {
                        keyPairCopy.Expired = true;
                        return keyPairCopy;
                    }

                    d++;
                }
            }
            else
            {
                // Update Q value
                keyPairCopy.PrivateKey.PrivateKeys[d - 1].Q = (keyPairCopy.PrivateKey.PrivateKeys[d - 1].Q + 1) % (1 << _lms[d - 1].GetH());
            }

            return keyPairCopy;
        }

        /// <summary>
        /// Used in signature generation to sign after advancing the key some number of times.
        /// This function is unsafe to use if the keyPair has been advanced before.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        private async Task<HssKeyPair> UpdateKeyPairAsync(HssKeyPair keyPair, int times = 1)
        {
            if (times == 0)
            {
                return keyPair;
            }
            keyPair = keyPair.GetDeepCopy();

            var divisor = 1;
            for (int i = 1; i < _lms.Length; i++)
            {
                divisor *= (1 << _lms[i].GetH());
            }

            // If update would cause the key to expire, then expire key
            if (((1 << _lms[0].GetH()) - keyPair.PrivateKey.PrivateKeys[0].Q) * divisor <= times)
            {
                keyPair.Expired = true;
                return keyPair;
            }

            for (int d = 0; d < _lms.Length; d++)
            {
                // Update divisor for next step
                if (d != 0)
                {
                    divisor /= (1 << _lms[d].GetH());
                }

                var qStep = times / divisor;

                // If tree update is needed
                if (qStep + keyPair.PrivateKey.PrivateKeys[d].Q >= (1 << _lms[d].GetH()))
                {
                    // If update would cause the key to expire, then expire key
                    if (d == 0)
                    {
                        keyPair.Expired = true;
                        return keyPair;
                    }
                    var newSeed = _sha256.HashMessage(_lms[d - 1].GetI()
                        .ConcatenateBits(new BitString(keyPair.PrivateKey.PrivateKeys[d - 1].Q, 32))
                        .ConcatenateBits(new BitString(65534, 16))
                        .ConcatenateBits(new BitString("ff", 8))
                        .ConcatenateBits(_lms[d - 1].GetSeed())).Digest;
                    var newI = _sha256.HashMessage(_lms[d - 1].GetI()
                        .ConcatenateBits(new BitString(keyPair.PrivateKey.PrivateKeys[d - 1].Q, 32))
                        .ConcatenateBits(new BitString(65535, 16))
                        .ConcatenateBits(new BitString("ff", 8))
                        .ConcatenateBits(_lms[d - 1].GetSeed())).Digest;
                    newI = newI.MSBSubstring(0, 128);
                    _lms[d] = new Lms(_lmsTypes[d], _lmotsTypes[d], _entropyType, newSeed, newI);
                    var newKeyPair = await _lms[d].GenerateLmsKeyPairAsync();
                    keyPair.PrivateKey.PrivateKeys[d] = newKeyPair.PrivateKey;
                    keyPair.PrivateKey.PublicKeys[d] = newKeyPair.PublicKey;
                    keyPair.PrivateKey.Signatures[d - 1] = _lms[d - 1].GenerateLmsSignature(
                        keyPair.PrivateKey.PublicKeys[d], keyPair.PrivateKey.PrivateKeys[d - 1]);
                }

                // Update Q value
                keyPair.PrivateKey.PrivateKeys[d].Q = (keyPair.PrivateKey.PrivateKeys[d].Q + qStep) % (1 << _lms[d].GetH());
            }

            return keyPair;
        }

        #endregion UpdateKeyPair

        #region Signature Generation

        // returns null if keyPair is exhausted
        public async Task<HssSignatureResult> GenerateHssSignatureAsync(BitString msg, HssKeyPair keyPair, int advanced = 0)
        {
            // 1. If the message-signing key prv[L - 1] is exhausted, regenerate that key pair, together with any 
            //    parent key pairs that might be necessary. If the root key pair is exhausted, then the HSS key pair is
            //    exhausted and MUST NOT generate any more signatures.
            keyPair = await UpdateKeyPairAsync(keyPair, advanced);
            if (keyPair.Expired)
            {
                return new HssSignatureResult("Key expired.");
            }

            // 2. Sign the message.
            var sig = _lms[_lms.Length - 1].GenerateLmsSignature(msg, keyPair.PrivateKey.PrivateKeys[_lms.Length - 1]);

            // 3. Create the list of signed public keys.
            var signedPubKey = new BitString("");
            for (int i = 0; i < _lms.Length - 1; i++)
            {
                signedPubKey = signedPubKey.ConcatenateBits(keyPair.PrivateKey.Signatures[i])
                    .ConcatenateBits(keyPair.PrivateKey.PublicKeys[i + 1]);
            }

            // 4. Return u32str(L-1) || signed_pub_key[0] || ... || signed_pub_key[L - 2] || sig[L - 1]
            return new HssSignatureResult(new BitString(_lms.Length - 1, 32).ConcatenateBits(signedPubKey).ConcatenateBits(sig));
        }

        #endregion Signature Generation

        #region Signature Verification

        public HssVerificationResult VerifyHssSignature(BitString msg, BitString publicKey, BitString signature)
        {
            // 1. The signature S is parsed into its components as follows:
            // a. Nspk = strTou32(first four bytes of S)
            //    if Nspk+1 is not equal to the number of levels L in pub return INVALID
            var Nspk = (int)signature.MSBSubstring(0, 32).ToPositiveBigInteger();
            var lengthInPublic = (int)publicKey.MSBSubstring(0, 32).ToPositiveBigInteger();
            if (Nspk + 1 != lengthInPublic)
            {
                return new HssVerificationResult("Validation failed. L values do not match.");
            }

            // b. for (i = 0; i<Nspk; i = i + 1) {
            //        siglist[i] = next LMS signature parsed from S
            //        publist[i] = next LMS public key parsed from S
            //    }
            var siglist = new BitString[Nspk + 1];
            var publist = new BitString[Nspk + 1];
            var currIndex = 32;

            for (int i = 0; i < Nspk; i++)
            {
                // assume sig and pub have same LMS mode
                var otsCode = signature.MSBSubstring(currIndex + 32, 32);
                var n = LmotsModeMapping.GetNFromCode(otsCode);
                var p = LmotsModeMapping.GetPFromCode(otsCode);
                var sigtype = signature.MSBSubstring((8 + n * (p + 1)) * 8 + currIndex, 32);
                var m = LmsModeMapping.GetMFromCode(sigtype);
                var h = LmsModeMapping.GetHFromCode(sigtype);
                var siglen = (12 + n * (p + 1) + m * h) * 8;
                var publen = 192 + (m * 8);
                siglist[i] = signature.MSBSubstring(currIndex, siglen);
                currIndex += siglen;
                publist[i] = signature.MSBSubstring(currIndex, publen);
                currIndex += publen;
            }

            // c. siglist[Nspk] = next LMS signature parsed from S
            var otsCodeLast = signature.MSBSubstring(currIndex + 32, 32);
            var nLast = LmotsModeMapping.GetNFromCode(otsCodeLast);
            var pLast = LmotsModeMapping.GetPFromCode(otsCodeLast);
            var sigtypeLast = signature.MSBSubstring((8 + nLast * (pLast + 1)) * 8 + currIndex, 32);
            var mLast = LmsModeMapping.GetMFromCode(sigtypeLast);
            var hLast = LmsModeMapping.GetHFromCode(sigtypeLast);
            var siglenLast = (12 + nLast * (pLast + 1) + mLast * hLast) * 8;
            siglist[Nspk] = signature.MSBSubstring(currIndex, siglenLast);

            // 2. Verify each part of the signature
            var key = publicKey.MSBSubstring(32, publicKey.BitLength - 32);
            for (int i = 0; i < Nspk; i++)
            {
                var result = _lms[i].VerifyLmsSignature(publist[i], key, siglist[i]);
                if (!result.Success)
                {
                    return new HssVerificationResult("LMS Validation failed: " + result.ErrorMessage);
                }
                key = publist[i];
            }

            // 3. return lms_verify(message, key, siglist[Nspk])
            var finalResult = _lms[Nspk].VerifyLmsSignature(msg, key, siglist[Nspk]);
            if (finalResult.Success)
            {
                return new HssVerificationResult();
            }
            else
            {
                return new HssVerificationResult("Validation failed. Final check failed: " + finalResult.ErrorMessage);
            }
        }

        #endregion Signature Verification
    }
}
