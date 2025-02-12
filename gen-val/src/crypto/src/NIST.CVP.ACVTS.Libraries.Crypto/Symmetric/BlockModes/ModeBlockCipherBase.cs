﻿using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes
{
    public abstract class ModeBlockCipherBase<TSymmetricCipherResult> : IModeBlockCipher<TSymmetricCipherResult>
        where TSymmetricCipherResult : IModeBlockCipherResult
    {
        protected int BitsInByte => 8;
        protected IBlockCipherEngine _engine;

        protected ModeBlockCipherBase(IBlockCipherEngine engine)
        {
            _engine = engine;
        }

        public abstract bool IsPartialBlockAllowed { get; }
        public abstract TSymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param);

        /// <summary>
        /// Checks that when partial blocks are not allowed, that the payload meets the block size
        /// </summary>
        /// <param name="payLoad"></param>
        protected virtual void CheckPayloadRequirements(BitString payLoad)
        {
            if (!IsPartialBlockAllowed)
            {
                if (payLoad.BitLength % (_engine.BlockSizeBytes / BitsInByte) != 0)
                {
                    throw new ArgumentException($"{nameof(payLoad)} must equal the Engine's block size");
                }
            }
        }

        /// <summary>
        /// Determine's number of blocks to process
        /// </summary>
        /// <param name="outputLengthBits">The requested output length</param>
        /// <returns></returns>
        protected virtual int GetNumberOfBlocks(int outputLengthBits)
        {
            var numberOfBlocks = outputLengthBits / (_engine.BlockSizeBytes * BitsInByte);

            // In cases of partial block, add an additional block for processing.
            if (outputLengthBits % _engine.BlockSizeBits != 0)
            {
                numberOfBlocks++;
            }

            return numberOfBlocks;
        }

        /// <summary>
        /// Get a byte array that will support the output's length
        /// </summary>
        /// <param name="outputLengthInBits">The output length</param>
        /// <returns></returns>
        protected virtual byte[] GetOutputBuffer(int outputLengthInBits)
        {
            var byteLength = GetNumberOfBlocks(outputLengthInBits) * _engine.BlockSizeBytes;

            return new byte[byteLength];
        }
    }
}
