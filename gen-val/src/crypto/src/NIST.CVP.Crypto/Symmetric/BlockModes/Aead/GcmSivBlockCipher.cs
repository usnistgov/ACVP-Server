using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System;

namespace NIST.CVP.Crypto.Symmetric.BlockModes.Aead
{
    public class GcmSivBlockCipher : IAeadModeBlockCipher
    {
        private const int BITS_IN_BYTE = 8;
        private readonly IBlockCipherEngine _engine;
        private readonly IModeBlockCipherFactory _factory;
        private readonly IAES_GCMInternals _gcmInternals;

        private readonly BitString _gHashConverter = new BitString("40000000000000000000000000000000");
        private readonly BitString _polynomial = new BitString("010000000000000000000000000000C2");
        private readonly BitString _inverse = new BitString("01000000000000000000000000000492");

        public const string INVALID_TAG_MESSAGE = "Tags do not match";

        public GcmSivBlockCipher(
            IBlockCipherEngine engine,
            IModeBlockCipherFactory factory,
            IAES_GCMInternals gcmInternals)
        {
            // GCM-SIV only valid for AES
            if (engine.BlockSizeBits != 128)
            {
                throw new ArgumentException("GCM-SIV Mode only valid with AES Engine");
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
            // Artificial limit. Specification has 2^36, but testing doesn't need that length.
            if (param.Payload.BitLength > 65536)
            {
                return new SymmetricCipherAeadResult("Plaintext too long to encrypt. Must be under 65536 bits");
            }

            if (param.Payload.BitLength % 8 != 0)
            {
                return new SymmetricCipherAeadResult("Plaintext must be on a byte boundary");
            }

            // Artificial limit. Specification has 2^36, but testing doesn't need that length.
            if (param.AdditionalAuthenticatedData.BitLength > 65536)
            {
                return new SymmetricCipherAeadResult("AAD too long to encrypt. Must be under 65536 bits");
            }

            if (param.AdditionalAuthenticatedData.BitLength % 8 != 0)
            {
                return new SymmetricCipherAeadResult("AAD must be on a byte boundary");
            }

            var (messageAuthKey, messageEncKey) = DeriveKeys(param.Key, param.Iv);

            var lengthBlock = LittleEndianify(BitString.To64BitString(param.AdditionalAuthenticatedData.BitLength))
                .ConcatenateBits(LittleEndianify(BitString.To64BitString(param.Payload.BitLength)));

            var paddedPlaintext = RightPadToMultipleOf16Bytes(param.Payload);
            var paddedAad = RightPadToMultipleOf16Bytes(param.AdditionalAuthenticatedData);
            var polyValInput = paddedAad.ConcatenateBits(paddedPlaintext).ConcatenateBits(lengthBlock);
            var S_s = PolyVal(messageAuthKey, polyValInput).ToBytes();
            var nonceBytes = param.Iv.ToBytes();

            for (var i = 0; i < 12; i++)
            {
                S_s[i] ^= nonceBytes[i];
            }

            S_s[15] &= 0x7f;
            var S_sBitString = new BitString(S_s);

            if (S_sBitString.BitLength % _engine.BlockSizeBits != 0)
            {
                return new SymmetricCipherAeadResult("S BitString was not a multiple of the block size");
            }

            var ecb = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);
            var tag = ecb.ProcessPayload
            (
                new ModeBlockCipherParameters
                (
                    BlockCipherDirections.Encrypt,
                    messageEncKey,
                    S_sBitString
                )
            );

            if (!tag.Success)
            {
                return new SymmetricCipherAeadResult("Unable to create tag");
            }

            var counterBlock = tag.Result.ToBytes();
            counterBlock[15] |= 0x80;
            var counterBitString = new BitString(counterBlock);
            var result = AesCtr(messageEncKey, counterBitString, param.Payload);

            return new SymmetricCipherAeadResult(result.ConcatenateBits(tag.Result));
        }

        private SymmetricCipherAeadResult Decrypt(IAeadModeBlockCipherParameters param)
        {
            if (param.Payload.BitLength < 16 || param.Payload.BitLength > 65536 + 16)
            {
                return new SymmetricCipherAeadResult("Ciphertext is too long or too short.");
            }

            if (param.AdditionalAuthenticatedData.BitLength > 65536)
            {
                return new SymmetricCipherAeadResult("AAD is too long");
            }

            var (messageAuthKey, messageEncKey) = DeriveKeys(param.Key, param.Iv);
            var tag = param.Payload.Substring(0, 16 * 8);
            var cipherText = new BitString(0);

            if (tag.BitLength != param.Payload.BitLength)
            {
                cipherText = param.Payload.MSBSubstring(0, param.Payload.BitLength - tag.BitLength);
            }

            var counterBlock = tag.ToBytes();
            counterBlock[15] |= 0x80;
            var counterBitString = new BitString(counterBlock);

            if (cipherText.BitLength % 8 != 0)
            {
                return new SymmetricCipherAeadResult("Ciphertext was not a multiple of bytes");
            }

            var plainText = AesCtr(messageEncKey, counterBitString, cipherText);

            var lengthBlock = LittleEndianify(BitString.To64BitString(param.AdditionalAuthenticatedData.BitLength))
                .ConcatenateBits(LittleEndianify(BitString.To64BitString(plainText.BitLength)));

            var paddedPlaintext = RightPadToMultipleOf16Bytes(plainText);
            var paddedAad = RightPadToMultipleOf16Bytes(param.AdditionalAuthenticatedData);
            var polyValInput = paddedAad.ConcatenateBits(paddedPlaintext).ConcatenateBits(lengthBlock);
            var S_s = PolyVal(messageAuthKey, polyValInput).ToBytes();
            var nonceBytes = param.Iv.ToBytes();

            for (var i = 0; i < 12; i++)
            {
                S_s[i] ^= nonceBytes[i];
            }

            S_s[15] &= 0x7f;
            var S_sBitString = new BitString(S_s);

            if (S_sBitString.BitLength % _engine.BlockSizeBits != 0)
            {
                return new SymmetricCipherAeadResult("S BitString was not a multiple of the block size");
            }

            var ecb = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);
            var expectedTag = ecb.ProcessPayload
            (
                new ModeBlockCipherParameters
                (
                    BlockCipherDirections.Encrypt,
                    messageEncKey,
                    S_sBitString
                )
            );

            return expectedTag.Result.Equals(tag) ? new SymmetricCipherAeadResult(plainText) : new SymmetricCipherAeadResult("Tags do not match");
        }

        // public for testing purposes
        public (BitString messageAuthKey, BitString messageEncKey) DeriveKeys(BitString keyGeneratingKey, BitString nonce)
        {
            var ecb = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb);

            #region MessageAuthKey
            var messageAuthKey1 = ecb.ProcessPayload
            (
                new ModeBlockCipherParameters
                (
                    BlockCipherDirections.Encrypt,
                    keyGeneratingKey,
                    LittleEndianify(BitString.To32BitString(0)).ConcatenateBits(nonce)
                )
            );

            var messageAuthKey2 = ecb.ProcessPayload
            (
                new ModeBlockCipherParameters
                (
                    BlockCipherDirections.Encrypt,
                    keyGeneratingKey,
                    LittleEndianify(BitString.To32BitString(1)).ConcatenateBits(nonce)
                )
            );

            if (!messageAuthKey1.Success || !messageAuthKey2.Success)
            {
                throw new Exception("Unable to create messageAuthKey");
            }

            var messageAuthKey = messageAuthKey1.Result.MSBSubstring(0, 8 * BITS_IN_BYTE)
                .ConcatenateBits(messageAuthKey2.Result.MSBSubstring(0, 8 * BITS_IN_BYTE));
            #endregion MessageAuthKey

            #region MessageEncKey
            var messageEncKey1 = ecb.ProcessPayload
            (
                new ModeBlockCipherParameters
                (
                    BlockCipherDirections.Encrypt,
                    keyGeneratingKey,
                    LittleEndianify(BitString.To32BitString(2)).ConcatenateBits(nonce)
                )
            );

            var messageEncKey2 = ecb.ProcessPayload
            (
                new ModeBlockCipherParameters
                (
                    BlockCipherDirections.Encrypt,
                    keyGeneratingKey,
                    LittleEndianify(BitString.To32BitString(3)).ConcatenateBits(nonce)
                )
            );

            if (!messageEncKey1.Success || !messageEncKey2.Success)
            {
                throw new Exception("Unable to create messageEncKey");
            }

            var messageEncKey = messageEncKey1.Result.MSBSubstring(0, 8 * BITS_IN_BYTE)
                .ConcatenateBits(messageEncKey2.Result.MSBSubstring(0, 8 * BITS_IN_BYTE));
            #endregion MessageEncKey

            #region AddExtraMessageEncKeyBits
            if (keyGeneratingKey.BitLength == 32 * BITS_IN_BYTE)
            {
                var messageEncKey3 = ecb.ProcessPayload
                (
                    new ModeBlockCipherParameters
                    (
                        BlockCipherDirections.Encrypt,
                        keyGeneratingKey,
                        LittleEndianify(BitString.To32BitString(4)).ConcatenateBits(nonce)
                    )
                );

                var messageEncKey4 = ecb.ProcessPayload
                (
                    new ModeBlockCipherParameters
                    (
                        BlockCipherDirections.Encrypt,
                        keyGeneratingKey,
                        LittleEndianify(BitString.To32BitString(5)).ConcatenateBits(nonce)
                    )
                );

                if (!messageEncKey3.Success || !messageEncKey4.Success)
                {
                    throw new Exception("Unable to create messageEncKey");
                }

                messageEncKey = messageEncKey.ConcatenateBits(messageEncKey3.Result.MSBSubstring(0, 8 * BITS_IN_BYTE)
                    .ConcatenateBits(messageEncKey4.Result.MSBSubstring(0, 8 * BITS_IN_BYTE)));
            }
            #endregion AddExtraMessageEncKeyBits

            return (messageAuthKey, messageEncKey);
        }

        public BitString AesCtr(BitString key, BitString initialCounterBlock, BitString input)
        {
            var ecb = _factory.GetStandardCipher(_engine, BlockCipherModesOfOperation.Ecb); 
            var block = initialCounterBlock.GetDeepCopy();
            var output = new BitString(0);
            var iterations = (int)System.Math.Ceiling(input.BitLength / 128.0);
            for (var i = 0; i < iterations; i++)
            {
                var inputBlock = (i * 128 + 128 <= input.BitLength) ? input.MSBSubstring(i * 128, 128) : input.MSBSubstring(i * 128, input.BitLength % 128);

                if (block.BitLength != 128)
                {
                    throw new Exception($"\nBlock length is not a mulitple of 128.\nInput: {input.ToHex()}\nKey: {key.ToHex()}\nInitialCtr: {initialCounterBlock.ToHex()}");
                }

                var keyStreamBlock = ecb.ProcessPayload
                (
                    new ModeBlockCipherParameters
                    (
                        BlockCipherDirections.Encrypt,
                        key,
                        block
                    )
                );

                var beginOfBlock = block.MSBSubstring(0, 32);
                var endOfBlock = block.MSBSubstring(32, 96);
                block = beginOfBlock.BitStringAddition(new BitString("01000000")).ConcatenateBits(endOfBlock).Substring(0, 128);

                var inputXor = inputBlock.GetDeepCopy().ConcatenateBits(BitString.Zeroes(keyStreamBlock.Result.BitLength - inputBlock.BitLength));
                var xor = keyStreamBlock.Result.XOR(inputXor).MSBSubstring(0, inputBlock.BitLength);
                output = output.ConcatenateBits(xor);
            }

            return output;
        }

        public BitString PolyVal(BitString H, BitString X)
        {
            var invX = new BitString(0);
            for (var i = 0; i < X.BitLength / 128; i++)
            {
                invX = invX.ConcatenateBits(LittleEndianify(X.MSBSubstring(128 * i, 128)));
            }

            var reversedH = LittleEndianify(H);
            var invH = MulXGHash(reversedH);
            var gHash = _gcmInternals.GHash(invH, invX);

            return LittleEndianify(gHash);
        }

        public BitString MulXGHash(BitString x)
        {
            return _gcmInternals.BlockProduct(x, _gHashConverter);
        }

        private BitString LittleEndianify(BitString x)
        {
            return new BitString(MsbLsbConversionHelpers.ReverseByteOrder(x.ToBytes()));
        }

        private BitString RightPadToMultipleOf16Bytes(BitString x)
        {
            while (x.BitLength / BITS_IN_BYTE % 16 != 0)
            {
                x = x.ConcatenateBits(BitString.Zeroes(8));
            }

            return x;
        }
    }
}
