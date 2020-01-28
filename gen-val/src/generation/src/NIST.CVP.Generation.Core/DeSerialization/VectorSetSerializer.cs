using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.JsonConverters;

namespace NIST.CVP.Generation.Core.DeSerialization
{
    public class VectorSetSerializer<TTestVectorSet, TTestGroup, TTestCase> : IVectorSetSerializer<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : class, ITestGroup<TTestGroup, TTestCase>
        where TTestCase : class, ITestCase<TTestGroup, TTestCase>
    {
        protected IJsonConverterProvider JsonConverterProvider;
        protected IContractResolverFactory<TTestGroup, TTestCase> ContractResolverFactory;

        public VectorSetSerializer(
            IJsonConverterProvider jsonConverterProvider, 
            IContractResolverFactory<TTestGroup, TTestCase> contractResolverFactory
        )
        {
            JsonConverterProvider = jsonConverterProvider;
            ContractResolverFactory = contractResolverFactory;
        }

        public string Serialize(TTestVectorSet vectorSet, Projection projection)
        {
            var jsonConverters = JsonConverterProvider.GetJsonConverters().ToList();
            var contractResolver = ContractResolverFactory.GetContractResolver(projection);

            return JsonConvert.SerializeObject(
                vectorSet,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = jsonConverters,
                    ContractResolver = contractResolver
                }
            );
        }
    }
}