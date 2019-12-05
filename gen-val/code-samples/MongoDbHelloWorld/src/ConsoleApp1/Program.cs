using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace ConsoleApp1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost");
            var server = client.GetDatabase("myHelloWorld");

            var collection = server.GetCollection<SomeObject>("SomeObjects");
            Console.WriteLine($"Count: {collection.CountDocuments(c => !string.IsNullOrEmpty(c.Message))}");

            var input = Console.ReadLine();

            collection.InsertOne(new SomeObject()
            {
                Id = ObjectId.GenerateNewId(),  // UUID for the db
                ObjectCreation = DateTime.Now,
                Message = input
            });

            Console.WriteLine($"Count: {collection.CountDocuments(c => !string.IsNullOrEmpty(c.Message))}");

            var filter = new FilterDefinitionBuilder<SomeObject>().Empty;   // Finds anything
            var someObject = collection.FindOneAndDelete(filter);           // Remove entry after finding it
            Console.WriteLine($"SomeObject: {someObject.Message}, {someObject.ObjectCreation}");

            Console.ReadLine();
        }
    }
}
