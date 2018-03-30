using System;

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

        // TODO more schemes

        public static SchemeBuilder GetBaseDhEphemBuilder()
        {
            return new SchemeBuilder()
            {
                _noKdfNoKc = new NoKdfNoKcBuilder().BuildNoKdfNoKc(),
                _kdfNoKc = new KdfNoKcBuilder().BuildKdfNoKc()
            };
        }

        public static SchemeBuilder GetBaseMqv1Builder()
        {
            return new SchemeBuilder()
            {
                _noKdfNoKc = new NoKdfNoKcBuilder().BuildNoKdfNoKc(),
                _kdfNoKc = new KdfNoKcBuilder().BuildKdfNoKc(),
                _kdfKc = new KdfKcBuilder().Build()
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

        public T Build<T>()
            where T : SchemeBase
        {
            T instance = Activator.CreateInstance<T>();

            instance.NoKdfNoKc = _noKdfNoKc;
            instance.KdfNoKc = _kdfNoKc;
            instance.KdfKc = _kdfKc;
            instance.Role = _role;

            return instance;
        }
    }
}