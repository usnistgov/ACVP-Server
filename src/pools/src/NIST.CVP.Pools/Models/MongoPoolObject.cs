using MongoDB.Bson;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace NIST.CVP.Pools.Models
{
    public class MongoPoolObject<TResult>
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public TResult Value { get; set; }
    }
}
