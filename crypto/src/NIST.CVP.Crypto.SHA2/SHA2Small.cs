using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.SHA2.SHAProperties;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA2
{
    public class SHA2Small : ISHABase
    {
        // Internal state variables
        private readonly uint[] _k, _h;
        private uint[] _w;
        
        private readonly SHAInternals _shaInternals;
        private readonly SHAPropertiesBase _shaProperties;

        public SHA2Small(SHAInternals shaInternals)
        {
            _shaInternals = shaInternals;
            _shaProperties = _shaInternals.SHAProperties;

            _k = _shaProperties.KValues.Select(k => Convert.ToUInt32(k, 16)).ToArray();
            _h = _shaProperties.HValues.Select(h => Convert.ToUInt32(h, 16)).ToArray();
        }

        public BitString HashMessage(BitString message)
        {
            var paddedMessage = PadMessage(message);
            var chunks = _shaInternals.Chunkify(paddedMessage);

            foreach (var chunk in chunks)
            {
                ProcessBlock(chunk);
            }

            return _shaInternals.BuildResult(_h.Select(bs => new BitString(bs, 32)).ToArray());
        }

        private BitString PadMessage(BitString message)
        {
            var bitLength = message.BitLength;
            message = message.ConcatenateBits(BitString.One());

            var bitsNeeded = ((((_shaProperties.BlockSize - 64) - message.BitLength) % _shaProperties.BlockSize) + _shaProperties.BlockSize) % _shaProperties.BlockSize;
            message = message.ConcatenateBits(BitString.Zeroes(bitsNeeded));

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
            _w = new uint[_shaProperties.Rounds];

            // Split each chunk into 16, 32-bit words
            for (var i = 0; i < 16; i++)
            {
                _w[i] = ((uint)bytes[i * 4 + 0] & 255) << 24 |
                        ((uint)bytes[i * 4 + 1] & 255) << 16 |
                        ((uint)bytes[i * 4 + 2] & 255) << 8  |
                        ((uint)bytes[i * 4 + 3] & 255);
            }

            for (var i = 16; i < _shaProperties.Rounds; i++)
            {
                _w[i] = _w[i - 16] + _w[i - 7] + 
                        (RightRotate(_w[i - 15], 7) ^ RightRotate(_w[i - 15], 18) ^ RightShift(_w[i - 15], 3)) + 
                        (RightRotate(_w[i - 2], 17) ^ RightRotate(_w[i - 2], 19) ^ RightShift(_w[i - 2], 10));
            }

            for (var i = 0; i < _shaProperties.Rounds; i++)
            {
                var sum1 = RightRotate(e, 6) ^ RightRotate(e, 11) ^ RightRotate(e, 25);
                var ch = (e & f) ^ (~e & g);
                var temp = h + sum1 + ch + _k[i] + _w[i];
                var temp2 = (RightRotate(a, 2) ^ RightRotate(a, 13) ^ RightRotate(a, 22)) + ((a & b) ^ (a & c) ^ (b & c));

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

        private uint RightShift(uint x, int amount)
        {
            return x >> amount;
        }

        private uint RightRotate(uint x, int amount)
        {
            return x >> amount | x << (32 - amount);
        }
    }
}
