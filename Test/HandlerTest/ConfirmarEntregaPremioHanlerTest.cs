using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Command;
using Application.Exception;
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
    private readonly Mock<IReclamoService> _mockReclamoService;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly ConfirmarEntregaPremioHandler _handler;

    public ConfirmarEntregaPremioHandlerTests()
    {
        _mockReclamoService = new Mock<IReclamoService>();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _handler = new ConfirmarEntregaPremioHandler(_mockReclamoService.Object, _mockPublishEndpoint.Object);
    }

    [Fact]
    public async Task Handle_DebeConfirmarEntregaPremio_CuandoReclamoExiste()
    {
        // Arrange
        var idReclamoPremio = Guid.NewGuid();

        var direccionEnvio = new DireccionEnvioPremioVO("Av. Central 123");
        var metodoEntrega = new MetodoEntregaPremioVO("Courier");
        var fechaReclamo = new FechaReclamoPremioVO(DateTime.UtcNow);

        var reclamoPremio = new ReclamoPremio(
            id: idReclamoPremio,
            idUsuario: Guid.NewGuid(),
            idSubasta: Guid.NewGuid(),
            direccionEnvio: direccionEnvio,
            metodoEntrega: metodoEntrega,
            fechaReclamo: fechaReclamo
        );

        _mockReclamoService
            .Setup(s => s.ConsultarReclamoPremioMongoAsync(idReclamoPremio))
            .ReturnsAsync(reclamoPremio);

        _mockPublishEndpoint
            .Setup(p => p.Publish(It.IsAny<EntregaPremioConfirmadaEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new ConfirmarEntregaPremioCommand(idReclamoPremio);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(resultado);
        _mockReclamoService.Verify(s => s.ConsultarReclamoPremioMongoAsync(idReclamoPremio), Times.Once);

        _mockPublishEndpoint.Verify(p => p.Publish(
            It.Is<EntregaPremioConfirmadaEvent>(e => e.idSubasta == reclamoPremio.IdSubasta),
            It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Handle_DebeLanzarFalloAlObtenerReclamoException_CuandoReclamoNoExiste()
    {
        // Arrange
        var idReclamoPremio = Guid.NewGuid();

        _mockReclamoService
            .Setup(s => s.ConsultarReclamoPremioMongoAsync(idReclamoPremio))
            .ReturnsAsync((ReclamoPremio)null);

        var command = new ConfirmarEntregaPremioCommand(idReclamoPremio);

        // Act & Assert
        await Assert.ThrowsAsync<FalloAlObtenerReclamoException>(() => _handler.Handle(command, CancellationToken.None));

        _mockReclamoService.Verify(s => s.ConsultarReclamoPremioMongoAsync(idReclamoPremio), Times.Once);
        _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<EntregaPremioConfirmadaEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DebeLanzarFalloAlObtenerReclamoException_CuandoOcurreErrorInesperado()
    {
        // Arrange
        var idReclamoPremio = Guid.NewGuid();

        _mockReclamoService
            .Setup(s => s.ConsultarReclamoPremioMongoAsync(idReclamoPremio))
            .ThrowsAsync(new Exception("Error inesperado"));

        var command = new ConfirmarEntregaPremioCommand(idReclamoPremio);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FalloAlObtenerReclamoException>(() => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("Ha ocurrido un error al obtener el reclamo de premio del usuario en la bd", ex.Message);
        _mockReclamoService.Verify(s => s.ConsultarReclamoPremioMongoAsync(idReclamoPremio), Times.Once);
        _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<EntregaPremioConfirmadaEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
