using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Value_Object;
using Moq;
using Moq.Protected;
using Xunit;

public class SubastaServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly SubastaService _service;

    public SubastaServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5003/")
        };

        _service = new SubastaService(_httpClient);
    }

    [Fact]
    public async Task ObtenerSubastaPorGuid_DebeRetornarSubasta_CuandoRespuestaExitosa()
    {
        // Arrange
        var subastaId = Guid.NewGuid();

        var dto = new SubastaDTO
        {
            Id = subastaId,
            nombreSubasta = "Subasta Test",
            descripcionSubasta = "Descripción",
            idProductoSubasta = Guid.NewGuid(),
            fechaInicioSubasta = DateTime.UtcNow,
            fechaFinSubasta = DateTime.UtcNow.AddDays(1),
            incrementoMinimoSubasta = 5,
            precioReservaSubasta = 100,
            estadoSubasta = "Abierta",
            idUsuario = Guid.NewGuid()
        };

        var responseContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(dto));

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseContent
            });

        // Act
        var result = await _service.ObtenerSubastaPorGuid(subastaId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Id, result.Id);
        Assert.Equal(dto.nombreSubasta, result.nombreSubasta.Nombre);
        Assert.Equal(dto.descripcionSubasta, result.descripcionSubasta.descripcion);
        Assert.Equal(dto.estadoSubasta, result.estadoSubasta.estado);
    }

    [Fact]
    public async Task ObtenerSubastaPorGuid_DebeRetornarNull_CuandoRespuestaFalla()
    {
        // Arrange
        var subastaId = Guid.NewGuid();

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act
        var result = await _service.ObtenerSubastaPorGuid(subastaId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ObtenerSubastaPorGuid_DebeRetornarNull_CuandoExcepcion()
    {
        // Arrange
        var subastaId = Guid.NewGuid();

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new Exception("Error"));

        // Act
        var result = await _service.ObtenerSubastaPorGuid(subastaId);

        // Assert
        Assert.Null(result);
    }
}
