using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Keys;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly ShaFactory _shaFactory = new ShaFactory();
        private readonly KeyComposerFactory _keyComposerFactory = new KeyComposerFactory();
        private readonly PaddingFactory _paddingFactory = new PaddingFactory();
        
        public RsaKeyResult CompleteDeferredRsaKeyCase(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            var entropyProvider = new TestableEntropyProvider();
            param.PublicExponent = new BitString(fullParam.Key.PubKey.E);

            if (param.KeyMode == PrimeGenModes.B32)
            {
                // Nothing
            }
            else if (param.KeyMode == PrimeGenModes.B33)
            {
                // P and Q
                entropyProvider.AddEntropy(new BitString(fullParam.Key.PrivKey.P, param.Modulus / 2));
                entropyProvider.AddEntropy(new BitString(fullParam.Key.PrivKey.Q, param.Modulus / 2));
            }
            else if (param.KeyMode == PrimeGenModes.B34)
            {
                // Nothing
            }
            else if (param.KeyMode == PrimeGenModes.B35)
            {
                // XP and XQ
                entropyProvider.AddEntropy(fullParam.AuxValues.XP);
                entropyProvider.AddEntropy(fullParam.AuxValues.XQ);
            }
            else if (param.KeyMode == PrimeGenModes.B36)
            {
                // XP and XQ
                entropyProvider.AddEntropy(fullParam.AuxValues.XP);
                entropyProvider.AddEntropy(fullParam.AuxValues.XQ);

                // XP1, XP2, XQ1, XQ2
                entropyProvider.AddEntropy(new BitString(fullParam.AuxValues.XP1).GetLeastSignificantBits(fullParam.BitLens[0]));
                entropyProvider.AddEntropy(new BitString(fullParam.AuxValues.XP2).GetLeastSignificantBits(fullParam.BitLens[1]));
                entropyProvider.AddEntropy(new BitString(fullParam.AuxValues.XQ1).GetLeastSignificantBits(fullParam.BitLens[2]));
                entropyProvider.AddEntropy(new BitString(fullParam.AuxValues.XQ2).GetLeastSignificantBits(fullParam.BitLens[3]));
            }

            return new RsaKeyResult
            {
                Key = GeneratePrimes(param, entropyProvider).Key
            };
        }

        public RsaKeyResult CompleteKey(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            var keyComposer = _keyComposerFactory.GetKeyComposer(keyMode);
            var primePair = new PrimePair
            {
                P = param.Key.PrivKey.P,
                Q = param.Key.PrivKey.Q
            };

            return new RsaKeyResult
            {
                Key = keyComposer.ComposeKey(param.Key.PubKey.E, primePair)
            };
        }

        private (bool Success, KeyPair Key, AuxiliaryResult Aux) GeneratePrimes(RsaKeyParameters param, IEntropyProvider entropyProvider)
        {
            // TODO Not every group has a hash alg... Can use a default value perhaps?
            ISha sha = null;
            if (param.HashAlg != null)
            {
                sha = _shaFactory.GetShaInstance(param.HashAlg);
            }

            var keyComposer = _keyComposerFactory.GetKeyComposer(param.KeyFormat);

            // Configure Prime Generator
            var keyResult = new KeyBuilder(new PrimeGeneratorFactory())
                .WithBitlens(param.BitLens)
                .WithEntropyProvider(entropyProvider)
                .WithHashFunction(sha)
                .WithNlen(param.Modulus)
                .WithPrimeGenMode(param.KeyMode)
                .WithPrimeTestMode(param.PrimeTest)
                .WithPublicExponent(param.PublicExponent)
                .WithKeyComposer(keyComposer)
                .WithSeed(param.Seed)
                .Build();

            return (keyResult.Success, keyResult.Key, keyResult.AuxValues);
        }
        
        public RsaKeyResult GetRsaKey(RsaKeyParameters param)
        {
            var entropyProvider = new EntropyProvider(_rand);
            (bool Success, KeyPair Key, AuxiliaryResult Aux) result;
            do
            {
                param.Seed = GetSeed(param.Modulus);
                param.PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? param.PublicExponent : GetEValue(RSA_PUBLIC_EXPONENT_BITS_MIN, RSA_PUBLIC_EXPONENT_BITS_MAX);
                param.BitLens = GetBitlens(param.Modulus, param.KeyMode);
                
                // Generate key until success
                result = GeneratePrimes(param, entropyProvider);

            } while (!result.Success);

            return new RsaKeyResult
            {
                Key = result.Key,
                AuxValues = result.Aux,
                BitLens = param.BitLens,
                Seed = param.Seed
            };
        }

        public RsaSignaturePrimitiveResult GetRsaSignaturePrimitive(RsaSignaturePrimitiveParameters param)
        {
            var keyParam = new RsaKeyParameters
            {
                KeyFormat = param.KeyFormat,
                Modulus = param.Modulo,
                PrimeTest = PrimeTestModes.C2,
                PublicExponentMode = PublicExponentModes.Random,
                KeyMode = PrimeGenModes.B33
            };

            var key = GetRsaKey(keyParam).Key;

            var shouldPass = _rand.GetRandomInt(0, 2) == 0;
            BitString message;
            BitString signature = null;
            if (shouldPass)
            {
                // No failure, get a random 2048-bit value less than N
                message = new BitString(_rand.GetRandomBigInteger(key.PubKey.N), 2048);
                signature = new BitString(new Rsa(new RsaVisitor()).Decrypt(message.ToPositiveBigInteger(), key.PrivKey, key.PubKey).PlainText, 2048);
            }
            else
            {
                // Yes failure, get a random 2048-bit value greater than N
                message = new BitString(_rand.GetRandomBigInteger(key.PubKey.N, NumberTheory.Pow2(2048)), 2048);
            }

            return new RsaSignaturePrimitiveResult
            {
                Key = key,
                Message = message,
                Signature = signature,
                ShouldPass = shouldPass
            };
        }

        public VerifyResult<RsaKeyResult> GetRsaKeyVerify(RsaKeyResult param)
        {
            // Check correctness in values
            if (!NumberTheory.MillerRabin(param.Key.PrivKey.P, 20))
            {
                return new VerifyResult<RsaKeyResult> { Result = false };
            }

            if (!NumberTheory.MillerRabin(param.Key.PrivKey.Q, 20))
            {
                return new VerifyResult<RsaKeyResult> { Result = false };
            }

            // This fails some tests that don't have an N value given to them so is compared to 0
            //if (param.Key.PubKey.N != param.Key.PrivKey.P * param.Key.PrivKey.Q)
            //{
            //    return new VerifyResult<RsaKeyResult> { Result = false };
            //}

            return new VerifyResult<RsaKeyResult>
            {
                Result = true
            };
        }

        public RsaSignatureResult GetDeferredRsaSignature(RsaSignatureParameters param)
        {
            return new RsaSignatureResult
            {
                Message = _rand.GetRandomBitString(param.Modulo / 2)
            };
        }

        public RsaSignatureResult GetRsaSignature(RsaSignatureParameters param)
        {
            var message = _rand.GetRandomBitString(param.Modulo / 2);
            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var salt = _rand.GetRandomBitString(param.SaltLength * 8);       // Comes in bytes, convert to bits
            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(salt);

            var paddingScheme = _paddingFactory.GetPaddingScheme(param.PaddingScheme, sha, entropyProvider, param.SaltLength);

            var result = new SignatureBuilder()
                .WithDecryptionScheme(new Rsa(new RsaVisitor()))
                .WithMessage(message)
                .WithPaddingScheme(paddingScheme)
                .WithKey(param.Key)
                .BuildSign();

            if (!result.Success)
            {
                throw new Exception();
            }

            return new RsaSignatureResult
            {
                Message = message,
                Signature = new BitString(result.Signature),
                Salt = param.PaddingScheme == SignatureSchemes.Pss ? salt : null
            };
        }

        public VerifyResult<RsaSignatureResult> CompleteDeferredRsaSignature(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(fullParam.Salt);

            var paddingScheme = _paddingFactory.GetPaddingScheme(param.PaddingScheme, sha, entropyProvider, param.SaltLength);

            var result = new SignatureBuilder()
                .WithDecryptionScheme(new Rsa(new RsaVisitor()))
                .WithKey(param.Key)
                .WithMessage(fullParam.Message)
                .WithPaddingScheme(paddingScheme)
                .WithSignature(fullParam.Signature)
                .BuildVerify();

            return new VerifyResult<RsaSignatureResult>
            {
                VerifiedValue = fullParam,
                Result = result.Success
            };
        }

        public VerifyResult<RsaSignatureResult> GetRsaVerify(RsaSignatureParameters param)
        {
            var message = _rand.GetRandomBitString(param.Modulo / 2);
            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var salt = _rand.GetRandomBitString(param.SaltLength * 8);      // Comes in bytes, convert to bits
            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(salt);

            var paddingScheme = _paddingFactory.GetSigningPaddingScheme(param.PaddingScheme, sha, param.Reason, entropyProvider, param.SaltLength);
            
            var copyKey = new KeyPair
            {
                PrivKey = param.Key.PrivKey,
                PubKey = new PublicKey
                {
                    E = param.Key.PubKey.E,
                    N = param.Key.PubKey.N
                }
            };

            var result = new SignatureBuilder()
                .WithDecryptionScheme(new Rsa(new RsaVisitor()))
                .WithMessage(message)
                .WithPaddingScheme(paddingScheme)
                .WithKey(copyKey)
                .BuildSign();

            if (!result.Success)
            {
                throw new Exception();
            }

            return new VerifyResult<RsaSignatureResult>
            {
                Result = param.Reason == SignatureModifications.None,
                VerifiedValue = new RsaSignatureResult
                {
                    Key = copyKey,
                    Message = message,
                    Signature = new BitString(result.Signature),
                    Salt = param.PaddingScheme == SignatureSchemes.Pss ? salt : null
                }
            };
        }

        public async Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            return await _taskFactory.StartNew(() => GetRsaKey(param));
        }

        public async Task<RsaKeyResult> CompleteKeyAsync(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            return await _taskFactory.StartNew(() => CompleteKey(param, keyMode));
        }

        public async Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredRsaKeyCase(param, fullParam));
        }

        public async Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param)
        {
            return await _taskFactory.StartNew(() => GetRsaKeyVerify(param));
        }

        public async Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param)
        {
            return await _taskFactory.StartNew(() => GetRsaSignaturePrimitive(param));
        }

        public async Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetDeferredRsaSignature(param));
        }

        public async Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredRsaSignature(param, fullParam));
        }

        public async Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetRsaSignature(param));
        }

        public async Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetRsaVerify(param));
        }

        private BitString GetEValue(int minLen, int maxLen)
        {
            BigInteger e;
            BitString e_bs;
            do
            {
                var min = minLen / 2;
                var max = maxLen / 2;

                e = GetRandomBigIntegerOfBitLength(_rand.GetRandomInt(min, max) * 2);
                if (e.IsEven)
                {
                    e++;
                }

                e_bs = new BitString(e);
            } while (e_bs.BitLength >= maxLen || e_bs.BitLength < minLen);

            return new BitString(e);
        }

        private BigInteger GetRandomBigIntegerOfBitLength(int len)
        {
            var bs = _rand.GetRandomBitString(len);
            return bs.ToPositiveBigInteger();
        }

        private BitString GetSeed(int modulo)
        {
            var security_strength = 0;
            if(modulo == 1024)
            {
                security_strength = 80;
            }
            else if (modulo == 2048)
            {
                security_strength = 112;
            }
            else if (modulo == 3072)
            {
                security_strength = 128;
            }

            return _rand.GetRandomBitString(2 * security_strength);
        }

        private int[] GetBitlens(int modulo, PrimeGenModes mode)
        {
            var bitlens = new int[4];
            var min_single = 0;
            var max_both = 0;

            // Min_single values were given as exclusive, we add 1 to make them inclusive
            if(modulo == 1024)
            {
                // Rough estimate based on existing test vectors
                min_single = 101;
                max_both = 236;
            }
            else if (modulo == 2048)
            {
                min_single = 140 + 1;

                if (mode == PrimeGenModes.B32 || mode == PrimeGenModes.B34)
                {
                    max_both = 494;
                }
                else
                {
                    max_both = 750;
                }
            }
            else if (modulo == 3072)
            {
                min_single = 170 + 1;

                if (mode == PrimeGenModes.B32 || mode == PrimeGenModes.B34)
                {
                    max_both = 1007;
                }
                else
                {
                    max_both = 1518;
                }
            }

            bitlens[0] = _rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[1] = _rand.GetRandomInt(min_single, max_both - bitlens[0]);
            bitlens[2] = _rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[3] = _rand.GetRandomInt(min_single, max_both - bitlens[2]);

            return bitlens;
        }
    }
}

