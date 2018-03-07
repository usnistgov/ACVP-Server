using System;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.JsonConverters;

namespace NIST.CVP.Generation.Core.DeSerialization
{
    public class VectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase> : IVectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : class, ITestCase<TTestGroup, TTestCase>
    {
        protected IJsonConverterProvider JsonConverterProvider;

        public VectorSetDeserializer(IJsonConverterProvider jsonConverterProvider)
        {
            JsonConverterProvider = jsonConverterProvider;
        }

        public virtual TTestVectorSet Deserialize(string vectorSetJson)
        {
            var jsonConverters = JsonConverterProvider.GetJsonConverters().ToList();

            var testVectorSet = JsonConvert.DeserializeObject<TTestVectorSet>(
                vectorSetJson,
                new JsonSerializerSettings()
                {
                    Converters = jsonConverters
                }
            );

            AssociateParentsToChildren(testVectorSet);

            return testVectorSet;
        }

        private void AssociateParentsToChildren(TTestVectorSet testVectorSet)
        {
            foreach (var group in testVectorSet.TestGroups)
            {
                foreach (var test in group.Tests)
                {
                    test.ParentGroup = group;
                }
            }
        }
    }
}