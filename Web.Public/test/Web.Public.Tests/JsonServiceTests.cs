using System;
using NUnit.Framework;
using Web.Public.Models;
using Web.Public.Services;

namespace Web.Public.Tests
{
    [TestFixture]
    public class JsonServiceTests
    {
        [Test]
        [TestCase("[{\"acvVersion\": \"1.0\"},{\"name\": \"test\"}]")]
        [TestCase("[{\"acvVersion\": \"1.0\"},{\"name\": \"test\", \"phoneNumbers\": [{\"number\": \"555-555-0001\", \"type\": \"phone\"}, {\"number\": \"555-555-0002\", \"type\": \"fax\"}]}]")]
        public void ShouldDeserializeOrganizationObjects(string json)
        {
            var jsonParser = new JsonService<Organization>();
            Organization result;
            try
            {
                result = jsonParser.GetObjectFromBodyJson(json);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            
            Assert.Pass();
        }
    }
}