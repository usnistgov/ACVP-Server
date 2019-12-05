using MongoDB.Bson;
using System;

namespace ConsoleApp1
{
    public class SomeObject
    {
        public ObjectId Id { get; set; }    // Needs an ObjectId, or Mongo doesn't know how to read it from a db
        public DateTime ObjectCreation { get; set; }
        public string Message { get; set; }
    }
}
