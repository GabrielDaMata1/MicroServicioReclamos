using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Services;
using Domain.Interfaces;
using Moq;
using Moq.Protected;
using Xunit;

public class UsuarioServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5001/")
        };

        _service = new UsuarioService(_httpClient);
    }

    [Fact]
    public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarGuid_CuandoRespuestaExitosa()
    {
        // Arrange
        var correo = "test@example.com";
        var expectedGuid = Guid.NewGuid();

        var responseContent = new StringContent($"\"{expectedGuid}\"");

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains($"IdUsuario/{correo}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseContent
            });

        // Act
        var result = await _service.ObtenerUsuarioPorIdAsync(correo);

        // Assert
        Assert.Equal(expectedGuid, result);
    }

    [Fact]
    public async Task ObtenerUsuarioPorIdAsync_DeberiaRetornarGuidEmpty_CuandoRespuestaFalla()
    {
        // Arrange
        var correo = "test@example.com";

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act
        var result = await _service.ObtenerUsuarioPorIdAsync(correo);

        // Assert
        Assert.Equal(Guid.Empty, result);
    }

    [Fact]
    public async Task ObtenerCorreoPorIdAsync_DeberiaRetornarCorreo_CuandoRespuestaExitosa()
    {
        // Arrange
        var idUsuario = Guid.NewGuid();
        var expectedCorreo = "user@test.com";

        var responseContent = new StringContent(expectedCorreo);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains($"Correo/{idUsuario}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseContent
            });

        // Act
        var result = await _service.ObtenerCorreoPorIdAsync(idUsuario);

        // Assert
        Assert.Equal(expectedCorreo, result);
    }

    [Fact]
    public async Task ObtenerCorreoPorIdAsync_DeberiaRetornarNull_CuandoRespuestaFalla()
    {
        // Arrange
        var idUsuario = Guid.NewGuid();

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act
        var result = await _service.ObtenerCorreoPorIdAsync(idUsuario);

        // Assert
        Assert.Null(result);
    }
}
