using Ask.Services;
using Moq;
using Moq.Protected;
using System.Net;

namespace Ask.Tests;

public class AskServiceTests
{
    private const string API_URL = "https://example.com/api";
    private const string API_KEY = "knzx097qjlhnsd1poqeh23u";

    [Fact]
    public async void Ask_NullOrEmptyPrompt_Error()
    {
        // Arrange
        var sut = new AskService(API_URL, API_KEY);

        // Act
        var result = await sut.AskAsync(string.Empty);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal("Please enter a prompt", result.Text);
    }

    [Fact]
    public async void Ask_MissingAPIUrl_Error()
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("suppose the api key is invalid") });

        var sut = new AskService(string.Empty, API_KEY, new HttpClient(mockHttpMessageHandler.Object));

        // Act
        var result = await sut.AskAsync("What's the bash command used to list files in a directory");

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal("API url is missing", result.Text);
    }

    [Fact]
    public async void Ask_MissingAPIKey_Error()
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("suppose the api key is invalid") });

        var sut = new AskService(API_KEY, string.Empty, new HttpClient(mockHttpMessageHandler.Object));

        // Act
        var result = await sut.AskAsync("What's the bash command used to list files in a directory");

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal("API key is missing", result.Text);
    }

    [Fact]
    public async void Ask_InvalidJsonResponse_Error()
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("suppose this json is empty or invalid") });

        var sut = new AskService(API_URL, API_KEY, new HttpClient(mockHttpMessageHandler.Object));

        // Act
        var result = await sut.AskAsync("What's the bash command used to list files in a directory");

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal("Something went wrong: the JSON response is invalid", result.Text);
    }

    [Fact]
    public async void Ask_ValidPrompt_Success()
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("{ \"choices\": [{ \"text\": \"ls\"}] }") });

        var sut = new AskService(API_URL, API_KEY, new HttpClient(mockHttpMessageHandler.Object));

        // Act
        var result = await sut.AskAsync("What's the bash command used to list files in a directory");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal("ls", result.Text);
    }
}