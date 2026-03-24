using NUnit.Framework;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using DevTeamAIAssistant.Services;

namespace DevTeamAIAssistant.Tests.Services;

[TestFixture]
public class ClaudeServiceTests
{
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<IHttpClientFactory> _mockHttpClientFactory;

    [SetUp]
    public void SetUp()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttpClientFactory.Setup(f => f.CreateClient("Anthropic")).Returns(new HttpClient());
    }

    [Test]
    public void Constructor_WithoutApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockConfiguration.Setup(c => c["Anthropic:ApiKey"]).Returns((string?)null);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new ClaudeService(_mockConfiguration.Object, _mockHttpClientFactory.Object));

        exception?.Message.Should().Contain("API key not configured");
    }

    [Test]
    public void Constructor_WithApiKey_CreatesService()
    {
        // Arrange
        _mockConfiguration.Setup(c => c["Anthropic:ApiKey"]).Returns("sk-ant-api03-test-key");
        _mockConfiguration.Setup(c => c["Anthropic:Model"]).Returns("claude-sonnet-4-20250514");

        // Act
        var service = new ClaudeService(_mockConfiguration.Object, _mockHttpClientFactory.Object);

        // Assert
        service.Should().NotBeNull();
    }

    [Test]
    public void Constructor_WithoutModel_UsesDefaultModel()
    {
        // Arrange
        _mockConfiguration.Setup(c => c["Anthropic:ApiKey"]).Returns("sk-ant-api03-test-key");
        _mockConfiguration.Setup(c => c["Anthropic:Model"]).Returns((string?)null);

        // Act
        var service = new ClaudeService(_mockConfiguration.Object, _mockHttpClientFactory.Object);

        // Assert
        service.Should().NotBeNull();
    }

    [TestCase("")]
    [TestCase("   ")]
    public void Constructor_WithEmptyApiKey_ThrowsException(string emptyKey)
    {
        // Arrange
        _mockConfiguration.Setup(c => c["Anthropic:ApiKey"]).Returns(emptyKey);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            new ClaudeService(_mockConfiguration.Object, _mockHttpClientFactory.Object));
    }
}