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
        private readonly PrimeFieldOperator _operator;

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

            _operator = new PrimeFieldOperator(p);
        }

        public EccPoint Add(EccPoint pointA, EccPoint pointB)
        {
            // Any point added to infinity is itself
            if (pointA.Infinity)
            {
                return pointB;
            }

            // Any point added to infinity is itself
            if (pointB.Infinity)
            {
                return pointA;
            }

            // Any point added to its inverse is infinity
            if (pointA.Equals(Negate(pointB)))
            {
                return new EccPoint("infinity");
            }

            // Cannot add two identical points, use Double instead
            if (pointA.Equals(pointB))
            {
                return Double(pointA);
            }

            var numerator = _operator.Subtract(pointB.Y, pointA.Y);
            var denominator = _operator.Subtract(pointB.X, pointA.X);
            var lambda = _operator.Divide(numerator, denominator);

            var x = _operator.Subtract(_operator.Subtract(_operator.Multiply(lambda, lambda), pointA.X), pointB.X);
            var y = _operator.Subtract(_operator.Multiply(_operator.Subtract(pointA.X, x), lambda), pointA.Y);

            return new EccPoint(x, y);
        }

        public EccPoint Negate(EccPoint point)
        {
            if (point.Infinity)
            {
                return point;
            }

            // Negate the point, - (x, y) == (x, -y), but -1 * y (mod q) == q - y
            return new EccPoint(point.X, _operator.Negate(point.Y));
        }

        public EccPoint Subtract(EccPoint pointA, EccPoint pointB)
        {
            return Add(pointA, Negate(pointB));
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

            var numerator = _operator.Add(_operator.Multiply(_operator.Multiply(3, point.X), point.X), CoefficientA);
            var denominator = _operator.Multiply(2, point.Y);
            var lambda = _operator.Divide(numerator, denominator);

            var x = _operator.Subtract(_operator.Multiply(lambda, lambda), _operator.Multiply(2, point.X));
            var y = _operator.Subtract(_operator.Multiply(_operator.Subtract(point.X, x), lambda), point.Y);

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
            // Find scalar within group and convert to NABS
            return Multiply(startPoint, new NonAdjacentBitString(_operator.Modulo(scalar)));
        }

        public bool PointExistsOnCurve(EccPoint point)
        {
            if (point.Infinity)
            {
                return true;
            }

            // Point is out of bounds
            if (point.X < 0 || point.X >= FieldSizeQ || point.Y < 0 || point.Y >= FieldSizeQ)
            {
                return false;
            }

            var lhs = _operator.Multiply(point.Y, point.Y);
            var rhs = _operator.Add(_operator.Add(_operator.Multiply(_operator.Multiply(point.X, point.X), point.X), _operator.Multiply(CoefficientA, point.X)), CoefficientB);

            return (lhs == rhs);
        }

        public bool PointExistsInField(EccPoint point)
        {
            if (point.X < 0 || point.X > FieldSizeQ - 1)
            {
                return false;
            }

            if (point.Y < 0 || point.Y > FieldSizeQ - 1)
            {
                return false;
            }

            return true;
        }
    }
}
