using System;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.SHA2.SHAProperties;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA2
{
    public class SHA1 : ISHABase
    {
        private readonly uint[] _k, _h;
        private uint[] _w;
        private uint _a, _b, _c, _d, _e;

        private readonly SHAInternals _shaInternals;
        private readonly SHAPropertiesBase _shaProperties;

        public SHA1(SHAInternals shaInternals)
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

            // The rest of the words are an expansion of the previous
            for (var i = 16; i < _shaProperties.Rounds; i++)
            {
                _w[i] = CS1(_w[i - 3] ^ _w[i - 8] ^ _w[i - 14] ^ _w[i - 16]);
            }

            _a = _h[0];
            _b = _h[1];
            _c = _h[2];
            _d = _h[3];
            _e = _h[4];

            Round1(_w[0]);  Round1(_w[1]);  Round1(_w[2]);  Round1(_w[3]);
            Round1(_w[4]);  Round1(_w[5]);  Round1(_w[6]);  Round1(_w[7]);
            Round1(_w[8]);  Round1(_w[9]);  Round1(_w[10]); Round1(_w[11]);
            Round1(_w[12]); Round1(_w[13]); Round1(_w[14]); Round1(_w[15]);
            Round1(_w[16]); Round1(_w[17]); Round1(_w[18]); Round1(_w[19]);

            Round2(_w[20]); Round2(_w[21]); Round2(_w[22]); Round2(_w[23]);
            Round2(_w[24]); Round2(_w[25]); Round2(_w[26]); Round2(_w[27]);
            Round2(_w[28]); Round2(_w[29]); Round2(_w[30]); Round2(_w[31]);
            Round2(_w[32]); Round2(_w[33]); Round2(_w[34]); Round2(_w[35]);
            Round2(_w[36]); Round2(_w[37]); Round2(_w[38]); Round2(_w[39]);

            Round3(_w[40]); Round3(_w[41]); Round3(_w[42]); Round3(_w[43]);
            Round3(_w[44]); Round3(_w[45]); Round3(_w[46]); Round3(_w[47]);
            Round3(_w[48]); Round3(_w[49]); Round3(_w[50]); Round3(_w[51]);
            Round3(_w[52]); Round3(_w[53]); Round3(_w[54]); Round3(_w[55]);
            Round3(_w[56]); Round3(_w[57]); Round3(_w[58]); Round3(_w[59]);

            Round4(_w[60]); Round4(_w[61]); Round4(_w[62]); Round4(_w[63]);
            Round4(_w[64]); Round4(_w[65]); Round4(_w[66]); Round4(_w[67]);
            Round4(_w[68]); Round4(_w[69]); Round4(_w[70]); Round4(_w[71]);
            Round4(_w[72]); Round4(_w[73]); Round4(_w[74]); Round4(_w[75]);
            Round4(_w[76]); Round4(_w[77]); Round4(_w[78]); Round4(_w[79]);

            _h[0] += _a;
            _h[1] += _b;
            _h[2] += _c;
            _h[3] += _d;
            _h[4] += _e;
        }

        private void Round1(uint w)
        {
            var intermediate = CS5(_a) + ((_b & (_c ^ _d)) ^ _d) + _e + w + _k[0];

            _e = _d;
            _d = _c;
            _c = CS30(_b);
            _b = _a;
            _a = intermediate;
        }

        private void Round2(uint w)
        {
            var intermediate = CS5(_a) + (_b ^ _c ^ _d) + _e + w + _k[1];

            _e = _d;
            _d = _c;
            _c = CS30(_b);
            _b = _a;
            _a = intermediate;
        }

        private void Round3(uint w)
        {
            var intermediate = CS5(_a) + ((_b & _c) | (_b & _d) | (_c & _d)) + _e + w + _k[2];

            _e = _d;
            _d = _c;
            _c = CS30(_b);
            _b = _a;
            _a = intermediate;
        }

        private void Round4(uint w)
        {
            var intermediate = CS5(_a) + (_b ^ _c ^ _d) + _e + w + _k[3];

            _e = _d;
            _d = _c;
            _c = CS30(_b);
            _b = _a;
            _a = intermediate;
        }

        private uint CS1(uint x)
        {
            return x << 1 | x >> 31;
        }

        private uint CS5(uint x)
        {
            return x << 5 | x >> 27;
        }

        private uint CS30(uint x)
        {
            return x << 30 | x >> 2;
        }
    }
}
