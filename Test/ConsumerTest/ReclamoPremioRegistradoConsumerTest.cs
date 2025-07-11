using System;
using System.Threading.Tasks;
using Application.Exception;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Domain.Value_Object;
using Infrastructure.Consumer;
using MassTransit;
using Moq;
using Xunit;

public class ReclamoPremioRegistradoConsumerTests
{
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly ReclamoPremioRegistradoConsumer _consumer;

    public ReclamoPremioRegistradoConsumerTests()
    {
        _mockReclamoService = new Mock<IReclamoService>();
        _consumer = new ReclamoPremioRegistradoConsumer(_mockReclamoService.Object);
    }

    [Fact]
    public async Task Consume_DeberiaRegistrarReclamoPremio_CuandoEventoRecibidoCorrectamente()
    {
        // Arrange
        var reclamoPremio = new ReclamoPremio(
            id: Guid.NewGuid(),
            idUsuario: Guid.NewGuid(),
            idSubasta: Guid.NewGuid(),
            direccionEnvio: new DireccionEnvioPremioVO("Calle 123"),
            metodoEntrega: new MetodoEntregaPremioVO("Delivery"),
            fechaReclamo: new FechaReclamoPremioVO(DateTime.UtcNow)
        );

        var evento = new ReclamoPremioRegistradoEvent(reclamoPremio);

        var contextMock = new Mock<ConsumeContext<ReclamoPremioRegistradoEvent>>();
        contextMock.Setup(x => x.Message).Returns(evento);

        _mockReclamoService
            .Setup(s => s.RegistrarReclamoPremioMongoAsync(reclamoPremio))
            .ReturnsAsync(System.Net.HttpStatusCode.Created);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        _mockReclamoService.Verify(s => s.RegistrarReclamoPremioMongoAsync(reclamoPremio), Times.Once);
    }


    [Fact]
    public async Task Consume_DeberiaLanzarFalloAlRegistrarReclamoException_CuandoOcurreErrorEnRegistro()
    {
        // Arrange
        var reclamoPremio = new ReclamoPremio(
            id: Guid.NewGuid(),
            idUsuario: Guid.NewGuid(),
            idSubasta: Guid.NewGuid(),
            direccionEnvio: new DireccionEnvioPremioVO("Av. Siempre Viva"),
            metodoEntrega: new MetodoEntregaPremioVO("Pickup"),
            fechaReclamo: new FechaReclamoPremioVO(DateTime.UtcNow)
        );

        var evento = new ReclamoPremioRegistradoEvent(reclamoPremio);

        var contextMock = new Mock<ConsumeContext<ReclamoPremioRegistradoEvent>>();
        contextMock.Setup(x => x.Message).Returns(evento);

        _mockReclamoService.Setup(s => s.RegistrarReclamoPremioMongoAsync(reclamoPremio))
            .ThrowsAsync(new Exception("Error en base de datos"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(
            () => _consumer.Consume(contextMock.Object)
        );

        Assert.Equal("Ha ocurrido un error al registrar el reclamo del premio en MongoDb", ex.Message);

        _mockReclamoService.Verify(s => s.RegistrarReclamoPremioMongoAsync(reclamoPremio), Times.Once);
    }
}
