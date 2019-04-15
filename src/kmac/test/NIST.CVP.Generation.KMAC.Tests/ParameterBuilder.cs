﻿using NIST.CVP.Generation.KMAC.v1_0;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KMAC.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private MathDomain _keyLen;
        private MathDomain _macLen;
        private MathDomain _msgLen;
        private bool _xof;
        private bool _nonxof;
        private int[] _digestSizes;
        private bool _hexCustomization;

        /// <summary>
        /// Provides default parameters that are valid (as of construction)
        /// </summary>
        public ParameterBuilder()
        {
            _algorithm = "KMAC";
            _msgLen = new MathDomain();
            _msgLen = _msgLen.AddSegment(new RangeDomainSegment(null, 0, 65536, 8));
            _keyLen = new MathDomain();
            _keyLen = _keyLen.AddSegment(new RangeDomainSegment(null, ParameterValidator._MIN_KEY_LENGTH, ParameterValidator._MAX_KEY_LENGTH, 8));
            _macLen = new MathDomain();
            _macLen = _macLen.AddSegment(new RangeDomainSegment(null, 32, 65536, 8));
            _xof = false;
            _nonxof = true;
            _digestSizes = new int[] { 128, 256 };
            _hexCustomization = false;
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMsgLen(MathDomain value)
        {
            _msgLen = value;
            return this;
        }

        public ParameterBuilder WithKeyLen(MathDomain value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithMacLen(MathDomain value)
        {
            _macLen = value;
            return this;
        }

        public ParameterBuilder WithXOF(bool value)
        {
            _xof = value;
            return this;
        }

        public ParameterBuilder WithNonXOF(bool value)
        {
            _nonxof = value;
            return this;
        }

        public ParameterBuilder WithHexCustomization(bool value)
        {
            _hexCustomization = value;
            return this;
        }

        public ParameterBuilder WithDigestSizes(int[] values)
        {
            _digestSizes = GetDeepCopy(values);
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                MsgLen = _msgLen,
                KeyLen = _keyLen,
                MacLen = _macLen,
                DigestSizes = _digestSizes,
                XOF = _xof,
                NonXOF = _nonxof,
                HexCustomization = _hexCustomization
            };
        }

        private int[] GetDeepCopy(int[] values)
        {
            var copy = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                copy[i] = values[i];
            }
            return copy;
        }
    }
}
