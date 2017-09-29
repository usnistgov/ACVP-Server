using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class SchemeBuilder
    {
        private NoKdfNoKc _noKdfNoKc;
        private KdfNoKc _kdfNoKc;
        private KdfKc _kdfKc;
        private string[] _role;

        public SchemeBuilder()
        {
            _role = new string[] { "initiator", "responder" };
        }

        public static SchemeBuilder GetBaseDhEphemBuilder()
        {
            // TODO
            return new SchemeBuilder()
            {
                _noKdfNoKc = new NoKdfNoKcBuilder().BuildNoKdfNoKc(),
                _kdfNoKc = new KdfNoKcBuilder().BuildKdfNoKc()
            };
        }

        public SchemeBuilder WithRole(string[] value)
        {
            _role = value;
            return this;
        }

        public SchemeBuilder WithNoKdfNoKc(NoKdfNoKc value)
        {
            _noKdfNoKc = value;
            return this;
        }

        public SchemeBuilder WithKdfNoKc(KdfNoKc value)
        {
            _kdfNoKc = value;
            return this;
        }

        public SchemeBuilder WithKdfKc(KdfKc value)
        {
            _kdfKc = value;
            return this;
        }

        public DhEphem BuildDhEphem()
        {
            return new DhEphem()
            {
                NoKdfNoKc = _noKdfNoKc,
                KdfNoKc = _kdfNoKc,
                KdfKc = _kdfKc,
                Role = _role
            };
        }
    }
}