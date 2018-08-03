using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.SHA3
{
    public class KeccakState
    {
        private const int RowSize = 5;
        private const int ColSize = 5;
        private const int GridSize = 25;
        public readonly int Width;              
        public readonly int L;
        
        private ulong[,] _state;

        private int[,] _offsets;
        private ulong[,] _workingStateChi = new ulong[5, 5];
        private ulong[,] _workingStatePi = new ulong[5, 5];


        public KeccakState(BitString content, int b)        // b is always 1600 but keep it flexible for future
        {
            Width = b / GridSize;
            L = (int)System.Math.Log(Width, 2);
            _state = new ulong[RowSize, ColSize];


            // done for better speed when using b as 1600
            if (b == 1600)
            {
                _offsets = new int[5, 5] { 
                    { 0, 36, 3, 41, 18 },
                    { 1, 44, 10, 45, 2 }, 
                    { 62, 6, 43, 15, 61 }, 
                    { 28, 55, 25, 21, 56 },
                    { 27, 20, 39, 8, 14 }
                };
            }
            else
            {
                _offsets = new int[5, 5];

                int x = 1, y = 0;

                for (var t = 0; t < 24; t++)
                {
                    _offsets[x, y] = ((t + 1) * (t + 2) / 2) % Width;
                    var newX = (0 * x + 1 * y) % 5;
                    var newY = (2 * x + 3 * y) % 5;
                    x = newX;
                    y = newY;
                }
            }

            for (var x = 0; x < RowSize; x++)
            {
                for (var y = 0; y < ColSize; y++)
                {
                    _state[x, y] = BitStringToLong(content.MSBSubstring((x + RowSize * y) * Width, Width));
                }
            }
        }

        public bool GetBit(int x, int y, int z)
        {
            return (_state[x, y] & (ulong)(1 << z - 1)) != 0;
        }

        public void SetBit(int x, int y, int z, bool bit)
        {
            ulong mask = ((ulong)1 << z);
            _state[x, y] = bit ? _state[x, y] | mask : _state[x, y] & ~mask;
        }

        public void SetLane(int x, int y, BitString lane)
        {
            _state[x, y] = BitStringToLong(lane);
        }

        public BitString ToBitString()
        {
            var result = new BitString(0);

            for (var i = 0; i < ColSize; i++)
            {
                result = BitString.ConcatenateBits(result, GetPlane(i));
            }

            return result;
        }

        public BitString GetPlane(int plane)
        {
            var result = new BitString(0);

            for (var i = 0; i < RowSize; i++)
            {
                result = BitString.ConcatenateBits(result, new BitString(BitConverter.GetBytes(_state[i, plane]), Width));
            }

            return result;
        }
        
        public BitString GetLane(int x, int y)
        {
            return new BitString(BitConverter.GetBytes(_state[x, y]));
        }

        #region Transformation Functions

        public KeccakState Iota(int roundIdx)
        {
            ulong rc = 0;

            for (var j = 0; j <= L; j++)
            {
                var idx = (int)System.Math.Pow(2, j) - 1;
                var bit = RC(7 * roundIdx + j);
                ulong mask = ((ulong)1 << idx);
                rc = bit ? rc | mask : rc & ~mask;
            }

            _state[0, 0] = _state[0, 0] ^ rc;
            return this;
        }

        public KeccakState Chi()
        {
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    var intermediate = (_state[(x + 1) % 5, y] ^ ulong.MaxValue) & _state[(x + 2) % 5, y];
                    _workingStateChi[x, y] = _state[x, y] ^ intermediate;
                }
            }
            _state = _workingStateChi;

            return this;
        }

        public KeccakState Pi()
        {
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    _workingStatePi[x, y] = _state[(x + 3 * y) % 5, x];
                }
            }
            _state = _workingStatePi;

            return this;
        }

        public KeccakState Rho()
        {
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    var input = _state[x, y];
                    var distance = _offsets[x, y];
                    _state[x, y] =  (input << distance) | (input >> (64 - distance));
                }
            }

            return this;
        }

        public KeccakState Theta()
        {
            var C = new ulong[5];
            var D = new ulong[5];

            for (var x = 0; x < 5; x++)
            {
                C[x] = (ulong)0;
                for (var y = 0; y < 5; y++)
                {
                    C[x] = C[x] ^ _state[x, y];
                }
            }

            var E = new ulong[5];
            var F = new ulong[5];
            for (var x = 0; x < 5; x++)
            {
                F[x] = C[(x + 1) % 5];
                E[x] = (F[x] << 1) | (F[x] >> 63);
                D[x] = C[(x - 1 + 5) % 5] ^ E[x];
            }

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    _state[x, y] =  _state[x, y] ^ D[x];
                }
            }

            return this;
        }
        #endregion Transformation Functions

        #region Helpers

        private static bool RC(int t)
        {
            var iterations = t % 255;
            if (iterations == 0)
            {
                return true;
            }

            byte R = 0b10000000;
            byte prev_R;

            for (var i = 0; i < iterations; i++)
            {
                prev_R = R;
                R = (byte)(R >> 1);

                // XORs
                var R_8 = (prev_R & 1) != 0;
                R = (R_8 != ((R & 0b10000000) != 0) ? R |= 0b10000000 : R &= 0b01111111);
                R = (R_8 != ((R & 0b00001000) != 0) ? R |= 0b00001000 : R &= 0b11110111);
                R = (R_8 != ((R & 0b00000100) != 0) ? R |= 0b00000100 : R &= 0b11111011);
                R = (R_8 != ((R & 0b00000010) != 0) ? R |= 0b00000010 : R &= 0b11111101);
            }
            return (R & 0b10000000) != 0;
        }
        #endregion Helpers

        private static ulong BitStringToLong(BitString bitString)
        {
            var bytes = bitString.ToBytes();
            if (bytes.Length < 8)
            {
                byte[] bytesCopy = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    if (i < bytes.Length)
                    {
                        bytesCopy[i] = bytes[i];
                    }
                    else
                    {
                        bytesCopy[i] = 0;
                    }
                }
                return BitConverter.ToUInt64(bytesCopy, 0);
            }
            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}
