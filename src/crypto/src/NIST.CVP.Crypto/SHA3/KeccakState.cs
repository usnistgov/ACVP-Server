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

        private const ulong ONES = 18446744073709551615;
        private static ulong[,] workingStateChi = new ulong[5, 5];
        private static ulong[,] workingStatePi = new ulong[5, 5];

        public KeccakState(BitString content, int b)        // b is always 1600 but keep it flexible for future
        {
            Width = b / GridSize;
            L = (int)System.Math.Log(Width, 2);
            _state = new ulong[RowSize, ColSize];

            for (var x = 0; x < RowSize; x++)
            {
                for (var y = 0; y < ColSize; y++)
                {
                    _state[x, y] = BitStringToLong(content.MSBSubstring((x + RowSize * y) * Width, Width));
                }
            }
        }

        public KeccakState(KeccakState old)
        {
            Width = old.Width;
            L = old.L;
            _state = new ulong[RowSize, ColSize];

            for (var x = 0; x < RowSize; x++)
            {
                for (var y = 0; y < ColSize; y++)
                {
                    _state[x, y] = old.GetLane(x, y);
                }
            }
        }

        public bool GetBit(int x, int y, int z)
        {
            return (GetLane(x, y) & (ulong)(1 << z - 1)) != 0;
        }

        public void SetBit(int x, int y, int z, bool bit)
        {
            ulong mask = (ulong)(1 << z);
            _state[x, y] = bit ? _state[x, y] | mask : _state[x, y] & ~mask;
        }

        public void SetLane(int x, int y, ulong lane)
        {
            _state[x, y] = lane;
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
        
        public ulong GetLane(int x, int y)
        {
            return _state[x, y];
        }

        #region Transformation Functions

        public static KeccakState Iota(KeccakState A, int roundIdx)
        {
            ulong rc = 0;

            for (var j = 0; j <= A.L; j++)
            {
                var idx = (int)System.Math.Pow(2, j) - 1;
                var bit = RC(7 * roundIdx + j);
                ulong mask = ((ulong)(1) << idx);
                rc = bit ? rc | mask : rc & ~mask;
            }

            A.SetLane(0, 0, A.GetLane(0, 0) ^ rc);
            return A;
        }

        public static KeccakState Chi(KeccakState A)
        {
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    var intermediate = (A.GetLane((x + 1) % 5, y) ^ ONES) & A.GetLane((x + 2) % 5, y);
                    workingStateChi[x, y] = A.GetLane(x, y) ^ intermediate;
                }
            }
            A._state = workingStateChi;

            return A;
        }

        public static KeccakState Pi(KeccakState A)
        {
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    workingStatePi[x, y] = A.GetLane((x + 3 * y) % 5, x);
                }
            }
            A._state = workingStatePi;

            return A;
        }

        public static KeccakState Rho(KeccakState A)
        {
            var offsets = RhoOffsets(A.Width);

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    A.SetLane(x, y, KeccakRotateLeft(A.GetLane(x, y), offsets[x, y]));
                }
            }

            return A;
        }

        public static KeccakState Theta(KeccakState A)
        {
            var C = new ulong[5];
            var D = new ulong[5];

            for (var x = 0; x < 5; x++)
            {
                C[x] = (ulong)0;
                for (var y = 0; y < 5; y++)
                {
                    C[x] = C[x] ^ A.GetLane(x, y);
                }
            }

            var E = new ulong[5];
            var F = new ulong[5];
            for (var x = 0; x < 5; x++)
            {
                F[x] = C[(x + 1) % 5];
                E[x] = KeccakRotateLeft(F[x], 1);
                D[x] = C[(x - 1 + 5) % 5] ^ E[x];
            }

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    A.SetLane(x, y, A.GetLane(x, y) ^ D[x]);
                }
            }

            return A;
        }
        #endregion Transformation Functions

        #region Helpers

        private static ulong ReverseByteOrder(ulong value) 
        {
            return (value & 0x00000000000000FF) << 56 | (value & 0x000000000000FF00) << 40 |
                (value & 0x0000000000FF0000) << 24 | (value & 0x00000000FF000000) << 8 |
                (value & 0x000000FF00000000) >> 8 | (value & 0x0000FF0000000000) >> 24 |
                (value & 0x00FF000000000000) >> 40 | (value & 0xFF00000000000000) >> 56;
        }

        private static ulong KeccakRotateLeft(ulong input, int distance)
        {
            return (input << distance) | (input >> (64 - distance));
        }

        private static int[,] RhoOffsets(int Width)
        {
            var offsets = new int[5, 5];
            int x = 1, y = 0;

            for (var t = 0; t < 24; t++)
            {
                offsets[x, y] = ((t + 1) * (t + 2) / 2) % Width;
                var newX = (0 * x + 1 * y) % 5;
                var newY = (2 * x + 3 * y) % 5;
                x = newX;
                y = newY;
            }

            return offsets;
        }

        private static bool RC(int t)
        {
            var iterations = t % 255;
            if (iterations == 0)
            {
                return true;
            }

            // R = 1000 0000
            var R = new bool[9];
            R[0] = true;
            R[1] = R[2] = R[3] = R[4] = R[5] = R[6] = R[7] = false;

            for (var i = 0; i < iterations; i++)
            {
                // R = 0 || R
                for (var j = 8; j > 0; j--)
                {
                    // Truncation happens silently
                    R[j] = R[j - 1];
                }
                R[0] = false;

                // XORs
                R[0] = (R[0] != R[8]);
                R[4] = (R[4] != R[8]);
                R[5] = (R[5] != R[8]);
                R[6] = (R[6] != R[8]);
            }

            return R[0];
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
