using System;
using Moq;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using NUnit.Framework;
using Web.Public.Models;
using Web.Public.Services;

namespace Web.Public.Tests
{
    [TestFixture]
    public class JsonServiceTests
    {
        private Mock<IWorkflowItemPayloadValidatorFactory> _mockWorkflowItemPayloadValidatorFactory = new Mock<IWorkflowItemPayloadValidatorFactory>();
        
        [Test]
        [TestCase("[{\"acvVersion\": \"1.0\"},{\"name\": \"test\"}]")]
        [TestCase("[{\"acvVersion\": \"1.0\"},{\"name\": \"test\", \"phoneNumbers\": [{\"number\": \"555-555-0001\", \"type\": \"phone\"}, {\"number\": \"555-555-0002\", \"type\": \"fax\"}]}]")]
        public void ShouldDeserializeOrganizationObjects(string json)
        {
            var jsonParser = new JsonReaderService(_mockWorkflowItemPayloadValidatorFactory.Object);
            try
            {
                jsonParser.GetWorkflowObjectFromBodyJson<OrganizationCreatePayload>(json, APIAction.CreateVendor);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            
            Assert.Pass();
        }
    }
}