using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exception;
using Application.Handler;
using Application.Query;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;
using Moq;
using Xunit;

public class ConsultarReclamoHandlerTests
{
    private readonly Mock<IUsuarioService> _mockUsuarioService;
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly Mock<ISubastaService> _mockSubastaService;
    private readonly ConsultarReclamoHandler _handler;

    public ConsultarReclamoHandlerTests()
    {
        _mockUsuarioService = new Mock<IUsuarioService>();
        _mockReclamoService = new Mock<IReclamoService>();
        _mockSubastaService = new Mock<ISubastaService>();
        _handler = new ConsultarReclamoHandler(
            _mockUsuarioService.Object,
            _mockReclamoService.Object,
            _mockSubastaService.Object
        );
    }

    [Fact]
    public async Task Handle_DebeRetornarHistorialReclamosDTO_CuandoReclamoExiste()
    {
        // Arrange
        var idReclamo = Guid.NewGuid();
        var idUsuario = Guid.NewGuid();
        var idSubasta = Guid.NewGuid();

        var reclamo = new Reclamo(
            id: idReclamo,
            idUsuario: idUsuario,
            idSubasta: idSubasta,
            descripcion: new DescripcionReclamoVO("Descripcion"),
            motivo: new MotivoReclamoVO("Motivo"),
            urlEvidencia: new UrlEvidenciaReclamoVO("http://evidencia.com"),
            fechaCreacion: new FechaCreacionReclamoVO(DateTime.UtcNow),
            estadoReclamo: new EstadoReclamoVO("Pendiente")
        );

        var subasta = new Subasta(
            Guid.NewGuid(),
            new NombreSubastaVO("Subasta 1"),
            new DescripcionSubastaVO("Descripcion Subasta"),
            Guid.NewGuid(),
            new FechaInicioSubastaVO(DateTime.UtcNow),
            new FechaFinSubastaVO(DateTime.UtcNow.AddDays(1)),
            new IncrementoMinimoSubastaVO(10),
            new PrecioReservaSubastaVO(100),
            new EstadoSubastaVO("Activa"),
            Guid.NewGuid()
        );

        var correoUsuario = "usuario@test.com";

        _mockReclamoService.Setup(r => r.ConsultarReclamoMongoAsync(idReclamo)).ReturnsAsync(reclamo);
        _mockSubastaService.Setup(s => s.ObtenerSubastaPorGuid(idSubasta)).ReturnsAsync(subasta);
        _mockUsuarioService.Setup(u => u.ObtenerCorreoPorIdAsync(idUsuario)).ReturnsAsync(correoUsuario);

        var query = new ConsultarReclamoQuery(idReclamo);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(reclamo.Id, resultado.id);
        Assert.Equal(reclamo.Descripcion.descripcionReclamo, resultado.descripcion);
        Assert.Equal(reclamo.Motivo.motivoReclamo, resultado.motivo);
        Assert.Equal(reclamo.UrlEvidencia.urlEvidenciaReclamo, resultado.urlEvidencia);
        Assert.Equal(reclamo.EstadoReclamo.estadoReclamo, resultado.estado);
        Assert.Equal(subasta.nombreSubasta.Nombre, resultado.nombreSubasta);
        Assert.Equal(correoUsuario, resultado.correo);
    }


    [Fact]
    public async Task Handle_DeberiaRetornarObjetoVacio_CuandoReclamoNoExiste()
    {
        // Arrange
        var idReclamo = Guid.NewGuid();

        _mockReclamoService.Setup(r => r.ConsultarReclamoMongoAsync(idReclamo)).ReturnsAsync((Reclamo)null);

        var query = new ConsultarReclamoQuery(idReclamo);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.Null(resultado.descripcion);
        Assert.Null(resultado.motivo);
        Assert.Null(resultado.urlEvidencia);
        Assert.Null(resultado.estado);
        Assert.Equal(Guid.Empty, resultado.id);
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlObtenerReclamoException_CuandoOcurreError()
    {
        // Arrange
        var idReclamo = Guid.NewGuid();

        _mockReclamoService.Setup(r => r.ConsultarReclamoMongoAsync(idReclamo))
            .ThrowsAsync(new Exception("Error inesperado"));

        var query = new ConsultarReclamoQuery(idReclamo);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FalloAlObtenerReclamoException>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Contains("Ha ocurrido un error al obtener el reclamo en la bd", ex.Message);
    }
}
