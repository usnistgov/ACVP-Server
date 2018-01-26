using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.Common.Hash.SHA2.SHAProperties;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA2
{
    public class SHA2Large : ISHABase
    {
        // Internal state variables
        private readonly ulong[] _k, _h;
        private ulong[] _w;
        
        private readonly SHAInternals _shaInternals;
        private readonly SHAPropertiesBase _shaProperties;

        public SHA2Large(SHAInternals shaInternals)
        {
            _shaInternals = shaInternals;
            _shaProperties = _shaInternals.SHAProperties;

            _k = _shaProperties.KValues.Select(k => Convert.ToUInt64(k, 16)).ToArray();
            _h = _shaProperties.HValues.Select(h => Convert.ToUInt64(h, 16)).ToArray();
        }

        public BitString HashMessage(BitString message)
        {
            var paddedMessage = PadMessage(message);
            var chunks = _shaInternals.Chunkify(paddedMessage);

            foreach (var chunk in chunks)
            {
                ProcessBlock(chunk);
            }

            return _shaInternals.BuildResult(_h.Select(bs => new BitString(bs, 64)).ToArray());
        }

        private BitString PadMessage(BitString message)
        {
            var bitLength = message.BitLength;
            message = message.ConcatenateBits(BitString.One());

            var bitsNeeded = ((((_shaProperties.BlockSize - 128) - message.BitLength) % _shaProperties.BlockSize) + _shaProperties.BlockSize) % _shaProperties.BlockSize;
            message = message.ConcatenateBits(BitString.Zeroes(bitsNeeded));

            // Maybe this could be changed to append a 128-bit integer, rather than 64 0-bits and then a 64-bit integer
            message = message.ConcatenateBits(BitString.Zeroes(64));
            return message.ConcatenateBits(BitString.To64BitString(bitLength));
        }

        private void ProcessBlock(BitString chunk)
        {
            var a = _h[0];
            var b = _h[1];
            var c = _h[2];
            var d = _h[3];
            var e = _h[4];
            var f = _h[5];
            var g = _h[6];
            var h = _h[7];

            var bytes = chunk.ToBytes();
            _w = new ulong[_shaProperties.Rounds];

            // Split each chunk into 16, 64-bit words
            for (var i = 0; i < 16; i++)
            {
                _w[i] = ((ulong)bytes[i * 8 + 0] & 255) << 56 |
                        ((ulong)bytes[i * 8 + 1] & 255) << 48 |
                        ((ulong)bytes[i * 8 + 2] & 255) << 40 |
                        ((ulong)bytes[i * 8 + 3] & 255) << 32 |
                        ((ulong)bytes[i * 8 + 4] & 255) << 24 |
                        ((ulong)bytes[i * 8 + 5] & 255) << 16 |
                        ((ulong)bytes[i * 8 + 6] & 255) << 8  |
                        ((ulong)bytes[i * 8 + 7] & 255);
            }

            for (var i = 16; i < _shaProperties.Rounds; i++)
            {
                _w[i] = _w[i - 16] + _w[i - 7] + 
                        (RightRotate(_w[i - 15], 1) ^ RightRotate(_w[i - 15], 8) ^ RightShift(_w[i - 15], 7)) + 
                        (RightRotate(_w[i - 2], 19) ^ RightRotate(_w[i - 2], 61) ^ RightShift(_w[i - 2], 6));
            }

            for (var i = 0; i < _shaProperties.Rounds; i++)
            {
                var sum1 = RightRotate(e, 14) ^ RightRotate(e, 18) ^ RightRotate(e, 41);
                var ch = (e & f) ^ (~e & g);
                var temp = h + sum1 + ch + _k[i] + _w[i];
                var temp2 = (RightRotate(a, 28) ^ RightRotate(a, 34) ^ RightRotate(a, 39)) + ((a & b) ^ (a & c) ^ (b & c));

                h = g;
                g = f;
                f = e;
                e = d + temp;
                d = c;
                c = b;
                b = a;
                a = temp + temp2;
            }

            _h[0] += a;
            _h[1] += b;
            _h[2] += c;
            _h[3] += d;
            _h[4] += e;
            _h[5] += f;
            _h[6] += g;
            _h[7] += h;
        }

        private ulong RightShift(ulong x, int amount)
        {
            return x >> amount;
        }

        private ulong RightRotate(ulong x, int amount)
        {
            return x >> amount | x << (64 - amount);
        }
    }
}
