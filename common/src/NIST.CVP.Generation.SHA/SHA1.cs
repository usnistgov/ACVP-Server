using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA
{
    public class SHA1 : ISHA
    {
        private BitString[] _k;
        private BitString[] _h;
        private BitString _a, _b, _c, _d, _e;
        private BitString[] _w;

        private SHAInternals _shaInternals;
        private SHAPropertiesBase _shaProperties;

        public SHA1(SHAInternals shaInternals)
        {
            _shaInternals = shaInternals;
            _shaProperties = _shaInternals.SHAProperties;
        }

        public BitString HashMessage(BitString message)
        {
            _k = _shaProperties.KValues;
            _h = _shaProperties.HValues;

            var paddedMessage = _shaInternals.PreProcessing(message);
            var chunks = _shaInternals.Chunkify(paddedMessage);
            
            foreach (var chunk in chunks)
            {
                DivideChunk(chunk);
                ProcessBlock();
            }

            return _shaInternals.BuildResult(_h);
        }

        private void DivideChunk(BitString chunk)
        {
            _w = new BitString[_shaProperties.Rounds];

            // Split each chunk into 16, 32-bit words
            for (var i = 0; i < 16; i++)
            {
                _w[i] = chunk.MSBSubstring(i * _shaProperties.WordSize, _shaProperties.WordSize);
            }

            // The rest of the words are an expansion of the previous
            for (var i = 16; i < _shaProperties.Rounds; i++)
            {
                var intermediate = _w[i - 3].GetDeepCopy();
                intermediate = BitString.XOR(intermediate, _w[i - 8]);
                intermediate = BitString.XOR(intermediate, _w[i - 14]);
                intermediate = BitString.XOR(intermediate, _w[i - 16]);

                _w[i] = CS1(intermediate);
            }
        }

        private void ProcessBlock()
        {
            _a = _h[0].GetDeepCopy();
            _b = _h[1].GetDeepCopy();
            _c = _h[2].GetDeepCopy();
            _d = _h[3].GetDeepCopy();
            _e = _h[4].GetDeepCopy();

            // Perform Rounds
            for (var i = 0; i < 20; i++)
            {
                Round1(i);
            }

            for (var i = 20; i < 40; i++)
            {
                Round2(i);
            }

            for (var i = 40; i < 60; i++)
            {
                Round3(i);
            }

            for (var i = 60; i < 80; i++)
            {
                Round4(i);
            }

            // Increment H Values wrapped to remain within 2^32
            _h[0] = BitString.AddWithModulo(_h[0], _a, _shaProperties.WordSize);
            _h[1] = BitString.AddWithModulo(_h[1], _b, _shaProperties.WordSize);
            _h[2] = BitString.AddWithModulo(_h[2], _c, _shaProperties.WordSize);
            _h[3] = BitString.AddWithModulo(_h[3], _d, _shaProperties.WordSize);
            _h[4] = BitString.AddWithModulo(_h[4], _e, _shaProperties.WordSize);
        }

        #region F Functions
        private BitString F1(BitString x, BitString y, BitString z)
        {
            // (x & (y ^ z)) ^ z
            return BitString.XOR(BitString.AND(x, BitString.XOR(y, z)), z);
        }

        private BitString F2(BitString x, BitString y, BitString z)
        {
            // x ^ y ^ z
            return BitString.XOR(BitString.XOR(x, y), z);
        }

        private BitString F3(BitString x, BitString y, BitString z)
        {
            // (x & y) | (x & z) | (y & z)
            var firstAND = BitString.AND(x, y);
            var secondAND = BitString.AND(x, z);
            var thirdAND = BitString.AND(y, z);
            return BitString.OR(firstAND, BitString.OR(secondAND, thirdAND));
        }
        #endregion F Functions

        #region Rounds
        private void Round1(int count)
        {
            var intermediate = CS5(_a);
            intermediate = BitString.AddWithModulo(intermediate, F1(_b, _c, _d), 32);
            intermediate = BitString.AddWithModulo(intermediate, _e, 32);
            intermediate = BitString.AddWithModulo(intermediate, _w[count], 32);
            intermediate = BitString.AddWithModulo(intermediate, _k[0], 32);

            _e = _d.GetDeepCopy();
            _d = _c.GetDeepCopy();
            _c = CS30(_b);
            _b = _a.GetDeepCopy();
            _a = intermediate;
        }

        private void Round2(int count)
        {
            var intermediate = CS5(_a);
            intermediate = BitString.AddWithModulo(intermediate, F2(_b, _c, _d), 32);
            intermediate = BitString.AddWithModulo(intermediate, _e, 32);
            intermediate = BitString.AddWithModulo(intermediate, _w[count], 32);
            intermediate = BitString.AddWithModulo(intermediate, _k[1], 32);

            _e = _d.GetDeepCopy();
            _d = _c.GetDeepCopy();
            _c = CS30(_b);
            _b = _a.GetDeepCopy();
            _a = intermediate;
        }

        private void Round3(int count)
        {
            var intermediate = CS5(_a);
            intermediate = BitString.AddWithModulo(intermediate, F3(_b, _c, _d), 32);
            intermediate = BitString.AddWithModulo(intermediate, _e, 32);
            intermediate = BitString.AddWithModulo(intermediate, _w[count], 32);
            intermediate = BitString.AddWithModulo(intermediate, _k[2], 32);

            _e = _d.GetDeepCopy();
            _d = _c.GetDeepCopy();
            _c = CS30(_b);
            _b = _a.GetDeepCopy();
            _a = intermediate;
        }

        private void Round4(int count)
        {
            var intermediate = CS5(_a);
            intermediate = BitString.AddWithModulo(intermediate, F2(_b, _c, _d), 32);
            intermediate = BitString.AddWithModulo(intermediate, _e, 32);
            intermediate = BitString.AddWithModulo(intermediate, _w[count], 32);
            intermediate = BitString.AddWithModulo(intermediate, _k[3], 32);

            _e = _d.GetDeepCopy();
            _d = _c.GetDeepCopy();
            _c = CS30(_b);
            _b = _a.GetDeepCopy();
            _a = intermediate;
        }
        #endregion Rounds

        #region Circular Shifts
        // Aliases for consistency with documentation
        private BitString CS1(BitString x)
        {
            return BitString.MSBRotate(x, 1);
        }

        private BitString CS5(BitString x)
        {
            return BitString.MSBRotate(x, 5);
        }

        private BitString CS30(BitString x)
        {
            return BitString.MSBRotate(x, 30);
        }
        #endregion
    }
}
