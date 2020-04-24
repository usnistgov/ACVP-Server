using System;
using Moq;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using NUnit.Framework;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;
using Web.Public.Services.WorkflowItemPayloadValidators;

namespace Web.Public.Tests
{
    [TestFixture]
    public class JsonServiceTests
    {
        private Mock<IWorkflowItemValidator> _mockWorkflowItemValidator = new Mock<IWorkflowItemValidator>();
        private Mock<IWorkflowItemValidatorFactory> _mockWorkflowItemValidatorFactory = new Mock<IWorkflowItemValidatorFactory>();

        [SetUp]
        public void Setup()
        {
            _mockWorkflowItemValidator
                .Setup(s => s.Validate(It.IsAny<IWorkflowItemPayload>()))
                .Returns(new PayloadValidationResult(null));
            _mockWorkflowItemValidatorFactory
                .Setup(s => s.GetWorkflowItemPayloadValidator(It.IsAny<APIAction>()))
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
                jsonParser.GetWorkflowItemPayloadFromBodyJson<OrganizationCreatePayload>(json, APIAction.CreateVendor);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            
            Assert.Pass();
        }
    }
}