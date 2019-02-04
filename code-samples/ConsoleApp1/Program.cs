using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://localhost");
            var server = client.GetDatabase("myHelloWorld");

            var collection = server.GetCollection<SomeObject>("SomeObjects");
            Console.WriteLine($"Count: {collection.CountDocuments(c => !string.IsNullOrEmpty(c.Message))}");

            collection.InsertOne(new SomeObject()
            {
                ObjectCreation = DateTime.Now,
                Message = "Hello World!"
            });

            Console.WriteLine($"Count: {collection.CountDocuments(c => !string.IsNullOrEmpty(c.Message))}");
        }
    }
}
