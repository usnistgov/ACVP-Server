using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.Ed
{
    public class EdwardsCurve : IEdwardsCurve
    {
        private readonly IFieldOperator _operator;
        
        public BigInteger CoefficientA { get; }
        public BigInteger CoefficientD { get; }
        public EdPoint BasePointG { get; }
        public BigInteger OrderN { get; }
        public BigInteger FieldSizeQ { get; }

        // Cofactor is always 1 for a prime curve
        public int CofactorH { get { return 1; } }

        // CurveType is obviously prime
        public CurveType CurveType { get { return CurveType.Prime; } }

        public Curve CurveName { get; }

        public EdwardsCurve(Curve curveName, BigInteger p, BigInteger a, BigInteger b, EdPoint g, BigInteger n)
        {
            CurveName = curveName;

            FieldSizeQ = p;
            CoefficientA = a;
            CoefficientD = b;
            BasePointG = g;
            OrderN = n;

            _operator = new PrimeFieldOperator(p);      // current Edwards curves only use prime field operator... and possibly all ed curves??
        }

        public EdPoint Add(EdPoint pointA, EdPoint pointB)
        {
            var numeratorX = _operator.Add(_operator.Multiply(pointA.X, pointB.Y), _operator.Multiply(pointA.Y, pointB.X));
            var numeratorY = _operator.Subtract(_operator.Multiply(pointA.Y, pointB.Y), _operator.Multiply(CoefficientA, _operator.Multiply(pointA.X, pointB.X)));
            var denominatorMult = _operator.Multiply(CoefficientD, _operator.Multiply(_operator.Multiply(pointA.X, pointB.X), _operator.Multiply(pointA.Y, pointB.Y)));
            var denominatorX = _operator.Add(1, denominatorMult);
            var denominatorY = _operator.Subtract(1, denominatorMult);

            var x = _operator.Divide(numeratorX, denominatorX);
            var y = _operator.Divide(numeratorY, denominatorY);

            return new EdPoint(x, y);
        }

        public EdPoint Negate(EdPoint point)
        {
            return new EdPoint(_operator.Negate(point.X), point.Y);
        }

        public EdPoint Subtract(EdPoint pointA, EdPoint pointB)
        {
            return Add(pointA, Negate(pointB));
        }

        public EdPoint Double(EdPoint point)
        {
            return Add(point, point);
        }

        private EdPoint Multiply(EdPoint startPoint, NonAdjacentBitString nafBs)
        {
            var point = new EdPoint(0 , 1);
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

        public EdPoint Multiply(EdPoint startPoint, BigInteger scalar)
        {
            // Find scalar within group and convert to NABS, normal modulo here, not on the field, like CAVS
            return Multiply(startPoint, new NonAdjacentBitString(scalar % OrderN));
        }

        public bool PointExistsOnCurve(EdPoint point)
        {
            // Point is out of bounds
            if (!PointExistsInField(point))
            {
                return false;
            }

            var lhs = _operator.Add(_operator.Multiply(CoefficientA, _operator.Multiply(point.X, point.X)), _operator.Multiply(point.Y, point.Y));
            var rhs = _operator.Add(1, _operator.Multiply(CoefficientD, _operator.Multiply(_operator.Multiply(point.X, point.X), _operator.Multiply(point.Y, point.Y))));

            return (lhs == rhs);
        }

        public bool PointExistsInField(EdPoint point)
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

        public BigInteger Encode(EdPoint point, int b)
        {
            var encoding = new BitString(point.Y, b - 1);

            encoding = BitString.ConcatenateBits(new BitString(point.X).GetLeastSignificantBits(1), encoding);

            return encoding.ToPositiveBigInteger();
        }

        public EdPoint Decode(BigInteger encoded, int b)
        {
            var encodedBitString = new BitString(encoded, b);
            var x = encodedBitString.GetMostSignificantBits(1);
            var Y = encodedBitString.GetMostSignificantBits(b - 1).ToPositiveBigInteger();
            BigInteger X;
            if (FieldSizeQ % 4 == 3)
            {
                var u = Y * Y - 1;
                var v = CoefficientD * Y * Y + 1;
                
            }
        }
    }
}
