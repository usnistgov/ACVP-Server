using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Aead
{
    public class GcmBlockCipher : IAeadModeBlockCipher
    {
        private const int BITS_IN_BYTE = 8;
        private readonly IBlockCipherEngine _engine;
        private readonly IModeBlockCipherFactory _factory;
        private readonly IAES_GCMInternals _gcmInternals;

        public const string INVALID_TAG_MESSAGE = "Tags do not match";

        public GcmBlockCipher(
            IBlockCipherEngine engine,
            IModeBlockCipherFactory factory,
            IAES_GCMInternals gcmInternals)
        {
            // GCM only valid for AES
            if (engine.BlockSizeBits != 128)
            {
                throw new ArgumentException("GCM Mode only valid with AES Engine");
            }

            _engine = engine;
            _factory = factory;
            _gcmInternals = gcmInternals;
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
            var ecbParams = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                param.Key,
                new BitString(_engine.BlockSizeBits)
            );
            var ecb = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);
            var h = ecb.ProcessPayload(ecbParams);

            var j0 = _gcmInternals.Getj0(h.Result, param.Iv);
            var cipherText = _gcmInternals.GCTR(_gcmInternals.inc_s(32, j0), param.Payload, param.Key);
            int u = _engine.BlockSizeBits * cipherText.BitLength.CeilingDivide(_engine.BlockSizeBits) -
                    cipherText.BitLength;
            int v = _engine.BlockSizeBits *
                    param.AdditionalAuthenticatedData.BitLength.CeilingDivide(_engine.BlockSizeBits) -
                    param.AdditionalAuthenticatedData.BitLength;
            var encryptedBits =
                param.AdditionalAuthenticatedData.ConcatenateBits(new BitString(v))
                    .ConcatenateBits(cipherText)
                    .ConcatenateBits(new BitString(u))
                    .ConcatenateBits(BitString.To64BitString(param.AdditionalAuthenticatedData.BitLength))
                    .ConcatenateBits(BitString.To64BitString(cipherText.BitLength));
            var s = _gcmInternals.GHash(h.Result, encryptedBits);
            var tag = _gcmInternals.GCTR(j0, s, param.Key).GetMostSignificantBits(param.TagLength);
            return new SymmetricCipherAeadResult(cipherText, tag);
        }

        private SymmetricCipherAeadResult Decrypt(IAeadModeBlockCipherParameters param)
        {
            var ecbParams = new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                param.Key,
                new BitString(_engine.BlockSizeBits)
            );
            var ecb = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);
            var h = ecb.ProcessPayload(ecbParams);
            var j0 = _gcmInternals.Getj0(h.Result, param.Iv);

            var plainText = _gcmInternals.GCTR(_gcmInternals.inc_s(32, j0), param.Payload, param.Key);

            int u = 128 * plainText.BitLength.CeilingDivide(128) - plainText.BitLength;
            int v = 128 * param.AdditionalAuthenticatedData.BitLength.CeilingDivide(128) - param.AdditionalAuthenticatedData.BitLength;

            var decryptedBits =
                param.AdditionalAuthenticatedData.ConcatenateBits(new BitString(v))
                    .ConcatenateBits(param.Payload)
                    .ConcatenateBits(new BitString(u))
                    .ConcatenateBits(BitString.To64BitString(param.AdditionalAuthenticatedData.BitLength))
                    .ConcatenateBits(BitString.To64BitString(param.Payload.BitLength));

            var s = _gcmInternals.GHash(h.Result, decryptedBits);
            var tagPrime = _gcmInternals.GCTR(j0, s, param.Key).GetMostSignificantBits(param.Tag.BitLength);
            if (!param.Tag.Equals(tagPrime))
            {
                return new SymmetricCipherAeadResult(INVALID_TAG_MESSAGE);
            }

            return new SymmetricCipherAeadResult(plainText);
        }
    }
}
