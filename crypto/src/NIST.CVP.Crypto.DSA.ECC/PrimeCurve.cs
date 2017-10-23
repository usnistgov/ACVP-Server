using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.Math;
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
            if (pointA.Infinity)
            {
                return pointB;
            }

            if (pointB.Infinity)
            {
                return pointA;
            }

            if (pointA.X == pointB.X && pointA.Y == -1 * pointB.Y)
            {
                return new EccPoint("infinity");
            }

            if (pointA.X == pointB.X)
            {
                throw new ArgumentException("Cannot add two points with same x value");
            }

            var numerator = (pointB.Y - pointA.Y).PosMod(FieldSizeQ);
            var demoninator = (pointB.X - pointA.X).PosMod(FieldSizeQ);
            var lambda = (numerator * NumberTheory.ModularInverse(demoninator, FieldSizeQ)).PosMod(FieldSizeQ);
            
            var pointCX = (lambda * lambda - pointA.X - pointB.X).PosMod(FieldSizeQ);
            var pointCY = (lambda * (pointA.X - pointCX) - pointA.Y).PosMod(FieldSizeQ);

            return new EccPoint(pointCX, pointCY);
        }

        public EccPoint Subtract(EccPoint pointA, EccPoint pointB)
        {
            // Negate the point, - (x, y) == (x, -y), but -1 * y (mod q) == q - y
            var negPointB = new EccPoint(pointB.X, FieldSizeQ - pointB.Y);
            return Add(pointA, negPointB);
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

            var numerator = (3 * point.X * point.X + CoefficientA).PosMod(FieldSizeQ);
            var denominator = (2 * point.Y).PosMod(FieldSizeQ);
            var lambda = (numerator * NumberTheory.ModularInverse(denominator, FieldSizeQ)).PosMod(FieldSizeQ);

            var x = ((lambda * lambda) - (2 * point.X)).PosMod(FieldSizeQ);
            var y = (lambda * (point.X - x) - point.Y).PosMod(FieldSizeQ);

            return new EccPoint(x, y);
        }

        public EccPoint Multiply(EccPoint startPoint, NonAdjacentBitString nafBs)
        {
            var point = new EccPoint("infinity");
            var naBits = nafBs.Bits;

            for (var i = naBits.Length - 1; i >= 0; i--)
            {
                point = Double(point);
                if (naBits[i] == 1)
                {
                    point = Add(point, startPoint);
                }
                else if (naBits[i] == -1)
                {
                    point = Subtract(point, startPoint);
                }
            }

            return point;
        }

        public EccPoint Multiply(EccPoint startPoint, BigInteger scalar)
        {
            // Find scalar within group
            scalar %= FieldSizeQ;
            return Multiply(startPoint, new NonAdjacentBitString(scalar));
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

            return (lhs.PosMod(FieldSizeQ) == rhs.PosMod(FieldSizeQ));
        }
    }
}
