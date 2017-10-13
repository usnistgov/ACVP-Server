using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class PrimeCurve : IEccCurve
    {
        // A = -3 (mod p) for prime curves
        public BigInteger CoefficientA { get { return FieldSizeQ - 3; } }
        public BigInteger CoefficientB { get; }
        public EccPoint BasePointG { get; }
        public BigInteger OrderN { get; }
        public BigInteger FieldSizeQ { get; }

        // Cofactor is always 1 for a prime curve
        public int CofactorH { get { return 1; } }

        // CurveType is obviously prime
        public CurveType CurveType { get { return CurveType.Prime; } }

        public PrimeCurve(BigInteger p, BigInteger b, EccPoint g, BigInteger n)
        {
            FieldSizeQ = p;
            CoefficientB = b;
            BasePointG = g;
            OrderN = n;
        }

        public EccPoint Add(EccPoint pointA, EccPoint pointB)
        {
            if (pointA.Infinity || pointB.Infinity)
            {
                return new EccPoint("infinity");
            }

            if (pointA.X == pointB.X && pointA.Y == -1 * pointB.Y)
            {
                return new EccPoint("infinity");
            }

            if (pointA.X == pointB.X)
            {
                throw new ArgumentException("Cannot add two points with same x value");
            }

            var lambda = ((pointB.Y - pointA.Y) / (pointB.X - pointA.X)).PosMod(FieldSizeQ);
            var pointCX = (lambda * lambda - pointA.X - pointB.X).PosMod(FieldSizeQ);
            var pointCY = (lambda * (pointA.X - pointCX) - pointA.Y).PosMod(FieldSizeQ);

            return new EccPoint(pointCX, pointCY);
        }

        public EccPoint Double(EccPoint point)
        {
            if ((point.X == 0 && point.Y == 0) || point.Infinity)
            {
                return point;
            }

            if (point.Y == 0)
            {
                throw new ArgumentException("Cannot double a point with y = 0");
            }

            var lambda = ((3 * point.X * point.X + CoefficientA) / (2 * point.Y)).PosMod(FieldSizeQ);
            var x = ((lambda * lambda) - (2 * point.X)).PosMod(FieldSizeQ);
            var y = (lambda * (point.X - x) - point.Y).PosMod(FieldSizeQ);

            return new EccPoint(x, y);
        }

        public EccPoint Multiply(EccPoint startPoint, BigInteger scalar)
        {
            var bits = new BitString(scalar).Bits;
            var point = new EccPoint(0, 0);
            
            for (var i = bits.Length - 1; i >= 0; i--)
            {
                point = Double(point);
                if (bits[i])
                {
                    point = Add(point, startPoint);
                }
            }

            return point;
        }

        public bool PointExistsOnCurve(EccPoint point)
        {
            if (point.Infinity)
            {
                return false;
            }

            // Point is out of bounds
            if (point.X < 0 || point.X >= FieldSizeQ || point.Y < 0 || point.Y >= FieldSizeQ)
            {
                return false;
            }

            var lhs = point.Y * point.Y;
            var rhs = point.X * point.X * point.X + CoefficientA * point.X + CoefficientB;

            return (lhs == rhs);
        }
    }
}
