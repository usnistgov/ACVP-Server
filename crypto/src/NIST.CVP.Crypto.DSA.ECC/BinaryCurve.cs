using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.DSA.ECC.Helpers;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class BinaryCurve : IEccCurve
    {
        private readonly GaloisFieldOperator _operator;

        public BigInteger FieldSizeQ { get; }
        public BigInteger CoefficientA { get; }
        public BigInteger CoefficientB { get; }
        public EccPoint BasePointG { get; }
        public BigInteger OrderN { get; }
        public int CofactorH { get; }

        public CurveType CurveType { get { return CurveType.Binary; } }

        public Curve CurveName { get; }

        private readonly List<EccPoint> _precomputedMultiplication;
        private bool HasPrecomputedValues => _precomputedMultiplication.Count != 0;

        public BinaryCurve(Curve curveName, BigInteger f, BigInteger a, BigInteger b, EccPoint basis, BigInteger n, int h, List<EccPoint> points = null)
        {
            CurveName = curveName;

            FieldSizeQ = f;
            CoefficientA = a;
            CoefficientB = b;
            BasePointG = basis;
            OrderN = n;
            CofactorH = h;

            _operator = new GaloisFieldOperator(f);
            _precomputedMultiplication = points ?? new List<EccPoint>();
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

            var numerator = _operator.Add(pointA.Y, pointB.Y);
            var denominator = _operator.Add(pointA.X, pointB.X);

            if (denominator == 0)
            {
                return new EccPoint("infinity");
            }

            var lambda = _operator.Multiply(numerator, _operator.Inverse(denominator));

            // x = lambda * lambda + lambda + x1 + x2 + a
            var x = _operator.Add(_operator.Add(_operator.Add(_operator.Add(_operator.Multiply(lambda, lambda), lambda), pointA.X), pointB.X), CoefficientA);

            // y = lambda * (x1 + x3) + x + y1
            var y = _operator.Add(_operator.Add(_operator.Multiply(_operator.Add(pointA.X, x), lambda), x), pointA.Y);

            return new EccPoint(x, y);
        }

        public EccPoint Negate(EccPoint point)
        {
            if (point.Infinity)
            {
                return point;
            }

            return new EccPoint(point.X, _operator.Add(point.X, point.Y));
        }

        public EccPoint Subtract(EccPoint pointA, EccPoint pointB)
        {
            if (pointA.Infinity || pointB.Infinity)
            {
                return pointA;
            }

            return Add(pointA, Negate(pointB));
        }

        public EccPoint Double(EccPoint point)
        {
            if (point.X == 0 || point.Infinity)
            {
                return point;
            }

            var numerator = point.Y;
            var denominator = point.X;
            var lambda = _operator.Add(_operator.Multiply(numerator, _operator.Inverse(denominator)), point.X);

            var x = _operator.Add(_operator.Add(_operator.Multiply(lambda, lambda), lambda), CoefficientA);
            var y = _operator.Add(_operator.Multiply(_operator.Add(lambda, 1), x), _operator.Multiply(point.X, point.X));

            return new EccPoint(x, y);
        }

        public EccPoint Multiply(EccPoint startPoint, BigInteger scalar)
        {
            // Intentional normal modulo, this is what CAVS does
            scalar %= OrderN;

            if (startPoint.Equals(BasePointG) && HasPrecomputedValues)
            {
                return FastMultiply(scalar);
            }
            else
            {
                return Multiply(startPoint, new NonAdjacentBitString(scalar));
            }
        }

        /// <summary>
        /// Based on the Fixed-base comb method (Algorithm 17 from https://link.springer.com/content/pdf/10.1007/3-540-44499-8_1.pdf)
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        private EccPoint FastMultiply(BigInteger scalar)
        {
            var width = 4;
            var degree = FieldSizeQ.ExactBitLength();
            var d = degree.CeilingDivide(width);
            var scalarBits = new BitString(scalar, degree).Bits;
            var point = new EccPoint("infinity");

            for (var i = d - 1; i >= 0; i--)
            {
                point = Double(point);

                var index = 0;
                for (var j = width - 1; j >= 0; j--)
                {
                    index *= 2;
                    if (i + d * j >= degree) continue;
                    if (scalarBits[i + d * j])
                    {
                        index++;
                    }
                }

                if (index != 0)
                {
                    point = Add(point, _precomputedMultiplication[index]);
                }
            }

            return point;
        }

        private EccPoint Multiply(EccPoint startPoint, NonAdjacentBitString nafBs)
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

        public bool PointExistsOnCurve(EccPoint point)
        {
            if (point.Infinity)
            {
                return true;
            }

            // Point is out of bounds
            if (!PointExistsInField(point))
            {
                return false;
            }

            var ySquared = _operator.Multiply(point.Y, point.Y);
            var xTimesY = _operator.Multiply(point.X, point.Y);
            var lhs = _operator.Add(ySquared, xTimesY);

            var xCubed = _operator.Multiply(_operator.Multiply(point.X, point.X), point.X);
            var xSquared = _operator.Multiply(point.X, point.X);
            var aTimesXSquares = _operator.Multiply(CoefficientA, xSquared);
            var rhs = _operator.Add(_operator.Add(xCubed, aTimesXSquares), CoefficientB);

            // y^2 + xy = x^3 + ax^2 + b
            return (lhs == rhs);
        }

        public bool PointExistsInField(EccPoint point)
        {
            var m = FieldSizeQ.ExactBitLength();

            if (point.X.ExactBitLength() == m || point.Y.ExactBitLength() == m)
            {
                return false;
            }

            return true;
        }
    }
}
