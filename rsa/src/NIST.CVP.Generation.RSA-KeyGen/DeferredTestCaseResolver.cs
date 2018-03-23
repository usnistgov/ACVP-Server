using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, KeyResult>
    {
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IShaFactory _shaFactory;

        public DeferredTestCaseResolver(IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory, IShaFactory shaFactory)
        {
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _shaFactory = shaFactory;
        }

        public KeyResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            // TODO Not every group has a hash alg... Can use a default value perhaps?
            ISha sha = null;
            if (serverTestGroup.HashAlg != null)
            {
                sha = _shaFactory.GetShaInstance(serverTestGroup.HashAlg);
            }
            
            var keyComposer = _keyComposerFactory.GetKeyComposer(serverTestGroup.KeyFormat);
            
            BigInteger e;
            if (serverTestGroup.PubExp == PublicExponentModes.Fixed)
            {
                e = serverTestGroup.FixedPubExp.ToPositiveBigInteger();
            }
            else
            {
                e = serverTestCase.Key.PubKey.E == 0 ? iutTestCase.Key.PubKey.E : serverTestCase.Key.PubKey.E;
            }

            var entropyProvider = new TestableEntropyProvider();
            LoadEntropy(serverTestGroup.PrimeGenMode, serverTestGroup.Modulo, iutTestCase, entropyProvider);

            return _keyBuilder
                .WithHashFunction(sha)
                .WithBitlens(iutTestCase.Bitlens)
                .WithKeyComposer(keyComposer)
                .WithNlen(serverTestGroup.Modulo)
                .WithPrimeGenMode(serverTestGroup.PrimeGenMode)
                .WithPrimeTestMode(serverTestGroup.PrimeTest)
                .WithPublicExponent(e)
                .WithEntropyProvider(entropyProvider)
                .WithSeed(iutTestCase.Seed)
                .Build();
        }

        private void LoadEntropy(PrimeGenModes primeGenMode, int modulo, TestCase iutTestCase, TestableEntropyProvider entropyProvider)
        {
            if (primeGenMode == PrimeGenModes.B32)
            {
                // Nothing
            }
            else if (primeGenMode == PrimeGenModes.B33)
            {
                // P and Q
                entropyProvider.AddEntropy(new BitString(iutTestCase.Key.PrivKey.P, modulo / 2));
                entropyProvider.AddEntropy(new BitString(iutTestCase.Key.PrivKey.Q, modulo / 2));
            }
            else if (primeGenMode == PrimeGenModes.B34)
            {
                // Nothing
            }
            else if (primeGenMode == PrimeGenModes.B35)
            {
                // XP and XQ
                entropyProvider.AddEntropy(iutTestCase.XP.ToPositiveBigInteger());
                entropyProvider.AddEntropy(iutTestCase.XQ.ToPositiveBigInteger());
            }
            else if (primeGenMode == PrimeGenModes.B36)
            {
                // XP and XQ
                entropyProvider.AddEntropy(iutTestCase.XP.ToPositiveBigInteger());
                entropyProvider.AddEntropy(iutTestCase.XQ.ToPositiveBigInteger());

                // XP1, XP2, XQ1, XQ2
                entropyProvider.AddEntropy(iutTestCase.XP1.GetLeastSignificantBits(iutTestCase.Bitlens[0]));
                entropyProvider.AddEntropy(iutTestCase.XP2.GetLeastSignificantBits(iutTestCase.Bitlens[1]));
                entropyProvider.AddEntropy(iutTestCase.XQ1.GetLeastSignificantBits(iutTestCase.Bitlens[2]));
                entropyProvider.AddEntropy(iutTestCase.XQ2.GetLeastSignificantBits(iutTestCase.Bitlens[3]));
            }
        }
    }
}
