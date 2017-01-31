using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using System.Numerics;
using NLog;

namespace NIST.CVP.Generation.SHA
{
    public class SHA2 : SHA
    {
        // Internal state variables
        private BitString[] _k;
        private BitString[] _h;
        private BitString[] _w;
        private BitString[] _temps;

        // For quick references
        private int[,] _sumShifts;
        private int[,] _sigmaShifts;

        private SHAInternals _shaInternals;
        private SHAPropertiesBase _shaProperties;

        public SHA2(SHAInternals shaInternals)
        {
            _shaInternals = shaInternals;
            _shaProperties = _shaInternals.SHAProperties;

            _sumShifts = _shaProperties.SumShifts;
            _sigmaShifts = _shaProperties.SigmaShifts;
        }

        public override BitString HashMessage(BitString message)
        {
            _k = _shaProperties.KValues;
            _h = _shaProperties.HValues;

            var paddedMessage = _shaInternals.PreProcessing(message);
            var chunks = _shaInternals.Chunkify(paddedMessage);

            foreach(var chunk in chunks)
            {
                _w = DivideIntoWords(chunk);
                ProcessBlock();
            }

            return _shaInternals.BuildResult(_h);
        }

        private void ProcessBlock()
        {
            // would be a-h but h is already used...
            _temps = new BitString[8];
            for(var i = 0; i < 8; i++)
            {
                _temps[i] = _h[i].GetDeepCopy();
            }

            for (var i = 0; i < _shaProperties.Rounds; i++)
            {
                // (e rightrotate 6) ^ (e rightrotate 13) ^ (e rightrotate 25)
                var sum1 = BitString.XOR(
                              BitString.XOR(
                                 BitString.LSBRotate(_temps[4], _sumShifts[1, 0]),
                                 BitString.LSBRotate(_temps[4], _sumShifts[1, 1])
                              ),
                              BitString.LSBRotate(_temps[4], _sumShifts[1, 2])
                           );

                // (e & f) ^ (!e and g)
                var ch = BitString.XOR(
                            BitString.AND(_temps[4], _temps[5]),
                            BitString.AND(
                                BitString.NOT(_temps[4]), 
                                _temps[6]
                            )
                         );

                // h + s1 + ch + k[i] + w[i]
                var temp1 = BitString.AddWithModulo(_temps[7], sum1, _shaProperties.WordSize);
                temp1 = BitString.AddWithModulo(temp1, ch, _shaProperties.WordSize);
                temp1 = BitString.AddWithModulo(temp1, _k[i], _shaProperties.WordSize);
                temp1 = BitString.AddWithModulo(temp1, _w[i], _shaProperties.WordSize);

                // (a rightrotate 2) ^ (a rightrotate 13) ^ (a rightrotate 22)
                var sum0 = BitString.XOR(
                              BitString.XOR(
                                  BitString.LSBRotate(_temps[0], _sumShifts[0, 0]),
                                  BitString.LSBRotate(_temps[0], _sumShifts[0, 1])
                              ),
                              BitString.LSBRotate(_temps[0], _sumShifts[0, 2])
                           );

                // (a & b) ^ (a & c) ^ (b & c)
                var maj = BitString.XOR(
                             BitString.XOR(
                                BitString.AND(_temps[0], _temps[1]),
                                BitString.AND(_temps[0], _temps[2])
                             ),
                             BitString.AND(_temps[1], _temps[2])
                          );

                // s0 + maj
                var temp2 = BitString.AddWithModulo(sum0, maj, _shaProperties.WordSize);

                _temps[7] = _temps[6].GetDeepCopy();
                _temps[6] = _temps[5].GetDeepCopy();
                _temps[5] = _temps[4].GetDeepCopy();
                _temps[4] = BitString.AddWithModulo(_temps[3], temp1, _shaProperties.WordSize);
                _temps[3] = _temps[2].GetDeepCopy();
                _temps[2] = _temps[1].GetDeepCopy();
                _temps[1] = _temps[0].GetDeepCopy();
                _temps[0] = BitString.AddWithModulo(temp1, temp2, _shaProperties.WordSize);
            }

            for(var i = 0; i < 8; i++)
            {
                _h[i] = BitString.AddWithModulo(_h[i], _temps[i], _shaProperties.WordSize);
            }
        }

        private BitString[] DivideIntoWords(BitString chunk)
        {
            var words = new BitString[_shaProperties.Rounds];

            // Split each chunk into 16, 32 or 64-bit words
            for (var i = 0; i < 16; i++)
            {
                words[i] = chunk.MSBSubstring(i * _shaProperties.WordSize, _shaProperties.WordSize);
            }

            // The rest of the words are an expansion of the previous
            for (var i = 16; i < _shaProperties.Rounds; i++)
            {
                var sigma0 = BitString.XOR(
                                BitString.XOR(
                                    BitString.LSBRotate(words[i - 15], _sigmaShifts[0, 0]),
                                    BitString.LSBRotate(words[i - 15], _sigmaShifts[0, 1])
                                ),
                                BitString.LSBShift(words[i - 15], _sigmaShifts[0, 2])
                             );

                var sigma1 = BitString.XOR(
                                BitString.XOR(
                                    BitString.LSBRotate(words[i - 2], _sigmaShifts[1, 0]),
                                    BitString.LSBRotate(words[i - 2], _sigmaShifts[1, 1])
                                ),
                                BitString.LSBShift(words[i - 2], _sigmaShifts[1, 2])
                             );

                words[i] = BitString.AddWithModulo(words[i - 16], sigma0, _shaProperties.WordSize);
                words[i] = BitString.AddWithModulo(words[i], words[i - 7], _shaProperties.WordSize);
                words[i] = BitString.AddWithModulo(words[i], sigma1, _shaProperties.WordSize);
            }

            return words;
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
