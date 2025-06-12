using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GenValApp.Controllers;
using GenValAppRunner.DTO;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Generation.GenValApp.Models;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Common;
using Newtonsoft.Json;
using System;
using Autofac;

namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests.Controllers
{
public class VectorSetsControllerTests
{
    [Test]
    public async Task Generate_ReturnsOk_WithExpectedResponse()
    {
        // Arrange
        var mockResolver = new Mock<IGeneratorResolver>();
        var mockGenerator = new Mock<IGenerator>();
        var mockScope = new Mock<ILifetimeScope>();

        var testVectorSet = new TestVectorSet { VsId = 1, Algorithm = "ACVP-AES-CBC" };
        var serializedProjection = JsonConvert.SerializeObject(testVectorSet);

        var generateResponse = new GenerateResponse
        {
            StatusCode = 0,
            ErrorMessage = null,
            InternalProjection = serializedProjection
        };

        mockGenerator
            .Setup(g => g.GenerateAsync(It.IsAny<GenerateRequest>()))
            .ReturnsAsync(generateResponse);

        mockResolver
            .Setup(r => r.Resolve(It.IsAny<AlgoMode>()))
            .Returns((mockGenerator.Object, mockScope.Object));

        var controller = new VectorSetsController(mockResolver.Object);

        var registration = new Registration
        {
            VsId = 1,
            Algorithm = "ACVP-AES-CBC",
            Revision = "1.0"
        };

        // Act
        var result = await controller.Generate(registration);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        var response = okResult.Value as VectorSetResponse;
        Assert.That(response, Is.Not.Null);

        Assert.That(response.StatusCode, Is.EqualTo((StatusCode)0));
        Assert.That(response.ErrorMessage, Is.Null);
        Assert.That(response.Result, Is.Not.Null);
        Assert.That(response.Result.Algorithm,Is.EqualTo("ACVP-AES-CBC"));
        Assert.That(response.Result.VsId,Is.EqualTo(1));
    }

    [Test]
    public async Task Generate_Returns500_ForInvalidAlgorithm()
    {
    // Arrange
    var mockGeneratorResolver = new Mock<IGeneratorResolver>();
    var controller = new VectorSetsController(mockGeneratorResolver.Object);

    var invalidRegistration = new Registration
    {
        Algorithm = "INVALID_ALGO",
        Revision = "1.0"
    };

    // Act
    var result = await controller.Generate(invalidRegistration);

    // Assert the result is an ObjectResult (e.g. 500 status)
    Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
    var statusResult = (ObjectResult)result.Result;

    // Extract the error object from the Value property of ObjectResult
    var errorObject = statusResult.Value as dynamic;  // or use a known type if you have one

    // Check the status code
    Assert.That(statusResult.StatusCode, Is.EqualTo(500));

    }
    [Test]
    public async Task Generate_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
      // Arrange
    var mockResolver = new Mock<IGeneratorResolver>();
    var controller = new VectorSetsController(mockResolver.Object);
    controller.ModelState.AddModelError("Algorithm", "The Algorithm field is required.");

    var registration = new Registration
    {
        // Algorithm missing intentionally
        Revision = "1.0"
    };

    // Act
    var result = await controller.Generate(registration);

    // Assert
    Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    var badRequestResult = (BadRequestObjectResult)result.Result;

    Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

    var error = badRequestResult.Value as SerializableError;
    Assert.That(error, Is.Not.Null);
    Assert.That(error.ContainsKey("Algorithm"), "Expected 'Algorithm' key in ModelState error.");

    }
}
}