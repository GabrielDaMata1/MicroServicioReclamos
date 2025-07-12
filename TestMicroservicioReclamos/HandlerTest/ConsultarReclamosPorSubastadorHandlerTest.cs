using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exception;
using Application.Handler;
using Application.Query;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

public class ConsultarReclamosPorSubastadorHandlerTests
{
    private readonly Mock<IUsuarioService> _mockUsuarioService;
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly Mock<ISubastaService> _mockSubastaService;
    private readonly ConsultarReclamosPorSubastadorHandler _handler;

    public ConsultarReclamosPorSubastadorHandlerTests()
    {
        _mockUsuarioService = new Mock<IUsuarioService>();
        _mockReclamoService = new Mock<IReclamoService>();
        _mockSubastaService = new Mock<ISubastaService>();

        _handler = new ConsultarReclamosPorSubastadorHandler(
            _mockUsuarioService.Object,
            _mockReclamoService.Object,
            _mockSubastaService.Object
        );
    }

    [Fact]
    public async Task Handle_DeberiaRetornarListaReclamos_CuandoTodoSaleBien()
    {
        var correo = "subastador@test.com";
        var idUsuario = Guid.NewGuid();

        var reclamo = ReclamoFactory.CrearReclamo();
        var subasta = SubastaFactory.CrearSubasta();

        _mockUsuarioService.Setup(u => u.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(idUsuario);
        _mockReclamoService.Setup(r => r.ConsultarReclamosPorSubastadorMongoAsync(idUsuario)).ReturnsAsync(new List<Reclamo> { reclamo });
        _mockSubastaService.Setup(s => s.ObtenerSubastaPorGuid(reclamo.IdSubasta)).ReturnsAsync(subasta);
        _mockUsuarioService.Setup(u => u.ObtenerCorreoPorIdAsync(reclamo.IdUsuario)).ReturnsAsync("usuario@test.com");

        var query = new ConsultarReclamosPorSubastadorQuery(correo);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(reclamo.Id, result[0].id);
        Assert.Equal(subasta.nombreSubasta.Nombre, result[0].nombreSubasta);
    }

    [Fact]
    public async Task Handle_DeberiaRetornarListaVacia_CuandoNoHayReclamos()
    {
        var correo = "subastador@test.com";
        var idUsuario = Guid.NewGuid();

        _mockUsuarioService.Setup(u => u.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(idUsuario);
        _mockReclamoService.Setup(r => r.ConsultarReclamosPorSubastadorMongoAsync(idUsuario)).ReturnsAsync(new List<Reclamo>());

        var query = new ConsultarReclamosPorSubastadorQuery(correo);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_DeberiaLanzarUsuarioNoEncontradoException_CuandoNoEncuentraUsuario()
    {
        var correo = "subastador@test.com";
        _mockUsuarioService.Setup(u => u.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(Guid.Empty);

        var query = new ConsultarReclamosPorSubastadorQuery(correo);

        await Assert.ThrowsAsync<UsuarioNoEncontradoException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlObtenerReclamoException_CuandoFalleLaConsultaDeReclamos()
    {
        var correo = "subastador@test.com";
        var idUsuario = Guid.NewGuid();

        _mockUsuarioService.Setup(u => u.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(idUsuario);
        _mockReclamoService.Setup(r => r.ConsultarReclamosPorSubastadorMongoAsync(idUsuario)).ThrowsAsync(new Exception("Error inesperado"));

        var query = new ConsultarReclamosPorSubastadorQuery(correo);

        await Assert.ThrowsAsync<FalloAlObtenerReclamoException>(() => _handler.Handle(query, CancellationToken.None));
    }
}

public static class ReclamoFactory
{
    public static Reclamo CrearReclamo()
    {
        return new Reclamo(Guid.NewGuid(), Guid.NewGuid(),
            new Domain.Value_Object.DescripcionReclamoVO("desc"),
            new Domain.Value_Object.MotivoReclamoVO("motivo"),
            new Domain.Value_Object.UrlEvidenciaReclamoVO("url"),
            new Domain.Value_Object.FechaCreacionReclamoVO(DateTime.UtcNow),
            new Domain.Value_Object.EstadoReclamoVO("estado")
        );
    }
}

public static class SubastaFactory
{
    public static Subasta CrearSubasta()
    {
        return new Subasta(
            Guid.NewGuid(),
            new Domain.Value_Object.NombreSubastaVO("subasta"),
            new Domain.Value_Object.DescripcionSubastaVO("descripcion"),
            Guid.NewGuid(),
            new Domain.Value_Object.FechaInicioSubastaVO(DateTime.UtcNow),
            new Domain.Value_Object.FechaFinSubastaVO(DateTime.UtcNow.AddDays(1)),
            new Domain.Value_Object.IncrementoMinimoSubastaVO(10),
            new Domain.Value_Object.PrecioReservaSubastaVO(100),
            new Domain.Value_Object.EstadoSubastaVO("activa"),
            Guid.NewGuid()
        );
    }
}
