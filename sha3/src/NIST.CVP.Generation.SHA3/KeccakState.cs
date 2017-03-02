using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA3
{
    public class KeccakState
    {
        private const int RowSize = 5;
        private const int ColSize = 5;
        private const int GridSize = 25;
        public readonly int Width;              
        public readonly int L;

        private readonly BitString[,] _state;

        public KeccakState(BitString content, int b)        // b is always 1600 but keep it flexible for future
        {
            Width = b / GridSize;
            L = (int)System.Math.Log(Width, 2);
            _state = new BitString[RowSize, ColSize];

            for (var x = 0; x < RowSize; x++)
            {
                for (var y = 0; y < ColSize; y++)
                {
                    _state[x, y] = content.MSBSubstring((x + RowSize * y) * Width, Width);
                }
            }
        }

        public KeccakState(KeccakState old)
        {
            Width = old.Width;
            L = old.L;
            _state = new BitString[RowSize, ColSize];

            for (var x = 0; x < RowSize; x++)
            {
                for (var y = 0; y < ColSize; y++)
                {
                    _state[x, y] = old.GetLane(x, y);
                }
            }
        }

        public BitString GetBit(int x, int y, int z)
        {
            return GetLane(x, y).MSBSubstring(z, 1);
        }

        public void SetBit(int x, int y, int z, bool bit)
        {
            var newBits = _state[x, y].Bits;
            newBits[z] = bit;
            _state[x, y] = new BitString(newBits);
        }

        public void SetLane(int x, int y, BitString lane)
        {
            _state[x, y] = lane.GetDeepCopy();
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
                result = BitString.ConcatenateBits(result, _state[i, plane]);
            }

            return result;
        }

        public BitString GetLane(int x, int y)
        {
            return _state[x, y];
        }

        #region Transformation Functions
        public static KeccakState Iota(KeccakState A, int roundIdx)
        {
            var A_prime = new KeccakState(A);

            var rc = new BitString(A.Width);

            for (var j = 0; j <= A.L; j++)
            {
                var idx = (int)System.Math.Pow(2, j) - 1;
                rc.Set(idx, RC(7 * roundIdx + j));
            }

            rc = BitString.ReverseByteOrder(rc);

            A_prime.SetLane(0, 0, BitString.XOR(A.GetLane(0, 0), rc));
            return A_prime;
        }

        public static KeccakState Chi(KeccakState A)
        {
            var A_prime = new KeccakState(A);

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    var intermediate = BitString.AND(BitString.XOR(A.GetLane((x + 1) % 5, y), BitString.Ones(A.Width)), A.GetLane((x + 2) % 5, y));
                    A_prime.SetLane(x, y, BitString.XOR(A.GetLane(x, y), intermediate));
                }
            }

            return A_prime;
        }

        public static KeccakState Pi(KeccakState A)
        {
            var A_prime = new KeccakState(A);

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    A_prime.SetLane(x, y, A.GetLane((x + 3 * y) % 5, x));
                }
            }

            return A_prime;
        }

        public static KeccakState Rho(KeccakState A)
        {
            var A_prime = new KeccakState(A);
            var offsets = RhoOffsets(A.Width);

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    A_prime.SetLane(x, y, KeccakRotateLeft(A.GetLane(x, y), offsets[x, y]));
                }
            }

            return A_prime;
        }

        public static KeccakState Theta(KeccakState A)
        {
            var A_prime = new KeccakState(A);
            var C = new BitString[5];
            var D = new BitString[5];

            for (var x = 0; x < 5; x++)
            {
                C[x] = new BitString(A.Width);
                for (var y = 0; y < 5; y++)
                {
                    C[x] = BitString.XOR(C[x], A.GetLane(x, y));
                }
            }

            var E = new BitString[5];
            var F = new BitString[5];
            for (var x = 0; x < 5; x++)
            {
                F[x] = C[(x + 1) % 5];
                E[x] = KeccakRotateLeft(F[x], 1);
                //E[x] = BitString.MSBRotate(F[x], 1);
                D[x] = BitString.XOR(C[(x - 1 + 5) % 5], E[x]);
            }

            //string e0 = E[0].ToHex();
            //string e1 = E[1].ToHex();
            //string e2 = E[2].ToHex();
            //string e3 = E[3].ToHex();
            //string e4 = E[4].ToHex();

            //string f0 = F[0].ToHex();
            //string f1 = F[1].ToHex();
            //string f2 = F[2].ToHex();
            //string f3 = F[3].ToHex();
            //string f4 = F[4].ToHex();

            //throw new Exception($"{f0}\n{f1}\n{f2}\n{f3}\n{f4}\n\n\n{e0}\n{e1}\n{e2}\n{e3}\n{e4}");

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    A_prime.SetLane(x, y, BitString.XOR(A_prime.GetLane(x, y), D[x]));
                }
            }

            return A_prime;
        }
        #endregion Transformation Functions

        #region Helpers
        private static BitString KeccakRotateLeft(BitString input, int distance)
        {
            return BitString.ReverseByteOrder(BitString.MSBRotate(BitString.ReverseByteOrder(input), distance));
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
    }
}
