using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exception;
using Application.Exceptions;
using Application.Handler;
using Domain.Entities;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using Domain.Value_Object;
using MassTransit;
using Moq;
using Xunit;

public class RegistrarReclamoPremioHandlerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
    private readonly Mock<IReclamoService> _reclamoServiceMock = new();
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
    private readonly Mock<INotificacionService> _notificacionServiceMock = new();
    private readonly Mock<ISubastaService> _subastaServiceMock = new();

    private readonly RegistrarReclamoPremioHandler _handler;

    public RegistrarReclamoPremioHandlerTests()
    {
        _handler = new RegistrarReclamoPremioHandler(
            _usuarioServiceMock.Object,
            _reclamoServiceMock.Object,
            _publishEndpointMock.Object,
            _notificacionServiceMock.Object,
            _subastaServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_DeberiaRetornarTrue_CuandoReclamoRegistradoCorrectamente()
    {
        var idSubasta = Guid.NewGuid();
        var correoUsuario = "ganador@ejemplo.com";
        var idUsuario = Guid.NewGuid();

        var command = new RegistrarReclamoPremioCommand(new RegistrarReclamoPremioDTO
        {
            idSubasta = idSubasta,
            correo = correoUsuario,
            direccionEnvio = "Calle Tecnológica 321",
            metodoEntrega = "Envio express"
        });

        _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correoUsuario)).ReturnsAsync(idUsuario);
        _reclamoServiceMock.Setup(x => x.RegistrarReclamoPremioPostgreSQLAsync(It.IsAny<ReclamoPremio>()))
            .ReturnsAsync(Guid.NewGuid());

        _subastaServiceMock.Setup(x => x.ObtenerSubastaPorGuid(idSubasta))
            .ReturnsAsync(new Subasta(
                idSubasta,
                new NombreSubastaVO("Subasta Legendaria"),
                new DescripcionSubastaVO("Edición coleccionista"),
                Guid.NewGuid(),
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-3)),
                new FechaFinSubastaVO(DateTime.UtcNow.AddDays(3)),
                new IncrementoMinimoSubastaVO(10),
                new PrecioReservaSubastaVO(1000),
                new EstadoSubastaVO("Activa"),
                Guid.NewGuid()
            ));

        _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync("subastador@correo.com");

        _notificacionServiceMock.Setup(x =>
            x.EnviarCorreoSubastadorReclamoPremio("subastador@correo.com", "Subasta Legendaria", correoUsuario))
            .ReturnsAsync(true);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        Assert.True(resultado);
    }

    [Fact]
    public async Task Handle_DeberiaLanzarUsuarioNoEncontradoException_CuandoIdUsuarioEsEmpty()
    {
        var command = new RegistrarReclamoPremioCommand(new RegistrarReclamoPremioDTO
        {
            idSubasta = Guid.NewGuid(),
            correo = "desconocido@correo.com",
            direccionEnvio = "Dirección inexistente",
            metodoEntrega = "Teletransportación"
        });

        _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(command.reclamoDto.correo))
            .ReturnsAsync(Guid.Empty);

        await Assert.ThrowsAsync<UsuarioNoEncontradoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlRegistrarReclamoException_CuandoRegistroFalla()
    {
        var command = new RegistrarReclamoPremioCommand(new RegistrarReclamoPremioDTO
        {
            idSubasta = Guid.NewGuid(),
            correo = "usuario@correo.com",
            direccionEnvio = "Zona Industrial",
            metodoEntrega = "Recoger en tienda"
        });

        _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(command.reclamoDto.correo))
            .ReturnsAsync(Guid.NewGuid());

        _reclamoServiceMock.Setup(x => x.RegistrarReclamoPremioPostgreSQLAsync(It.IsAny<ReclamoPremio>()))
            .ReturnsAsync(Guid.Empty);

        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlEnviarCorreoException_CuandoEnvioCorreoFalla()
    {
        var idSubasta = Guid.NewGuid();
        var correo = "usuario@correo.com";
        var idUsuario = Guid.NewGuid();
        var subastadorId = Guid.NewGuid();

        var command = new RegistrarReclamoPremioCommand(new RegistrarReclamoPremioDTO
        {
            idSubasta = idSubasta,
            correo = correo,
            direccionEnvio = "Av. Principal #99",
            metodoEntrega = "Drone"
        });

        _usuarioServiceMock.Setup(x => x.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(idUsuario);
        _reclamoServiceMock.Setup(x => x.RegistrarReclamoPremioPostgreSQLAsync(It.IsAny<ReclamoPremio>()))
            .ReturnsAsync(Guid.NewGuid());

        _subastaServiceMock.Setup(x => x.ObtenerSubastaPorGuid(idSubasta))
            .ReturnsAsync(new Subasta(
                idSubasta,
                new NombreSubastaVO("Subasta Fallida"),
                new DescripcionSubastaVO("No entregado"),
                Guid.NewGuid(),
                new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-5)),
                new FechaFinSubastaVO(DateTime.UtcNow),
                new IncrementoMinimoSubastaVO(25),
                new PrecioReservaSubastaVO(200),
                new EstadoSubastaVO("Finalizada"),
                subastadorId
            ));

        _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(subastadorId))
            .ReturnsAsync("subastador@fallido.com");

        _notificacionServiceMock.Setup(x =>
            x.EnviarCorreoSubastadorReclamoPremio("subastador@fallido.com", "Subasta Fallida", correo))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<FalloAlEnviarCorreoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }


}
