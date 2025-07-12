using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exception;
using Application.Handler;
using Domain.Entities;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using Moq;
using Xunit;

public class RegistrarReclamoPremioHandlerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly Mock<IReclamoService> _reclamoServiceMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly RegistrarReclamoPremioHandler _handler;

    public RegistrarReclamoPremioHandlerTests()
    {
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _reclamoServiceMock = new Mock<IReclamoService>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _handler = new RegistrarReclamoPremioHandler(
            _usuarioServiceMock.Object,
            _reclamoServiceMock.Object,
            _publishEndpointMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsTrue_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new RegistrarReclamoPremioCommand(
            new RegistrarReclamoPremioDTO
            {
                correo = "usuario@ejemplo.com",
                idSubasta = Guid.NewGuid(),
                direccionEnvio = "Calle Falsa 123",
                metodoEntrega = "Courier"
            }
        );

        var usuarioId = Guid.NewGuid();
        var reclamoPremioId = Guid.NewGuid();

        _usuarioServiceMock
            .Setup(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo))
            .ReturnsAsync(usuarioId);

        _reclamoServiceMock
            .Setup(s => s.RegistrarReclamoPremioPostgreSQLAsync(It.IsAny<ReclamoPremio>()))
            .ReturnsAsync(reclamoPremioId);

        _publishEndpointMock
            .Setup(p => p.Publish(It.IsAny<ReclamoPremioRegistradoEvent>(), default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _usuarioServiceMock.Verify(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo), Times.Once);
        _reclamoServiceMock.Verify(s => s.RegistrarReclamoPremioPostgreSQLAsync(It.IsAny<ReclamoPremio>()), Times.Once);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<ReclamoPremioRegistradoEvent>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsUsuarioNoEncontradoException_WhenUserIdIsEmpty()
    {
        // Arrange
        var request = new RegistrarReclamoPremioCommand(
            new RegistrarReclamoPremioDTO { correo = "usuario@ejemplo.com" }
        );

        _usuarioServiceMock
            .Setup(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo))
            .ReturnsAsync(Guid.Empty);

        // Act & Assert
        await Assert.ThrowsAsync<UsuarioNoEncontradoException>(() =>
            _handler.Handle(request, CancellationToken.None));

        _reclamoServiceMock.Verify(s => s.RegistrarReclamoPremioPostgreSQLAsync(It.IsAny<ReclamoPremio>()), Times.Never);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<ReclamoPremioRegistradoEvent>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_ThrowsFalloAlRegistrarReclamoException_WhenRegistrationReturnsEmptyGuid()
    {
        // Arrange
        var request = new RegistrarReclamoPremioCommand(
            new RegistrarReclamoPremioDTO
            {
                correo = "usuario@ejemplo.com",
                idSubasta = Guid.NewGuid(),
                direccionEnvio = "Calle Falsa 123",
                metodoEntrega = "Courier"
            }
        );

        var usuarioId = Guid.NewGuid();

        _usuarioServiceMock
            .Setup(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo))
            .ReturnsAsync(usuarioId);

        _reclamoServiceMock
            .Setup(s => s.RegistrarReclamoPremioPostgreSQLAsync(It.IsAny<ReclamoPremio>()))
            .ReturnsAsync(Guid.Empty); // Simula fallo en el registro

        // Act & Assert
        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(request, CancellationToken.None));

        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<ReclamoPremioRegistradoEvent>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_ThrowsFalloAlRegistrarReclamoException_WhenUnexpectedExceptionIsThrown()
    {
        // Arrange
        var request = new RegistrarReclamoPremioCommand(
            new RegistrarReclamoPremioDTO { correo = "usuario@ejemplo.com" }
        );

        _usuarioServiceMock
            .Setup(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo))
            .ThrowsAsync(new Exception("Error inesperado"));

        // Act & Assert
        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(request, CancellationToken.None));
    }
}
