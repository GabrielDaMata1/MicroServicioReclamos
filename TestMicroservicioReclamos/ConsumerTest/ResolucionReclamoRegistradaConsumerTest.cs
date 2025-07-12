using System;
using System.Net;
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

public class ResolucionReclamoRegistradaConsumerTest
{
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly ResolucionReclamoRegistradaConsumer _consumer;

    public ResolucionReclamoRegistradaConsumerTest()
    {
        _mockReclamoService = new Mock<IReclamoService>();
        _consumer = new ResolucionReclamoRegistradaConsumer(_mockReclamoService.Object);
    }

    [Fact]
    public async Task Consume_DeberiaRegistrarResolucionYActualizarEstado_CuandoTodoSaleBien()
    {
        // Arrange
        var resolucion = new ResolucionReclamo(
            id: Guid.NewGuid(),
            idReclamo: Guid.NewGuid(),
            descripcion: new DescripcionResolucionVO("Problema solucionado"),
            fechaResolucion: new FechaResolucionVO(DateTime.UtcNow)
        );

        var evento = new ResolucionReclamoRegistradaEvent(resolucion, resolucion.IdReclamo);

        var contextMock = new Mock<ConsumeContext<ResolucionReclamoRegistradaEvent>>();
        contextMock.Setup(c => c.Message).Returns(evento);

        _mockReclamoService.Setup(s => s.RegistrarResolucionReclamoMongoAsync(resolucion))
            .ReturnsAsync(HttpStatusCode.Created);

        _mockReclamoService.Setup(s => s.ActualizarEstadoReclamoMongoAsync(resolucion.IdReclamo, "Resuelto"))
            .ReturnsAsync(HttpStatusCode.OK);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        _mockReclamoService.Verify(s => s.RegistrarResolucionReclamoMongoAsync(resolucion), Times.Once);
        _mockReclamoService.Verify(s => s.ActualizarEstadoReclamoMongoAsync(resolucion.IdReclamo, "Resuelto"), Times.Once);
    }


    [Fact]
    public async Task Consume_DeberiaLanzarFalloAlRegistrarReclamoException_CuandoFalleElRegistroDeResolucion()
    {
        // Arrange
        var resolucion = new ResolucionReclamo(
            id: Guid.NewGuid(),
            idReclamo: Guid.NewGuid(),
            descripcion: new DescripcionResolucionVO("Problema solucionado"),
            fechaResolucion: new FechaResolucionVO(DateTime.UtcNow)
        );
        var evento = new ResolucionReclamoRegistradaEvent(resolucion, resolucion.IdReclamo);

        var contextMock = new Mock<ConsumeContext<ResolucionReclamoRegistradaEvent>>();
        contextMock.Setup(c => c.Message).Returns(evento);

        _mockReclamoService.Setup(s => s.RegistrarResolucionReclamoMongoAsync(resolucion))
            .ThrowsAsync(new Exception("Error inesperado"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() => _consumer.Consume(contextMock.Object));
        Assert.Contains("Ha ocurrido un error al registrar la resolucion del reclamo en MongoDb", ex.Message);

        _mockReclamoService.Verify(s => s.RegistrarResolucionReclamoMongoAsync(resolucion), Times.Once);
        _mockReclamoService.Verify(s => s.ActualizarEstadoReclamoMongoAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Consume_DeberiaLanzarFalloAlRegistrarReclamoException_CuandoFalleLaActualizacionDelEstado()
    {
        // Arrange
        var resolucion = new ResolucionReclamo(
            id: Guid.NewGuid(),
            idReclamo: Guid.NewGuid(),
            descripcion: new DescripcionResolucionVO("Problema solucionado"),
            fechaResolucion: new FechaResolucionVO(DateTime.UtcNow)
        );

        var evento = new ResolucionReclamoRegistradaEvent(resolucion, resolucion.IdReclamo);

        var contextMock = new Mock<ConsumeContext<ResolucionReclamoRegistradaEvent>>();
        contextMock.Setup(c => c.Message).Returns(evento);

        _mockReclamoService.Setup(s => s.RegistrarResolucionReclamoMongoAsync(resolucion))
            .ReturnsAsync(HttpStatusCode.Created);

        _mockReclamoService.Setup(s => s.ActualizarEstadoReclamoMongoAsync(resolucion.IdReclamo, "Resuelto"))
            .ThrowsAsync(new Exception("Error al actualizar estado"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() => _consumer.Consume(contextMock.Object));
        Assert.Contains("Ha ocurrido un error al registrar la resolucion del reclamo en MongoDb", ex.Message);

        _mockReclamoService.Verify(s => s.RegistrarResolucionReclamoMongoAsync(resolucion), Times.Once);
        _mockReclamoService.Verify(s => s.ActualizarEstadoReclamoMongoAsync(resolucion.IdReclamo, "Resuelto"), Times.Once);
    }
}
