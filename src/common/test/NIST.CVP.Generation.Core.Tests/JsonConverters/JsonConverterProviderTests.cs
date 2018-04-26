﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.JsonConverters
{
    [TestFixture, UnitTest]
    public class JsonConverterProviderTests
    {
        private IJsonConverterProvider _subject;
        #region FakeJsonConverterProvider
        private class TestJsonConverterProvider : JsonConverterProvider
        {
            private class FakeJsonConverter : JsonConverter
            {
                public override bool CanConvert(Type objectType)
                {
                    throw new NotImplementedException();
                }

                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }
            }

            protected override void AddAdditionalConverters(HashSet<JsonConverter> registeredConverters)
            {
                registeredConverters.Add(new FakeJsonConverter());
            }
        }
        #endregion FakeJsonConverterProvider

        public void ShouldContainConvertersByDefault()
        {
            _subject = new JsonConverterProvider();

            var result = _subject.GetJsonConverters();

            Assert.IsTrue(result.Any());
        }

        public void ShouldAllowAdditionalConvertersThroughExtension()
        {
            _subject = new JsonConverterProvider();
            var originalCount = _subject.GetJsonConverters().Count();

            _subject = new TestJsonConverterProvider();
            var results = _subject.GetJsonConverters();

            Assert.IsTrue(results.Count() > originalCount);
        }
    }
}