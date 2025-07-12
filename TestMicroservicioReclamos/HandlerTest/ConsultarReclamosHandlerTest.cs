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

public class ConsultarReclamosHandlerTests
{
    private readonly Mock<IUsuarioService> _mockUsuarioService;
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly Mock<ISubastaService> _mockSubastaService;
    private readonly ConsultarReclamosHandler _handler;

    public ConsultarReclamosHandlerTests()
    {
        _mockUsuarioService = new Mock<IUsuarioService>();
        _mockReclamoService = new Mock<IReclamoService>();
        _mockSubastaService = new Mock<ISubastaService>();
        _handler = new ConsultarReclamosHandler(
            _mockUsuarioService.Object,
            _mockReclamoService.Object,
            _mockSubastaService.Object
        );
    }

    [Fact]
    public async Task Handle_DeberiaRetornarListaDeReclamos_CuandoExistanReclamos()
    {
        // Arrange
        var reclamoId = Guid.NewGuid();
        var idUsuario = Guid.NewGuid();
        var idSubasta = Guid.NewGuid();

        var reclamo = new Reclamo(
            reclamoId,
            idUsuario,
            idSubasta,
            new DescripcionReclamoVO("Descripcion"),
            new MotivoReclamoVO("Motivo"),
            new UrlEvidenciaReclamoVO("http://evidencia.com"),
            new FechaCreacionReclamoVO(DateTime.UtcNow),
            new EstadoReclamoVO("Pendiente")
        );

        var subasta = new Subasta(
            idSubasta,
            new NombreSubastaVO("Subasta 1"),
            new DescripcionSubastaVO("Descripcion Subasta"),
            Guid.NewGuid(),
            new FechaInicioSubastaVO(DateTime.UtcNow),
            new FechaFinSubastaVO(DateTime.UtcNow.AddDays(1)),
            new IncrementoMinimoSubastaVO(10),
            new PrecioReservaSubastaVO(200),
            new EstadoSubastaVO("Activa"),
            Guid.NewGuid()
        );

        _mockReclamoService.Setup(r => r.ConsultarReclamosMongoAsync())
            .ReturnsAsync(new List<Reclamo> { reclamo });

        _mockSubastaService.Setup(s => s.ObtenerSubastaPorGuid(idSubasta))
            .ReturnsAsync(subasta);

        _mockUsuarioService.Setup(u => u.ObtenerCorreoPorIdAsync(idUsuario))
            .ReturnsAsync("usuario@test.com");

        var query = new ConsultarReclamosQuery();

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(resultado);
        var item = resultado.First();
        Assert.Equal(reclamoId, item.id);
        Assert.Equal("usuario@test.com", item.correo);
        Assert.Equal("Subasta 1", item.nombreSubasta);
    }


    [Fact]
    public async Task Handle_DeberiaRetornarListaVacia_CuandoNoExistanReclamos()
    {
        // Arrange
        _mockReclamoService.Setup(r => r.ConsultarReclamosMongoAsync())
            .ReturnsAsync(new List<Reclamo>());

        var query = new ConsultarReclamosQuery();

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task Handle_DeberiaLanzarFalloAlObtenerReclamoException_CuandoOcurreError()
    {
        // Arrange
        _mockReclamoService.Setup(r => r.ConsultarReclamosMongoAsync())
            .ThrowsAsync(new Exception("Error inesperado"));

        var query = new ConsultarReclamosQuery();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FalloAlObtenerReclamoException>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Contains("Ha ocurrido un error al obtener el reclamo en la bd", ex.Message);
    }
}
