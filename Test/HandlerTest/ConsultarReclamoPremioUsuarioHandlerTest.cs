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

public class ConsultarReclamoPremioUsuarioHandlerTests
{
    private readonly Mock<IUsuarioService> _mockUsuarioService;
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly Mock<ISubastaService> _mockSubastaService;
    private readonly ConsultarReclamoPremioUsuarioHandler _handler;

    public ConsultarReclamoPremioUsuarioHandlerTests()
    {
        _mockUsuarioService = new Mock<IUsuarioService>();
        _mockReclamoService = new Mock<IReclamoService>();
        _mockSubastaService = new Mock<ISubastaService>();
        _handler = new ConsultarReclamoPremioUsuarioHandler(
            _mockUsuarioService.Object,
            _mockReclamoService.Object,
            _mockSubastaService.Object
        );
    }

    [Fact]
    public async Task Handle_DeberiaRetornarListaDeReclamos_CuandoExistanReclamos()
    {
        // Arrange
        var correo = "usuario@test.com";
        var idUsuario = Guid.NewGuid();
        var idSubasta = Guid.NewGuid();
        var idReclamo = Guid.NewGuid();

        _mockUsuarioService.Setup(u => u.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(idUsuario);

        var reclamoPremio = new ReclamoPremio(
            id: idReclamo,
            idUsuario: idUsuario,
            idSubasta: idSubasta,
            direccionEnvio: new DireccionEnvioPremioVO("Calle 123"),
            metodoEntrega: new MetodoEntregaPremioVO("Envio"),
            fechaReclamo: new FechaReclamoPremioVO(DateTime.UtcNow)
        );

        _mockReclamoService.Setup(r => r.ConsultarReclamosPremiosUsuarioMongoAsync(idUsuario))
            .ReturnsAsync(new List<ReclamoPremio> { reclamoPremio });

        var subasta = new Subasta(
            idSubasta,
            new NombreSubastaVO("Subasta 1"),
            new DescripcionSubastaVO("Descripcion Subasta"),
            Guid.NewGuid(),
            new FechaInicioSubastaVO(DateTime.UtcNow),
            new FechaFinSubastaVO(DateTime.UtcNow.AddDays(1)),
            new IncrementoMinimoSubastaVO(5),
            new PrecioReservaSubastaVO(100),
            new EstadoSubastaVO("Activa"),
            Guid.NewGuid()
        );

        _mockSubastaService.Setup(s => s.ObtenerSubastaPorGuid(idSubasta))
            .ReturnsAsync(subasta);

        var query = new ConsultarReclamosPremiosUsuarioQuery(correo);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(resultado);
        var reclamo = resultado.First();
        Assert.Equal(idReclamo, reclamo.idReclamoPremio);
        Assert.Equal(subasta.nombreSubasta.Nombre, reclamo.nombreSubasta);
        Assert.Equal("Calle 123", reclamo.direccionEnvio);
    }


    [Fact]
    public async Task Handle_DeberiaRetornarListaVacia_CuandoNoExistanReclamos()
    {
        // Arrange
        var correo = "usuario@test.com";
        var idUsuario = Guid.NewGuid();

        _mockUsuarioService.Setup(u => u.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(idUsuario);
        _mockReclamoService.Setup(r => r.ConsultarReclamosPremiosUsuarioMongoAsync(idUsuario))
            .ReturnsAsync(new List<ReclamoPremio>());

        var query = new ConsultarReclamosPremiosUsuarioQuery(correo);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task Handle_DeberiaLanzarUsuarioNoEncontradoException_CuandoIdUsuarioSeaVacio()
    {
        // Arrange
        var correo = "usuario@test.com";

        _mockUsuarioService.Setup(u => u.ObtenerUsuarioPorIdAsync(correo)).ReturnsAsync(Guid.Empty);

        var query = new ConsultarReclamosPremiosUsuarioQuery(correo);

        // Act & Assert
        await Assert.ThrowsAsync<UsuarioNoEncontradoException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlObtenerReclamoException_CuandoOcurreError()
    {
        // Arrange
        var correo = "usuario@test.com";

        _mockUsuarioService.Setup(u => u.ObtenerUsuarioPorIdAsync(correo))
            .ThrowsAsync(new Exception("Error inesperado"));

        var query = new ConsultarReclamosPremiosUsuarioQuery(correo);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FalloAlObtenerReclamoException>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Contains("Ha ocurrido un error al obtener los reclamos de premios del usuario en la bd", ex.Message);
    }
}
