using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
{
//    public class BsonConverter : IBsonConverter
//    {
//        private readonly IList<JsonConverter> _jsonConverters;
//
//        public BsonConverter(IJsonConverterProvider jsonConverterProvider)
//        {
//            _jsonConverters = jsonConverterProvider.GetJsonConverters();
//        }
//
//        public T FromBson<T>(string dataEncBase64)
//        {
//            var data = Convert.FromBase64String(dataEncBase64);
//
//            using (var ms = new MemoryStream(data))
//            {
//                using (var reader = new BsonDataReader(ms))
//                {
//                    var serializer = GetSerializer();
//                    return serializer.Deserialize<T>(reader);
//                }
//            }
//        }
//
//        public string ToBson<T>(T t)
//        {
//            using (MemoryStream ms = new MemoryStream())
//            {
//                using (BsonDataWriter dw = new BsonDataWriter(ms))
//                {
//                    var serializer = GetSerializer();
//                    serializer.Serialize(dw, t);
//                    return Convert.ToBase64String(ms.ToArray());
//                }
//            }
//        }
//
//        private JsonSerializer GetSerializer()
//        {
//            return JsonSerializer.Create(new JsonSerializerSettings()
//            {
//                Converters = _jsonConverters
//            });
//        }
//    }
}