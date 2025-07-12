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

public class ReclamoRegistradoConsumerTests
{
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly ReclamoRegistradoConsumer _consumer;

    public ReclamoRegistradoConsumerTests()
    {
        _mockReclamoService = new Mock<IReclamoService>();
        _consumer = new ReclamoRegistradoConsumer(_mockReclamoService.Object);
    }

    [Fact]
    public async Task Consume_DeberiaRegistrarReclamo_CuandoEventoRecibidoCorrectamente()
    {
        // Arrange
        var reclamo = new Reclamo(
            idUsuario: Guid.NewGuid(),
            idSubasta: Guid.NewGuid(),
            descripcion: new DescripcionReclamoVO("Producto dañado"),
            motivo: new MotivoReclamoVO("Defecto"),
            urlEvidencia: new UrlEvidenciaReclamoVO("https://firebase.com/evidencia.jpg"), // provide dummy URL
            fechaCreacion: new FechaCreacionReclamoVO(DateTime.UtcNow),
            estadoReclamo: new EstadoReclamoVO("Pendiente")
        );

        var evento = new ReclamoRegistradoEvent(reclamo);

        var contextMock = new Mock<ConsumeContext<ReclamoRegistradoEvent>>();
        contextMock.Setup(x => x.Message).Returns(evento);

        _mockReclamoService.Setup(s => s.RegistrarReclamoMongoAsync(reclamo))
            .ReturnsAsync(System.Net.HttpStatusCode.Created);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        _mockReclamoService.Verify(s => s.RegistrarReclamoMongoAsync(reclamo), Times.Once);
    }


    [Fact]
    public async Task Consume_DeberiaLanzarFalloAlRegistrarReclamoException_CuandoOcurreErrorEnRegistro()
    {
        // Arrange
        var reclamo = new Reclamo(
            idUsuario: Guid.NewGuid(),
            idSubasta: Guid.NewGuid(),
            descripcion: new DescripcionReclamoVO("Producto incompleto"),
            motivo: new MotivoReclamoVO("Error de envío"),
            urlEvidencia: new UrlEvidenciaReclamoVO("https://firebase.com/evidencia.jpg"), // Dummy URL
            fechaCreacion: new FechaCreacionReclamoVO(DateTime.UtcNow),
            estadoReclamo: new EstadoReclamoVO("Pendiente")
        );

        var evento = new ReclamoRegistradoEvent(reclamo);

        var contextMock = new Mock<ConsumeContext<ReclamoRegistradoEvent>>();
        contextMock.Setup(x => x.Message).Returns(evento);

        _mockReclamoService.Setup(s => s.RegistrarReclamoMongoAsync(reclamo))
            .ThrowsAsync(new Exception("Error en base de datos"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() => _consumer.Consume(contextMock.Object));

        Assert.Equal("Ha ocurrido un error al registrar el reclamo en MongoDb", ex.Message);

        _mockReclamoService.Verify(s => s.RegistrarReclamoMongoAsync(reclamo), Times.Once);
    }

}
