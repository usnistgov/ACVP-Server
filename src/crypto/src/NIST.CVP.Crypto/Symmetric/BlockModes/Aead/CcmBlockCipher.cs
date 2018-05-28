using System;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Symmetric.BlockModes.Aead
{
    public class CcmBlockCipher : IAeadModeBlockCipher
    {
        private const int BITS_IN_BYTE = 8;
        private readonly IBlockCipherEngine _engine;
        private readonly IModeBlockCipherFactory _factory;
        private readonly IAES_CCMInternals _ccmInternals;

        public const string INVALID_TAG_MESSAGE = "Tags do not match";

        public CcmBlockCipher(IBlockCipherEngine engine, IModeBlockCipherFactory factory, IAES_CCMInternals ccmInternals)
        {
            _engine = engine;
            _factory = factory;
            _ccmInternals = ccmInternals;
        }

        public bool IsPartialBlockAllowed => false;
        
        public SymmetricCipherAeadResult ProcessPayload(IAeadModeBlockCipherParameters param)
        {
            switch (param.Direction)
            {
                case BlockCipherDirections.Encrypt:
                    return Encrypt(param);
                case BlockCipherDirections.Decrypt:
                    return Decrypt(param);
                default:
                    throw new ArgumentException(nameof(param.Direction));
            }
        }

        private SymmetricCipherAeadResult Encrypt(IAeadModeBlockCipherParameters param)
        {
            byte[] b = null;
            byte[] ctr = new byte[_engine.BlockSizeBytes];
            int r = 0;

            _ccmInternals.CCM_format_80211(
                ref b,
                param.Iv.ToBytes(),
                param.Iv.BitLength,
                param.Payload.ToBytes(),
                param.Payload.BitLength,
                param.AdditionalAuthenticatedData.ToBytes(),
                param.AdditionalAuthenticatedData.BitLength,
                param.TagLength,
                ref ctr,
                ref r
            );

            // MAC b
            var cbcMacRijndael = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.CbcMac);
            var cbcMacParam = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                new BitString(_engine.BlockSizeBits),
                param.Key,
                new BitString(b)
            );
            var mac =
                new BitString(cbcMacRijndael.ProcessPayload(cbcMacParam)
                    .Result
                    .ToBytes()
                    .Take(_engine.BlockSizeBytes) // mac is the first block
                    .ToArray() 
                );

            // Encrypt Payload and MAC
            var counterRijndael = _factory.GetCounterCipher(
                _engine, 
                new AdditiveCounter(
                    _engine, 
                    new BitString(ctr)
                )
            );
            var counterParam = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                param.Key,
                mac
            );
            // Tag - made up of the most significant bits of the length of the tag
            var T = counterRijndael.ProcessPayload(counterParam)
                .Result
                .GetMostSignificantBits(param.TagLength);

            int m = (param.Payload.BitLength + (_engine.BlockSizeBits - 1)) / _engine.BlockSizeBits;
            var payLoadAtBlockSize = param.Payload.ConcatenateBits(
                new BitString((m * _engine.BlockSizeBits) - param.Payload.BitLength)
            );
            counterParam.Payload = payLoadAtBlockSize;
            // ct should be made up of the most significant bits of the length of the payload.
            BitString ct = m > 0
                ? counterRijndael.ProcessPayload(param).Result.GetMostSignificantBits(param.Payload.BitLength)
                : new BitString(0);

            // final return is made up of the CT contatenated with  T
            // TODO note right now CCM's ct return contains the tag as per CAVS testing
            return new SymmetricCipherAeadResult(ct.ConcatenateBits(T), T);
        }

        private SymmetricCipherAeadResult Decrypt(IAeadModeBlockCipherParameters param)
        {
            byte[] ctr = null;
            int r = 0;

            _ccmInternals.CCM_counter_80211(param.Iv.ToBytes(), param.Iv.BitLength, ref ctr);

            // Decrypt cipherText and MAC
            int plen = param.Payload.BitLength - param.TagLength;
            int m = (plen + (_engine.BlockSizeBits - 1)) / _engine.BlockSizeBits + 1; // the length of encrypted payload + 1 block for MAC

            var tagPortion = new BitString(
                param.Payload.ToBytes()
                .Skip(plen / BITS_IN_BYTE)
                .Take(param.TagLength / BITS_IN_BYTE)
                .ToArray()
            );
            var cipherTextPortion = new BitString(
                param.Payload.ToBytes().Take(plen / BITS_IN_BYTE).ToArray()
            );

            // tagPortion should be exactly 16 bytes, bitString should be ended on a block boundry
            byte[] ct = tagPortion
                .ConcatenateBits(
                    // add bits to hit the block byte boundry for tag
                    new BitString(_engine.BlockSizeBits - tagPortion.BitLength) 
                ) 
                .ConcatenateBits(cipherTextPortion)
                .ConcatenateBits(
                    new BitString(
                        // Add bits to hit a block boundry
                        (m * _engine.BlockSizeBits) - (_engine.BlockSizeBits + cipherTextPortion.BitLength)
                    )
                ) 
                .ToBytes();

            var counterRijndael = _factory.GetCounterCipher(
                _engine,
                new AdditiveCounter(_engine, new BitString(ctr))
            );
            var counterParam = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                param.Key,
                new BitString(ct)
            );

            var pt = counterRijndael.ProcessPayload(counterParam);

            // Payload starts at 16th byte of PT
            var payload = new BitString(pt.Result.ToBytes().Skip(16).ToArray());

            // The tag is the first Tlen/8 bytes of the PT
            var T = pt.Result.GetMostSignificantBits(param.TagLength);

            // Format the data
            byte[] b = null;
            _ccmInternals.CCM_format_80211(
                ref b, 
                param.Iv.ToBytes(),
                param.Iv.BitLength, 
                payload.ToBytes(), 
                plen,
                param.AdditionalAuthenticatedData.ToBytes(),
                param.AdditionalAuthenticatedData.BitLength, 
                param.TagLength, 
                ref ctr, 
                ref r
            );

            // Calculate the MAC
            BitString iv = new BitString(_engine.BlockSizeBits);
            var cbcMacRijndael = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.CbcMac);
            var macParam = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iv,
                param.Key,
                new BitString(b)
            );
            var mac =
                cbcMacRijndael.ProcessPayload(macParam);

            // Check calculated tag equals provided tag
            if (!mac.Result.GetMostSignificantBits(param.TagLength).Equals(
                T.GetMostSignificantBits(param.TagLength)
            ))
            {
                return new SymmetricCipherAeadResult(INVALID_TAG_MESSAGE);
            }

            return new SymmetricCipherAeadResult(payload);
        }
    }
}