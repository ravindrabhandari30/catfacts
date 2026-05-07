using System.Collections.Generic;
using cat_facts.Controllers;
using cat_facts.Model;
using cat_facts.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace cat_facts.Tests.Controllers
{
    public class CatFactsControllerTests
    {
        private readonly Mock<ICatFactService> _mockService;
        private readonly CatFactsController _controller;

        public CatFactsControllerTests()
        {
            _mockService = new Mock<ICatFactService>();
            _controller = new CatFactsController(_mockService.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOkResult_WithFacts()
        {
            // Arrange
            var facts = new List<CatFact>
            {
                new CatFact
                {
                    Id = 1,
                    Fact = "Cats sleep 16 hours"
                },
                new CatFact
                {
                    Id = 2,
                    Fact = "Cats have whiskers"
                }
            };

            _mockService.Setup(s => s.GetAll()).Returns(facts);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var returnedFacts = okResult.Value as List<CatFact>;

            returnedFacts.Should().NotBeNull();
            returnedFacts.Should().BeEquivalentTo(facts);
        }

        [Fact]
        public void GetRandom_ShouldReturnOk_WhenFactExists()
        {
            // Arrange
            var fact = new CatFact
            {
                Id = 1,
                Fact = "Cats can jump high"
            };

            _mockService.Setup(s => s.GetRandom()).Returns(fact);

            // Act
            var result = _controller.GetRandom();

            // Assert
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var returnedFact = okResult.Value as CatFact;

            returnedFact.Should().NotBeNull();
            returnedFact.Should().BeEquivalentTo(fact);
        }

        [Fact]
        public void GetRandom_ShouldReturnNotFound_WhenNoFactExists()
        {
            // Arrange
            _mockService.Setup(s => s.GetRandom()).Returns((CatFact?)null);

            // Act
            var result = _controller.GetRandom();

            // Assert
            var notFoundResult = result as NotFoundObjectResult;

            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be("No facts available");
        }

        [Fact]
        public void Add_ShouldReturnBadRequest_WhenFactIsEmpty()
        {
            // Arrange
            string fact = "";

            // Act
            var result = _controller.Add(fact);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;

            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Fact cannot be empty");
        }

        [Fact]
        public void Add_ShouldReturnOk_WhenFactIsValid()
        {
            // Arrange
            string fact = "Cats purr";

            var addedFact = new CatFact
            {
                Id = 1,
                Fact = fact
            };

            _mockService.Setup(s => s.Add(fact)).Returns(addedFact);

            // Act
            var result = _controller.Add(fact);

            // Assert
            var okResult = result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var returnedFact = okResult.Value as CatFact;

            returnedFact.Should().NotBeNull();
            returnedFact!.Fact.Should().Be(fact);
            returnedFact.Id.Should().Be(1);
        }
    }
}