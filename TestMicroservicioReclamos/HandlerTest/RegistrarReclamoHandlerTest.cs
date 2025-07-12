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
using Domain.Value_Object;
using MassTransit;
using Moq;
using Xunit;

public class RegistrarReclamoHandlerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly Mock<IReclamoService> _reclamoServiceMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly RegistrarReclamoHandler _handler;

    public RegistrarReclamoHandlerTests()
    {
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _reclamoServiceMock = new Mock<IReclamoService>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _handler = new RegistrarReclamoHandler(
            _usuarioServiceMock.Object,
            _reclamoServiceMock.Object,
            _publishEndpointMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsTrue_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new RegistrarReclamoCommand(
            new RegistrarReclamoDTO
            {
                correo = "user@example.com",
                idSubasta = Guid.NewGuid(),
                descripcion = "Descripcion prueba",
                motivo = "Motivo prueba",
                urlEvidencia = "http://evidencia.url"
            }
        );



        var usuarioId = Guid.NewGuid();
        var reclamoId = Guid.NewGuid();

        // Expected Reclamo instance created in your handler
        var expectedReclamo = new Reclamo(
            usuarioId,
            request.reclamoDto.idSubasta,
            new DescripcionReclamoVO(request.reclamoDto.descripcion),
            new MotivoReclamoVO(request.reclamoDto.motivo),
            new UrlEvidenciaReclamoVO(request.reclamoDto.urlEvidencia),
            new FechaCreacionReclamoVO(It.IsAny<DateTime>()), // If your handler sets this to UtcNow, you may have to adjust this
            new EstadoReclamoVO("Pendiente")
        );

        _usuarioServiceMock
            .Setup(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo))
            .ReturnsAsync(usuarioId);

        _reclamoServiceMock
            .Setup(s => s.RegistrarReclamoPostgreSQLAsync(It.IsAny<Reclamo>()))
            .ReturnsAsync(reclamoId);

        _publishEndpointMock
            .Setup(p => p.Publish(It.IsAny<ReclamoRegistradoEvent>(), default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _usuarioServiceMock.Verify(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo), Times.Once);
        _reclamoServiceMock.Verify(s => s.RegistrarReclamoPostgreSQLAsync(It.IsAny<Reclamo>()), Times.Once);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<ReclamoRegistradoEvent>(), default), Times.Once);
    }


    [Fact]
    public async Task Handle_ThrowsFalloAlRegistrarReclamoException_RegistrationReturnsEmptyGuid()
    {
        // Arrange
        var request = new RegistrarReclamoCommand(
            new RegistrarReclamoDTO
            {
                correo = "user@example.com",
                idSubasta = Guid.NewGuid(),
                descripcion = "Descripcion prueba",
                motivo = "Motivo prueba",
                urlEvidencia = "http://evidencia.url"
            }
        );

        var usuarioId = Guid.NewGuid();

        _usuarioServiceMock
            .Setup(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo))
            .ReturnsAsync(usuarioId);

        _reclamoServiceMock
            .Setup(s => s.RegistrarReclamoPostgreSQLAsync(It.IsAny<Reclamo>()))
            .ReturnsAsync(Guid.Empty); // Simula fallo en el registro

        // Act & Assert
        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(request, CancellationToken.None));

        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<ReclamoRegistradoEvent>(), default), Times.Never);
    }


    [Fact]
    public async Task Handle_ThrowsFalloAlRegistrarReclamoException_WhenRegistrationReturnsEmptyGuid()
    {
        // Arrange
        var request = new RegistrarReclamoCommand(
            new RegistrarReclamoDTO
            {
                correo = "user@example.com",
                idSubasta = Guid.NewGuid(),
                descripcion = "Descripcion prueba",
                motivo = "Motivo prueba",
                urlEvidencia = "http://evidencia.url"
            }
        );

        var usuarioId = Guid.NewGuid();

        _usuarioServiceMock
            .Setup(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo))
            .ReturnsAsync(usuarioId);

        _reclamoServiceMock
            .Setup(s => s.RegistrarReclamoPostgreSQLAsync(It.IsAny<Reclamo>()))
            .ReturnsAsync(Guid.Empty); // Simula fallo en el registro

        // Act & Assert
        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(request, CancellationToken.None));

        // Verifica que no se publicó el evento
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<ReclamoRegistradoEvent>(), default), Times.Never);
    }


    [Fact]
    public async Task Handle_ThrowsFalloAlRegistrarReclamoException_WhenUnexpectedExceptionIsThrown()
    {
        // Arrange
        var request = new RegistrarReclamoCommand(
            new RegistrarReclamoDTO
            {
                correo = "user@example.com",
                idSubasta = Guid.NewGuid(),
                descripcion = "Test",
                motivo = "Test",
                urlEvidencia = "http://test.url"
            }
        );

        _usuarioServiceMock
            .Setup(s => s.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo))
            .ThrowsAsync(new Exception("Error inesperado"));

        // Act & Assert
        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(request, CancellationToken.None));
    }

}
