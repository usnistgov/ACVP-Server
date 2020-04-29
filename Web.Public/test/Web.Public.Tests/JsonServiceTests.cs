using System;
using Moq;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NUnit.Framework;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;
using Web.Public.Services.MessagePayloadValidators;

namespace Web.Public.Tests
{
    [TestFixture]
    public class JsonServiceTests
    {
        private Mock<IMessagePayloadValidator> _mockWorkflowItemValidator = new Mock<IMessagePayloadValidator>();
        private Mock<IMessagePayloadValidatorFactory> _mockWorkflowItemValidatorFactory = new Mock<IMessagePayloadValidatorFactory>();

        [SetUp]
        public void Setup()
        {
            _mockWorkflowItemValidator
                .Setup(s => s.Validate(It.IsAny<IWorkflowItemPayload>()))
                .Returns(new PayloadValidationResult(null));
            _mockWorkflowItemValidatorFactory
                .Setup(s => s.GetMessagePayloadValidator(It.IsAny<APIAction>()))
                .Returns(_mockWorkflowItemValidator.Object);
        }
        
        [Test]
        [TestCase("[{\"acvVersion\": \"1.0\"},{\"name\": \"test\"}]")]
        [TestCase("[{\"acvVersion\": \"1.0\"},{\"name\": \"test\", \"phoneNumbers\": [{\"number\": \"555-555-0001\", \"type\": \"phone\"}, {\"number\": \"555-555-0002\", \"type\": \"fax\"}]}]")]
        public void ShouldDeserializeOrganizationObjects(string json)
        {
            var jsonParser = new JsonReaderService(_mockWorkflowItemValidatorFactory.Object);
            try
            {
                jsonParser.GetMessagePayloadFromBodyJson<OrganizationCreatePayload>(json, APIAction.CreateVendor);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            
            Assert.Pass();
        }
    }
}