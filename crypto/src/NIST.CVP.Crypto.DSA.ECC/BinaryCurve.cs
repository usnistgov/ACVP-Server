using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NLog;

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

        public BinaryCurve(BigInteger f, BigInteger a, BigInteger b, EccPoint basis, BigInteger n, int h)
        {
            FieldSizeQ = f;
            CoefficientA = a;
            CoefficientB = b;
            BasePointG = basis;
            OrderN = n;
            CofactorH = h;

            _operator = new GaloisFieldOperator(f);
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
            return Multiply(startPoint, new NonAdjacentBitString(scalar));
        }

        public EccPoint Multiply(EccPoint startPoint, NonAdjacentBitString nafBs)
        {
            LoggingHelper.ConfigureLogging("binarycurvetest", "binary");
            var logger = LogManager.GetCurrentClassLogger();

            //var point = new EccPoint(startPoint.X, startPoint.Y);
            var point = new EccPoint("infinity");
            var naBits = nafBs.Bits;
            
            for (var i = naBits.Length - 1; i >= 0; i--)
            {
                point = Double(point);
                if (naBits[i] == 1)
                {
                    logger.Error($"Before addition = {new BitString(point.X).ToHex()}, {new BitString(point.Y).ToHex()}");
                    point = Add(point, startPoint);
                    logger.Error($"After addition = {new BitString(point.X).ToHex()}, {new BitString(point.Y).ToHex()}");
                }
                else if (naBits[i] == -1)
                {
                    logger.Error($"Before subtraction = {new BitString(point.X).ToHex()}, {new BitString(point.Y).ToHex()}");
                    point = Subtract(point, startPoint);
                    logger.Error($"After subtraction = {new BitString(point.X).ToHex()}, {new BitString(point.Y).ToHex()}");
                }
            }

            return point;
        }

        public bool PointExistsOnCurve(EccPoint point)
        {
            throw new NotImplementedException();
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
