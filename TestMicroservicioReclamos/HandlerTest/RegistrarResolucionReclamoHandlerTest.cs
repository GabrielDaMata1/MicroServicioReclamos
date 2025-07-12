using System;
using System.Net;
using System.Reflection;
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
using Infrastructure.Models.MongoDB;
using MassTransit;
using Moq;
using Xunit;

public class RegistrarResolucionReclamoHandlerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
    private readonly Mock<IReclamoService> _reclamoServiceMock = new();
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
    private readonly Mock<INotificacionService> _notificacionServiceMock = new();
    private readonly Mock<ISubastaService> _subastaServiceMock = new();

    private readonly RegistrarResolucionReclamoHandler _handler;

    public RegistrarResolucionReclamoHandlerTests()
    {
        _handler = new RegistrarResolucionReclamoHandler(
            _usuarioServiceMock.Object,
            _reclamoServiceMock.Object,
            _publishEndpointMock.Object
        );

        typeof(RegistrarResolucionReclamoHandler)
            .GetField("_notificacionService", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(_handler, _notificacionServiceMock.Object);

        typeof(RegistrarResolucionReclamoHandler)
            .GetField("_subastaService", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(_handler, _subastaServiceMock.Object);
    }

    [Fact]
    public async Task Handle_DeberiaRetornarTrue_CuandoResolucionExitosa()
    {
        var idReclamo = Guid.NewGuid();
        var idSubasta = Guid.NewGuid();
        var idUsuario = Guid.NewGuid();

        var dto = new RegistrarResolucionReclamoDTO
        {
            idReclamo = idReclamo,
            resolucion = "Reclamo aprobado"
        };

        var command = new RegistrarResolucionReclamoCommand(dto);

        _reclamoServiceMock.Setup(x => x.RegistrarResolucionReclamoPostgreSQLAsync(It.IsAny<ResolucionReclamo>()))
            .ReturnsAsync(Guid.NewGuid());

        _reclamoServiceMock.Setup(x => x.ActualizarEstadoReclamoPostgreSQLAsync(idReclamo, "Resuelto"))
            .ReturnsAsync(HttpStatusCode.OK);

        _reclamoServiceMock.Setup(x => x.ConsultarReclamoMongoAsync(idReclamo))
            .ReturnsAsync(new Reclamo { IdUsuario = idUsuario, IdSubasta = idSubasta });

        _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(idUsuario))
            .ReturnsAsync("usuario@correo.com");

        var subasta = new Subasta(
            idSubasta,
            new NombreSubastaVO("Subasta Premium"),
            new DescripcionSubastaVO("Electrónica de alta gama"),
            Guid.NewGuid(),
            new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-2)),
            new FechaFinSubastaVO(DateTime.UtcNow.AddDays(2)),
            new IncrementoMinimoSubastaVO(100),
            new PrecioReservaSubastaVO(5000),
            new EstadoSubastaVO("Activa"),
            idUsuario
        );

        _subastaServiceMock.Setup(x => x.ObtenerSubastaPorGuid(idSubasta))
            .ReturnsAsync(subasta);

        _notificacionServiceMock.Setup(x =>
            x.EnviarCorreoUsuarioResolucionReclamo("usuario@correo.com", "Subasta Premium", "Reclamo aprobado"))
            .ReturnsAsync(true);

        var resultado = await _handler.Handle(command, CancellationToken.None);

        Assert.True(resultado);
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlRegistrarReclamoException_CuandoIdResolucionEsEmpty()
    {
        var dto = new RegistrarResolucionReclamoDTO
        {
            idReclamo = Guid.NewGuid(),
            resolucion = "Resolución inválida"
        };

        var command = new RegistrarResolucionReclamoCommand(dto);

        _reclamoServiceMock.Setup(x => x.RegistrarResolucionReclamoPostgreSQLAsync(It.IsAny<ResolucionReclamo>()))
            .ReturnsAsync(Guid.Empty);

        await Assert.ThrowsAsync<FalloAlRegistrarReclamoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlEnviarCorreoException_CuandoEnvioDeCorreoFalla()
    {
        var idReclamo = Guid.NewGuid();
        var idSubasta = Guid.NewGuid();
        var idUsuario = Guid.NewGuid();

        var dto = new RegistrarResolucionReclamoDTO
        {
            idReclamo = idReclamo,
            resolucion = "Rechazado por falta de evidencia"
        };

        var command = new RegistrarResolucionReclamoCommand(dto);

        _reclamoServiceMock.Setup(x => x.RegistrarResolucionReclamoPostgreSQLAsync(It.IsAny<ResolucionReclamo>()))
            .ReturnsAsync(Guid.NewGuid());

        _reclamoServiceMock.Setup(x => x.ActualizarEstadoReclamoPostgreSQLAsync(idReclamo, "Resuelto"))
            .ReturnsAsync(HttpStatusCode.OK);

        _reclamoServiceMock.Setup(x => x.ConsultarReclamoMongoAsync(idReclamo))
            .ReturnsAsync(new Reclamo { IdUsuario = idUsuario, IdSubasta = idSubasta });

        _usuarioServiceMock.Setup(x => x.ObtenerCorreoPorIdAsync(idUsuario))
            .ReturnsAsync("usuario@correo.com");

        var subasta = new Subasta(
            idSubasta,
            new NombreSubastaVO("Subasta Fallida"),
            new DescripcionSubastaVO("Producto no entregado"),
            Guid.NewGuid(),
            new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-5)),
            new FechaFinSubastaVO(DateTime.UtcNow.AddDays(-1)),
            new IncrementoMinimoSubastaVO(20),
            new PrecioReservaSubastaVO(500),
            new EstadoSubastaVO("Finalizada"),
            idUsuario
        );

        _subastaServiceMock.Setup(x => x.ObtenerSubastaPorGuid(idSubasta))
            .ReturnsAsync(subasta);

        _notificacionServiceMock.Setup(x =>
            x.EnviarCorreoUsuarioResolucionReclamo("usuario@correo.com", "Subasta Fallida", dto.resolucion))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<FalloAlEnviarCorreoException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }


}
