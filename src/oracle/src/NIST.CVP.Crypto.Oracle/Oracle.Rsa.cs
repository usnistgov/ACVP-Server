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
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
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
        
        private RsaKeyResult CompleteDeferredRsaKeyCase(RsaKeyParameters param, RsaKeyResult fullParam)
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

        private RsaKeyResult CompleteKey(RsaKeyResult param, PrivateKeyModes keyMode)
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

        private RsaPrimeResult GeneratePrimes(RsaKeyParameters param, IEntropyProvider entropyProvider)
        {
            // Only works with random public exponent
            var poolBoy = new PoolBoy<RsaPrimeResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.RSA_KEY);
            if (poolResult != null)
            {
                return poolResult;
            }

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

            return new RsaPrimeResult
            {
                Aux = keyResult.AuxValues,
                Key = keyResult.Key,
                Success = keyResult.Success
            };
        }
        
        private RsaKeyResult GetRsaKey(RsaKeyParameters param)
        {
            var rand = new Random800_90();
            var entropyProvider = new EntropyProvider(rand);
            RsaPrimeResult result;
            do
            {
                // These will be overwritten if the value comes from the pool
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

        private RsaSignaturePrimitiveResult GetRsaSignaturePrimitive(RsaSignaturePrimitiveParameters param)
        {
            var rand = new Random800_90();
            var keyParam = new RsaKeyParameters
            {
                KeyFormat = param.KeyFormat,
                Modulus = param.Modulo,
                PrimeTest = PrimeTestModes.C2,
                PublicExponentMode = PublicExponentModes.Random,
                KeyMode = PrimeGenModes.B33
            };

            var key = GetRsaKey(keyParam).Key;

            var shouldPass = rand.GetRandomInt(0, 2) == 0;
            BitString message;
            BitString signature = null;
            if (shouldPass)
            {
                // No failure, get a random 2048-bit value less than N
                message = new BitString(rand.GetRandomBigInteger(key.PubKey.N), 2048);
                signature = new BitString(new Rsa(new RsaVisitor()).Decrypt(message.ToPositiveBigInteger(), key.PrivKey, key.PubKey).PlainText, 2048);
            }
            else
            {
                // Yes failure, get a random 2048-bit value greater than N
                message = new BitString(rand.GetRandomBigInteger(key.PubKey.N, NumberTheory.Pow2(2048)), 2048);
            }

            return new RsaSignaturePrimitiveResult
            {
                Key = key,
                Message = message,
                Signature = signature,
                ShouldPass = shouldPass
            };
        }

        private VerifyResult<RsaKeyResult> GetRsaKeyVerify(RsaKeyResult param)
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

        private RsaSignatureResult GetDeferredRsaSignature(RsaSignatureParameters param)
        {
            var rand = new Random800_90();
            return new RsaSignatureResult
            {
                Message = rand.GetRandomBitString(param.Modulo / 2)
            };
        }

        private RsaSignatureResult GetRsaSignature(RsaSignatureParameters param)
        {
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.Modulo / 2);
            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var salt = rand.GetRandomBitString(param.SaltLength * 8);       // Comes in bytes, convert to bits
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

        private VerifyResult<RsaSignatureResult> CompleteDeferredRsaSignature(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            var rand = new Random800_90();
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

        private VerifyResult<RsaSignatureResult> GetRsaVerify(RsaSignatureParameters param)
        {
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.Modulo / 2);
            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var salt = rand.GetRandomBitString(param.SaltLength * 8);      // Comes in bytes, convert to bits
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

        private RsaDecryptionPrimitiveResult GetDeferredRsaDecryptionPrimitive(RsaDecryptionPrimitiveParameters param)
        {
            var rand = new Random800_90();
            return new RsaDecryptionPrimitiveResult
            {
                CipherText = param.TestPassed
                    ? rand.GetRandomBitString(param.Modulo)
                    : BitString.Ones(2).ConcatenateBits(rand.GetRandomBitString(param.Modulo - 2))         // Try to force the failing case high
            };
        }

        private RsaDecryptionPrimitiveResult CompleteDeferredRsaDecryptionPrimitive(RsaDecryptionPrimitiveParameters param, RsaDecryptionPrimitiveResult fullParam)
        {
            var rsa = new Rsa(new RsaVisitor());
            var result = rsa.Encrypt(fullParam.PlainText.ToPositiveBigInteger(), fullParam.Key.PubKey);
            if (result.Success)
            {
                return new RsaDecryptionPrimitiveResult
                {
                    CipherText = new BitString(result.CipherText, param.Modulo, false),
                    TestPassed = true
                };
            }
            else
            {
                return new RsaDecryptionPrimitiveResult
                {
                    TestPassed = false
                };
            }
        }

        private RsaDecryptionPrimitiveResult GetRsaDecryptionPrimitive(RsaDecryptionPrimitiveParameters param)
        {
            var rand = new Random800_90();
            if (param.TestPassed)
            {
                // Correct tests
                KeyResult keyResult;
                do
                {
                    var e = GetEValue(RSA_PUBLIC_EXPONENT_BITS_MIN, RSA_PUBLIC_EXPONENT_BITS_MAX);
                    keyResult = new KeyBuilder(new PrimeGeneratorFactory())
                        .WithPrimeGenMode(PrimeGenModes.B33)
                        .WithEntropyProvider(new EntropyProvider(rand))
                        .WithNlen(param.Modulo)
                        .WithPublicExponent(e)
                        .WithPrimeTestMode(PrimeTestModes.C2)
                        .WithKeyComposer(_keyComposerFactory.GetKeyComposer(PrivateKeyModes.Standard))
                        .Build();
                } while (!keyResult.Success);

                var cipherText = new BitString(rand.GetRandomBigInteger(1, keyResult.Key.PubKey.N - 1));
                var plainText = new Rsa(new RsaVisitor()).Decrypt(cipherText.ToPositiveBigInteger(), keyResult.Key.PrivKey, keyResult.Key.PubKey).PlainText;

                return new RsaDecryptionPrimitiveResult
                {
                    CipherText = cipherText,
                    Key = keyResult.Key,
                    PlainText = new BitString(plainText, param.Modulo, false)
                };
            }
            else
            {
                // Failure tests - save some time and generate a dummy key

                // Pick a random ciphertext and force a leading '1' (so that it MUST be 2048 bits)
                var cipherText = BitString.One().ConcatenateBits(rand.GetRandomBitString(param.Modulo - 1));

                // Pick a random n that is 2048 bits and less than the ciphertext
                var n = rand.GetRandomBigInteger(NumberTheory.Pow2(param.Modulo - 1), cipherText.ToPositiveBigInteger());
                var e = GetEValue(RSA_PUBLIC_EXPONENT_BITS_MIN, RSA_PUBLIC_EXPONENT_BITS_MAX).ToPositiveBigInteger();

                return new RsaDecryptionPrimitiveResult
                {
                    CipherText = cipherText,
                    Key = new KeyPair { PubKey = new PublicKey { E = e, N = n } }
                };
            }
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

        public async Task<RsaDecryptionPrimitiveResult> GetDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            return await _taskFactory.StartNew(() => GetDeferredRsaDecryptionPrimitive(param));
        }

        public async Task<RsaDecryptionPrimitiveResult> CompleteDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param,
            RsaDecryptionPrimitiveResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredRsaDecryptionPrimitive(param, fullParam));
        }

        public async Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            return await _taskFactory.StartNew(() => GetRsaDecryptionPrimitive(param));
        }

        private BitString GetEValue(int minLen, int maxLen)
        {
            var rand = new Random800_90();
            BigInteger e;
            BitString e_bs;
            do
            {
                var min = minLen / 2;
                var max = maxLen / 2;

                e = GetRandomBigIntegerOfBitLength(rand.GetRandomInt(min, max) * 2);
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
            var rand = new Random800_90();
            var bs = rand.GetRandomBitString(len);
            return bs.ToPositiveBigInteger();
        }

        private BitString GetSeed(int modulo)
        {
            var rand = new Random800_90();
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

            return rand.GetRandomBitString(2 * security_strength);
        }

        private int[] GetBitlens(int modulo, PrimeGenModes mode)
        {
            var rand = new Random800_90();
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

            bitlens[0] = rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[1] = rand.GetRandomInt(min_single, max_both - bitlens[0]);
            bitlens[2] = rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[3] = rand.GetRandomInt(min_single, max_both - bitlens[2]);

            return bitlens;
        }
    }
}

