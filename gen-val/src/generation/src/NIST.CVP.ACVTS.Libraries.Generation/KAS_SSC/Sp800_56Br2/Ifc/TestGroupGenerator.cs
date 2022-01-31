using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private static readonly string[] TestTypes =
        {
            "AFT",
            "VAL"
        };

        private int _registeredSchemeCount;

        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            var schemes = parameters.Scheme.GetRegisteredSchemes().ToList();
            _registeredSchemeCount = schemes.Count();

            var testTypeQueue = new ShuffleQueue<string>(TestTypes.ToList());
            var keyGenQueue = GetKeyGenMethods(parameters.KeyGenerationMethods);
            var moduloQueue = GetModulos(parameters.Modulo);

            foreach (var scheme in parameters.Scheme.GetRegisteredSchemes())
            {

                var roleQueue = new ShuffleQueue<KeyAgreementRole>(scheme.KasRole.ToList());

                var maxCount = new[]
                {
                    testTypeQueue.OriginalListCount,
                    keyGenQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount),
                    moduloQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount),
                    roleQueue.OriginalListCount
                }.Max();

                for (var i = 0; i < maxCount; i++)
                {
                    var exponent = parameters.FixedPublicExponent == 0
                        ? BigInteger.Zero
                        : parameters.FixedPublicExponent;

                    groups.Add(new TestGroup()
                    {
                        IsSample = parameters.IsSample,
                        Modulo = moduloQueue.Pop(),
                        Scheme = scheme.Scheme,
                        KasRole = roleQueue.Pop(),
                        PublicExponent = exponent,
                        TestType = testTypeQueue.Pop(),
                        KeyGenerationMethod = keyGenQueue.Pop(),
                        HashFunctionZ = parameters.HashFunctionZ
                    });
                }
            }

            await GenerateKeysForGroups(groups);

            return groups;
        }

        private ShuffleQueue<IfcKeyGenerationMethod> GetKeyGenMethods(IfcKeyGenerationMethod[] keyGenMethods)
        {
            return new ShuffleQueue<IfcKeyGenerationMethod>(keyGenMethods.ToList());
        }

        private ShuffleQueue<int> GetModulos(IEnumerable<int> modulo)
        {
            return new ShuffleQueue<int>(modulo.ToList());
        }

        private async Task GenerateKeysForGroups(List<TestGroup> groups)
        {
            var moduloExponentPairs = groups.Select(s => new
            {
                s.Modulo,
                s.PublicExponent,
                s.PrivateKeyMode

            }).Distinct();
            var tasks = new Dictionary<(int modulo, BigInteger publicExponent, PrivateKeyModes privateKeyMode), List<Task<KeyPair>>>();

            foreach (var pair in moduloExponentPairs)
            {
                var list = new List<Task<KeyPair>>();
                for (var i = 0; i < 100; i++)
                {
                    list.Add(GetKey(pair.Modulo, pair.PublicExponent, pair.PrivateKeyMode));
                }

                tasks.Add((pair.Modulo, pair.PublicExponent, pair.PrivateKeyMode), list);
            }

            await Task.WhenAll(tasks.SelectMany(s => s.Value));

            var shuffleQueues = new Dictionary<(int modulo, BigInteger publicExponent, PrivateKeyModes privateKeyMode), ShuffleQueue<KeyPair>>();
            foreach (var kvp in tasks)
            {
                List<KeyPair> keys = new List<KeyPair>();
                foreach (var task in kvp.Value)
                {
                    keys.Add(await task);
                }
                shuffleQueues.Add(kvp.Key, new ShuffleQueue<KeyPair>(keys));
            }

            foreach (var group in groups)
            {
                group.ShuffleKeys = shuffleQueues
                    .First(f => f.Key.modulo == group.Modulo &&
                                f.Key.publicExponent == group.PublicExponent &&
                                f.Key.privateKeyMode == group.PrivateKeyMode).Value;
            }
        }

        private async Task<KeyPair> GetKey(int modulo, BigInteger publicExponent, PrivateKeyModes privateKeyMode)
        {
            var task = _oracle.GetRsaKeyAsync(new RsaKeyParameters()
            {
                Modulus = modulo,
                Standard = Fips186Standard.Fips186_5,
                KeyFormat = privateKeyMode,
                KeyMode = PrimeGenModes.RandomProbablePrimes,
                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                PublicExponentMode = publicExponent == 0 ? PublicExponentModes.Random : PublicExponentModes.Fixed,
                PublicExponent = publicExponent == 0 ? null : new BitString(publicExponent)
            });

            var result = await task;
            return result.Key;
        }
    }
}
