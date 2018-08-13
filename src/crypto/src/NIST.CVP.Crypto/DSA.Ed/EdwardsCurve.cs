using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Math;
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

        // Coefficients used for EdDSA as specified in IETF RFC 8032
        public int VariableB { get; }
        public int VariableN { get; }
        public int VariableC { get; }

        // CurveType is obviously prime
        public CurveType CurveType { get { return CurveType.Prime; } }

        public Curve CurveName { get; }

        public EdwardsCurve(Curve curveName, BigInteger p, BigInteger a, BigInteger d, EdPoint g, BigInteger n, int b, int dSAn, int c)
        {
            CurveName = curveName;

            FieldSizeQ = p;
            CoefficientA = a;
            CoefficientD = d;
            BasePointG = g;
            OrderN = n;
            VariableB = b;
            VariableN = dSAn;
            VariableC = c;

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
            var x = encodedBitString.GetMostSignificantBits(1).ToPositiveBigInteger();
            var Y = encodedBitString.GetMostSignificantBits(b - 1).ToPositiveBigInteger();

            BigInteger X;
            var u = (Y * Y - 1) % FieldSizeQ;
            var v = (CoefficientD * Y * Y + 1) % FieldSizeQ;
            if (FieldSizeQ % 4 == 3)
            {
                var w = (u * u * u * v * BigInteger.ModPow(BigInteger.ModPow(u, 5, FieldSizeQ) * BigInteger.ModPow(v, 3, FieldSizeQ) % FieldSizeQ, (FieldSizeQ - 3) / 4, FieldSizeQ)) % FieldSizeQ;
                if ((v * w * w) % FieldSizeQ == u)
                {
                    X = w;
                }
                else
                {
                    throw new Exception("Square root does not exist");
                }
            }
            else if (FieldSizeQ % 8 == 5)
            {
                var w = (u * v * v * v * BigInteger.ModPow(u * BigInteger.ModPow(v, 7, FieldSizeQ), (FieldSizeQ - 5) / 8, FieldSizeQ)) % FieldSizeQ;
                if ((v * w * w) % FieldSizeQ == u)
                {
                    X = w;
                }
                else if ((v * w * w) % FieldSizeQ == (-u) % FieldSizeQ)
                {
                    X = (w * BigInteger.ModPow(2, (FieldSizeQ - 1) / 4, FieldSizeQ)) % FieldSizeQ;
                }
                else
                {
                    throw new Exception("Square root does not exist");
                }

            }
            else
            {
                // need to use Tonelli-Shanks algorithm in SP800-186 Appendix E
            }

            if (X == 0 && x == 1)
            {
                throw new Exception("Point Decode failed");
            }

            if (X % 2 == x)
            {
                return new EdPoint(X, Y);
            }
            else
            {
                return new EdPoint(FieldSizeQ - X, Y);
            }
        }
    }
}
