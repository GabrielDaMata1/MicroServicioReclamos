using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Command;
using Application.Exception;
using Application.Exceptions;
using Application.Handler;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Domain.Value_Object;
using MassTransit;
using Moq;
using Xunit;

public class ConfirmarEntregaPremioHandlerTests
{
    private readonly Mock<IReclamoService> _reclamoServiceMock = new();
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
    private readonly Mock<INotificacionService> _notificacionServiceMock = new();
    private readonly Mock<ISubastaService> _subastaServiceMock = new();
    private readonly Mock<IUsuarioService> _usuarioServiceMock = new();

    private readonly ConfirmarEntregaPremioHandler _handler;

    public ConfirmarEntregaPremioHandlerTests()
    {
        _handler = new ConfirmarEntregaPremioHandler(
            _reclamoServiceMock.Object,
            _publishEndpointMock.Object,
            _notificacionServiceMock.Object,
            _subastaServiceMock.Object,
            _usuarioServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_DeberiaRetornarTrue_CuandoConfirmacionExitosa()
    {
        var idReclamo = Guid.NewGuid();
        var idUsuario = Guid.NewGuid();
        var idSubasta = Guid.NewGuid();
        var idSubastador = Guid.NewGuid();

        var command = new ConfirmarEntregaPremioCommand(idReclamo);

        var reclamoPremio = new ReclamoPremio(
            idReclamo,
            idUsuario,
            idSubasta,
            new DireccionEnvioPremioVO("Av. Bolívar 10"),
            new MetodoEntregaPremioVO("Delivery"),
            new FechaReclamoPremioVO(DateTime.UtcNow.AddDays(-1))
        );

        var subasta = new Subasta(
            idSubasta,
            new NombreSubastaVO("Subasta Exclusiva"),
            new DescripcionSubastaVO("Edición limitada"),
            Guid.NewGuid(),
            new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-3)),
            new FechaFinSubastaVO(DateTime.UtcNow.AddDays(2)),
            new IncrementoMinimoSubastaVO(100),
            new PrecioReservaSubastaVO(1000),
            new EstadoSubastaVO("Finalizada"),
            idSubastador
        );

        _reclamoServiceMock.Setup(x => x.ConsultarReclamoPremioMongoAsync(idReclamo))
            .ReturnsAsync(reclamoPremio);

        _subastaServiceMock.Setup(x => x.ObtenerSubastaPorGuid(idSubasta))
            .ReturnsAsync(subasta);

        _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(idUsuario))
            .ReturnsAsync("usuario@correo.com");

        _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(idSubastador))
            .ReturnsAsync("subastador@correo.com");

        _notificacionServiceMock.Setup(x =>
            x.EnviarCorreoConfirmacionSubastadorReclamoPremio("subastador@correo.com", "Subasta Exclusiva", "usuario@correo.com"))
            .ReturnsAsync(true);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        Assert.True(resultado);
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlObtenerReclamoException_CuandoReclamoNoExiste()
    {
        var command = new ConfirmarEntregaPremioCommand(Guid.NewGuid());

        _reclamoServiceMock.Setup(x =>
            x.ConsultarReclamoPremioMongoAsync(command.idReclamoPremio))
            .ReturnsAsync((ReclamoPremio)null);

        await Assert.ThrowsAsync<FalloAlObtenerReclamoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlEnviarCorreoException_CuandoEnvioCorreoFalla()
    {
        var idReclamo = Guid.NewGuid();
        var idUsuario = Guid.NewGuid();
        var idSubasta = Guid.NewGuid();
        var idSubastador = Guid.NewGuid();

        var command = new ConfirmarEntregaPremioCommand(idReclamo);

        var reclamoPremio = new ReclamoPremio(
            idReclamo,
            idUsuario,
            idSubasta,
            new DireccionEnvioPremioVO("Calle Falsa 123"),
            new MetodoEntregaPremioVO("Pickup"),
            new FechaReclamoPremioVO(DateTime.UtcNow)
        );

        var subasta = new Subasta(
            idSubasta,
            new NombreSubastaVO("Subasta Fallida"),
            new DescripcionSubastaVO("Sin entrega"),
            Guid.NewGuid(),
            new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-5)),
            new FechaFinSubastaVO(DateTime.UtcNow),
            new IncrementoMinimoSubastaVO(50),
            new PrecioReservaSubastaVO(500),
            new EstadoSubastaVO("Finalizada"),
            idSubastador
        );

        _reclamoServiceMock.Setup(x => x.ConsultarReclamoPremioMongoAsync(idReclamo))
            .ReturnsAsync(reclamoPremio);

        _subastaServiceMock.Setup(x => x.ObtenerSubastaPorGuid(idSubasta))
            .ReturnsAsync(subasta);

        _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(idUsuario))
            .ReturnsAsync("user@correo.com");

        _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(idSubastador))
            .ReturnsAsync("fallo@correo.com");

        _notificacionServiceMock.Setup(x =>
            x.EnviarCorreoConfirmacionSubastadorReclamoPremio("fallo@correo.com", "Subasta Fallida", "user@correo.com"))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<FalloAlEnviarCorreoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }


}
