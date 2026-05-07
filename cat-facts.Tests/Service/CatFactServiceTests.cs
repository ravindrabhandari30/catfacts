using cat_facts.Model;
using cat_facts.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.IO;
using Xunit;

namespace cat_facts.Tests.Services
{
    [Collection("Sequential")]

    public class CatFactServiceTests
    {
        private readonly CatFactService _service;
        private readonly string _filePath;
        private readonly Mock<IWebHostEnvironment> _mockEnv;

        public CatFactServiceTests()
        {
            // ✔ Get project root (cat_facts project)
            var basePath = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\.."));

            // ✔ Point to real Data folder in API project
            _filePath = Path.Combine(projectRoot, "Data", "catfact.json");

            // ✔ Ensure folder exists
            var dir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir!);
            }

            // ✔ RESET FILE before each test (VERY IMPORTANT)
            File.WriteAllText(_filePath, "[]");

            // ✔ Mock environment to point to project root
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockEnv.Setup(x => x.ContentRootPath).Returns(projectRoot);

            // ✔ Create service
            _service = new CatFactService(_mockEnv.Object);
        }

        [Fact]
        public void Add_ShouldAddFact_AndReturnIt()
        {
            // Act
            var result = _service.Add("Cats sleep a lot");

            // Assert
            result.Should().NotBeNull();
            result.Fact.Should().Be("Cats sleep a lot");
            result.Id.Should().Be(1);

            var all = _service.GetAll();
            all.Should().HaveCount(1);
        }

        [Fact]
        public void GetAll_ShouldReturnFacts()
        {
            // Arrange
            _service.Add("Fact 1");
            _service.Add("Fact 2");

            // Act
            var result = _service.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Fact == "Fact 1");
            result.Should().Contain(x => x.Fact == "Fact 2");
        }

        [Fact]
        public void GetRandom_ShouldReturnNull_WhenEmpty()
        {
            // Act
            var result = _service.GetRandom();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetRandom_ShouldReturnFact_WhenExists()
        {
            // Arrange
            _service.Add("Fact 1");

            // Act
            var result = _service.GetRandom();

            // Assert
            result.Should().NotBeNull();
            result!.Fact.Should().Be("Fact 1");
        }

        [Fact]
        public void Add_ShouldIncrementIdCorrectly()
        {
            // Act
            var first = _service.Add("First");
            var second = _service.Add("Second");

            // Assert
            first.Id.Should().Be(1);
            second.Id.Should().Be(2);
        }
    }
}