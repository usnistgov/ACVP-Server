using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX963;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX943
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _hashAlg;
        private MathDomain _sharedInfo;
        private MathDomain _keyData;
        private int[] _fieldSize;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "ansix9.63";
            _hashAlg = new[] { "SHA2-224", "SHA2-512" };
            _sharedInfo = new MathDomain().AddSegment(new ValueDomainSegment(0));
            _keyData = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _fieldSize = new[] { 224 };
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMode(string value)
        {
            _mode = value;
            return this;
        }

        public ParameterBuilder WithFieldSize(int[] value)
        {
            _fieldSize = value;
            return this;
        }

        public ParameterBuilder WithHashAlg(string[] value)
        {
            _hashAlg = value;
            return this;
        }

        public ParameterBuilder WithSharedInfoLength(MathDomain value)
        {
            _sharedInfo = value;
            return this;
        }

        public ParameterBuilder WithKeyDataLength(MathDomain value)
        {
            _keyData = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                HashAlg = _hashAlg,
                SharedInfoLength = _sharedInfo,
                KeyDataLength = _keyData,
                FieldSize = _fieldSize
            };
        }
    }
}
