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
using Domain.Value_Object;
using Moq;
using Xunit;

public class ConsultarReclamosResolucionHandlerTests
{
    private readonly Mock<IUsuarioService> _mockUsuarioService;
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly Mock<ISubastaService> _mockSubastaService;
    private readonly ConsultarReclamosResolucionHandler _handler;

    public ConsultarReclamosResolucionHandlerTests()
    {
        _mockUsuarioService = new Mock<IUsuarioService>();
        _mockReclamoService = new Mock<IReclamoService>();
        _mockSubastaService = new Mock<ISubastaService>();

        _handler = new ConsultarReclamosResolucionHandler(
            _mockUsuarioService.Object,
            _mockReclamoService.Object,
            _mockSubastaService.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnList_WhenReclamosExist()
    {
        // Arrange
        var correo = "user@example.com";
        var idUsuario = Guid.NewGuid();
        var idSubasta = Guid.NewGuid();
        var idReclamo = Guid.NewGuid();

        // Reclamo constructor with Id (see your domain code)
        var reclamo = new Reclamo(
            idReclamo,
            idUsuario,
            idSubasta,
            new DescripcionReclamoVO("desc"),
            new MotivoReclamoVO("motivo"),
            new UrlEvidenciaReclamoVO("url"),
            new FechaCreacionReclamoVO(DateTime.UtcNow),
            new EstadoReclamoVO("pendiente")
        );

        // ResolucionReclamo constructor with Id (see your domain code)
        var resolucion = new ResolucionReclamo(
            idReclamo, // For Id? Actually you probably want a new Id here:
            idReclamo,
            new DescripcionResolucionVO("resuelto"),
            new FechaResolucionVO(DateTime.UtcNow)
        );
        // If Id is different, do something like:
        resolucion.Id = Guid.NewGuid();

        // Subasta full constructor (all properties required)
        var subasta = new Subasta(
            idSubasta,
            new NombreSubastaVO("Subasta1"),
            new DescripcionSubastaVO("Descripcion Subasta"),
            Guid.NewGuid(), // idUsuarioSubastador or similar
            new FechaInicioSubastaVO(DateTime.UtcNow.AddDays(-1)),
            new FechaFinSubastaVO(DateTime.UtcNow.AddDays(1)),
            new IncrementoMinimoSubastaVO(10),
            new PrecioReservaSubastaVO(100),
            new EstadoSubastaVO("Activa"),
            Guid.NewGuid()  // some other guid property, e.g., categoriaId
        );

        // Setup mocks
        _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(idUsuario);
        _mockReclamoService.Setup(s => s.ConsultarReclamosUsuarioMongoAsync(idUsuario)).ReturnsAsync(new List<Reclamo> { reclamo });
        _mockSubastaService.Setup(s => s.ObtenerSubastaPorGuid(idSubasta)).ReturnsAsync(subasta);
        _mockReclamoService.Setup(s => s.ConsultarResolucionReclamoMongoAsync(idReclamo)).ReturnsAsync(resolucion);

        // Act
        var result = await _handler.Handle(new ConsultarReclamosResolucionQuery(correo), CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal(idReclamo, result[0].idReclamo);
        Assert.Equal("resuelto", result[0].descripcionResolucion);
    }


    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoReclamosExist()
    {
        var correo = "user@example.com";
        var idUsuario = Guid.NewGuid();

        _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(idUsuario);
        _mockReclamoService.Setup(s => s.ConsultarReclamosUsuarioMongoAsync(idUsuario)).ReturnsAsync(new List<Reclamo>());

        var result = await _handler.Handle(new ConsultarReclamosResolucionQuery(correo), CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowUsuarioNoEncontradoException_WhenUserNotFound()
    {
        var correo = "user@example.com";

        _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(Guid.Empty);

        await Assert.ThrowsAsync<UsuarioNoEncontradoException>(() => _handler.Handle(new ConsultarReclamosResolucionQuery(correo), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowFalloAlObtenerReclamoException_WhenUnexpectedErrorOccurs()
    {
        var correo = "user@example.com";

        _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(correo)).ThrowsAsync(new Exception("error"));

        await Assert.ThrowsAsync<FalloAlObtenerReclamoException>(() => _handler.Handle(new ConsultarReclamosResolucionQuery(correo), CancellationToken.None));
    }
}
