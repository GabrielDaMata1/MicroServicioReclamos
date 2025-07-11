using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exception;
using Application.Handler;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;
using Moq;
using Xunit;

public class RegistrarResolucionReclamoHandlerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly Mock<IReclamoService> _reclamoServiceMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly RegistrarResolucionReclamoHandler _handler;

    public RegistrarResolucionReclamoHandlerTests()
    {
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _reclamoServiceMock = new Mock<IReclamoService>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _handler = new RegistrarResolucionReclamoHandler(
            _usuarioServiceMock.Object,
            _reclamoServiceMock.Object,
            _publishEndpointMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsTrue_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var resolucionDto = new RegistrarResolucionReclamoDTO
        {
            idReclamo = Guid.NewGuid(),
            resolucion = "Reclamo aprobado"
        };

        var command = new RegistrarResolucionReclamoCommand(resolucionDto);
        var fakeId = Guid.NewGuid();

        _reclamoServiceMock
            .Setup(s => s.RegistrarResolucionReclamoPostgreSQLAsync(It.IsAny<ResolucionReclamo>()))
            .ReturnsAsync(fakeId);

        _reclamoServiceMock
            .Setup(s => s.RegistrarResolucionReclamoPostgreSQLAsync(It.IsAny<ResolucionReclamo>()))
            .ReturnsAsync(fakeId);

        

        _publishEndpointMock
            .Setup(p => p.Publish(It.IsAny<ResolucionReclamoRegistradaEvent>(), default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _reclamoServiceMock.Verify(s => s.RegistrarResolucionReclamoPostgreSQLAsync(It.IsAny<ResolucionReclamo>()), Times.Once);
        _reclamoServiceMock.Verify(s => s.ActualizarEstadoReclamoPostgreSQLAsync(resolucionDto.idReclamo, "Resuelto"), Times.Once);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<ResolucionReclamoRegistradaEvent>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsFalloAlRegistrarReclamoException_WhenIdResolucionIsEmpty()
    {
        // Arrange
        var resolucionDto = new RegistrarResolucionReclamoDTO
        {
            idReclamo = Guid.NewGuid(),
            resolucion = "Rechazado"
        };

        var command = new RegistrarResolucionReclamoCommand(resolucionDto);

        _reclamoServiceMock
            .Setup(s => s.RegistrarResolucionReclamoPostgreSQLAsync(It.IsAny<ResolucionReclamo>()))
            .ReturnsAsync(Guid.Empty);

        // Act & Assert
        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ThrowsFalloAlRegistrarReclamoException_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var resolucionDto = new RegistrarResolucionReclamoDTO
        {
            idReclamo = Guid.NewGuid(),
            resolucion = "Pendiente"
        };

        var command = new RegistrarResolucionReclamoCommand(resolucionDto);

        _reclamoServiceMock
            .Setup(s => s.RegistrarResolucionReclamoPostgreSQLAsync(It.IsAny<ResolucionReclamo>()))
            .ThrowsAsync(new Exception("Error inesperado"));

        // Act & Assert
        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
