using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;
using Moq;
using Xunit;

public class ReclamoServiceTests
{
    private readonly Mock<IReclamoMongoRepository> _mockReclamoMongoRepo;
    private readonly Mock<IReclamoPostgreSQLRepository> _mockReclamoPostgresRepo;
    private readonly Mock<IResolucionReclamoMongoRepository> _mockResolucionMongoRepo;
    private readonly Mock<IResolucionReclamoPostgreSQLRepository> _mockResolucionPostgresRepo;
    private readonly Mock<IReclamoPremioMongoRepository> _mockReclamoPremioMongoRepo;
    private readonly Mock<IReclamoPremioPostgreSQLRepository> _mockReclamoPremioPostgresRepo;

    private readonly ReclamoService _service;

    public ReclamoServiceTests()
    {
        _mockReclamoMongoRepo = new Mock<IReclamoMongoRepository>();
        _mockReclamoPostgresRepo = new Mock<IReclamoPostgreSQLRepository>();
        _mockResolucionMongoRepo = new Mock<IResolucionReclamoMongoRepository>();
        _mockResolucionPostgresRepo = new Mock<IResolucionReclamoPostgreSQLRepository>();
        _mockReclamoPremioMongoRepo = new Mock<IReclamoPremioMongoRepository>();
        _mockReclamoPremioPostgresRepo = new Mock<IReclamoPremioPostgreSQLRepository>();

        _service = new ReclamoService(
            _mockReclamoMongoRepo.Object,
            _mockReclamoPostgresRepo.Object,
            _mockResolucionPostgresRepo.Object,
            _mockResolucionMongoRepo.Object,
            _mockReclamoPremioMongoRepo.Object,
            _mockReclamoPremioPostgresRepo.Object
        );
    }

    [Fact]
    public async Task ActualizarEstadoReclamoMongoAsync_DebeRetornarHttpStatusCode()
    {
        var id = Guid.NewGuid();
        var estado = "Resuelto";

        _mockReclamoMongoRepo.Setup(r => r.ActualizarEstadoReclamo(id, estado))
            .ReturnsAsync(HttpStatusCode.OK);

        var result = await _service.ActualizarEstadoReclamoMongoAsync(id, estado);

        Assert.Equal(HttpStatusCode.OK, result);
    }

    [Fact]
    public async Task ActualizarEstadoReclamoPostgreSQLAsync_DebeRetornarHttpStatusCode()
    {
        var id = Guid.NewGuid();
        var estado = "Resuelto";

        _mockReclamoPostgresRepo.Setup(r => r.ActualizarEstadoReclamo(id, estado))
            .ReturnsAsync(HttpStatusCode.OK);

        var result = await _service.ActualizarEstadoReclamoPostgreSQLAsync(id, estado);

        Assert.Equal(HttpStatusCode.OK, result);
    }

    [Fact]
    public async Task ConsultarReclamoMongoAsync_DebeRetornarReclamo()
    {
        var id = Guid.NewGuid();
        var reclamo = new Reclamo { Id = id };

        _mockReclamoMongoRepo.Setup(r => r.ConsultarReclamoMongo(id))
            .ReturnsAsync(reclamo);

        var result = await _service.ConsultarReclamoMongoAsync(id);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task ConsultarReclamoPremioMongoAsync_DebeRetornarReclamoPremio()
    {
        // Arrange
        var id = Guid.NewGuid();

        var direccionEnvio = new DireccionEnvioPremioVO("Calle Falsa 123");
        var metodoEntrega = new MetodoEntregaPremioVO("Delivery");
        var fechaReclamo = new FechaReclamoPremioVO(DateTime.UtcNow);

        var reclamoPremio = new ReclamoPremio(
            id,                 // Id
            Guid.NewGuid(),     // IdUsuario (dummy)
            Guid.NewGuid(),     // IdSubasta (dummy)
            direccionEnvio,
            metodoEntrega,
            fechaReclamo
        );

        _mockReclamoPremioMongoRepo
            .Setup(r => r.ConsultarReclamoPremioMongo(id))
            .ReturnsAsync(reclamoPremio);

        // Act
        var result = await _service.ConsultarReclamoPremioMongoAsync(id);

        // Assert
        Assert.Equal(id, result.Id);
    }




    [Fact]
    public async Task ConsultarReclamosMongoAsync_DebeRetornarListaReclamos()
    {
        _mockReclamoMongoRepo.Setup(r => r.ConsultarReclamosMongo())
            .ReturnsAsync(new List<Reclamo> { new Reclamo() });

        var result = await _service.ConsultarReclamosMongoAsync();

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ConsultarReclamosPorSubastadorMongoAsync_DebeRetornarListaReclamos()
    {
        var idSubastador = Guid.NewGuid();
        _mockReclamoMongoRepo.Setup(r => r.ConsultarReclamosPorSubastadorMongo(idSubastador))
            .ReturnsAsync(new List<Reclamo> { new Reclamo() });

        var result = await _service.ConsultarReclamosPorSubastadorMongoAsync(idSubastador);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ConsultarReclamosPremiosUsuarioMongoAsync_DebeRetornarListaReclamosPremio()
    {
        // Arrange
        var idUsuario = Guid.NewGuid();

        var direccionEnvio = new DireccionEnvioPremioVO("Avenida Principal 456");
        var metodoEntrega = new MetodoEntregaPremioVO("Pickup");
        var fechaReclamo = new FechaReclamoPremioVO(DateTime.UtcNow);

        var reclamoPremio = new ReclamoPremio(
            Guid.NewGuid(),  // Id
            idUsuario,       // This matches the userId we're querying for
            Guid.NewGuid(),  // IdSubasta (dummy)
            direccionEnvio,
            metodoEntrega,
            fechaReclamo
        );

        _mockReclamoPremioMongoRepo
            .Setup(r => r.ConsultarReclamosPremiosUsuarioMongo(idUsuario))
            .ReturnsAsync(new List<ReclamoPremio> { reclamoPremio });

        // Act
        var result = await _service.ConsultarReclamosPremiosUsuarioMongoAsync(idUsuario);

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, r => Assert.Equal(idUsuario, r.IdUsuario));
    }


    [Fact]
    public async Task ConsultarReclamosUsuarioMongoAsync_DebeRetornarListaReclamos()
    {
        var idUsuario = Guid.NewGuid();
        _mockReclamoMongoRepo.Setup(r => r.ConsultarReclamosUsuarioMongo(idUsuario))
            .ReturnsAsync(new List<Reclamo> { new Reclamo() });

        var result = await _service.ConsultarReclamosUsuarioMongoAsync(idUsuario);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ConsultarResolucionReclamoMongoAsync_DebeRetornarResolucion()
    {
        var id = Guid.NewGuid();
        var resolucion = new ResolucionReclamo { Id = id };

        _mockResolucionMongoRepo.Setup(r => r.ConsultarResolucionReclamoMongo(id))
            .ReturnsAsync(resolucion);

        var result = await _service.ConsultarResolucionReclamoMongoAsync(id);

        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task RegistrarReclamoMongoAsync_DebeRetornarHttpStatusCode()
    {
        var reclamo = new Reclamo();

        _mockReclamoMongoRepo.Setup(r => r.RegistrarReclamoMongo(reclamo))
            .ReturnsAsync(HttpStatusCode.Created);

        var result = await _service.RegistrarReclamoMongoAsync(reclamo);

        Assert.Equal(HttpStatusCode.Created, result);
    }

    [Fact]
    public async Task RegistrarReclamoPostgreSQLAsync_DebeRetornarGuid()
    {
        var reclamo = new Reclamo();
        var id = Guid.NewGuid();

        _mockReclamoPostgresRepo.Setup(r => r.RegistrarReclamoAsync(reclamo))
            .ReturnsAsync(id);

        var result = await _service.RegistrarReclamoPostgreSQLAsync(reclamo);

        Assert.Equal(id, result);
    }

    [Fact]
    public async Task RegistrarReclamoPremioMongoAsync_DebeRetornarHttpStatusCode()
    {
        // Arrange
        var direccionEnvio = new DireccionEnvioPremioVO("Calle 987");
        var metodoEntrega = new MetodoEntregaPremioVO("Entrega Personal");
        var fechaReclamo = new FechaReclamoPremioVO(DateTime.UtcNow);

        var reclamoPremio = new ReclamoPremio(
            Guid.NewGuid(),     // Id
            Guid.NewGuid(),     // IdUsuario
            Guid.NewGuid(),     // IdSubasta
            direccionEnvio,
            metodoEntrega,
            fechaReclamo
        );

        _mockReclamoPremioMongoRepo
            .Setup(r => r.RegistrarReclamoPremioAsync(reclamoPremio))
            .ReturnsAsync(HttpStatusCode.Created);

        // Act
        var result = await _service.RegistrarReclamoPremioMongoAsync(reclamoPremio);

        // Assert
        Assert.Equal(HttpStatusCode.Created, result);
    }


    [Fact]
    public async Task RegistrarReclamoPremioPostgreSQLAsync_DebeRetornarGuid()
    {
        // Arrange
        var direccionEnvio = new DireccionEnvioPremioVO("Calle 123");
        var metodoEntrega = new MetodoEntregaPremioVO("Envío Postal");
        var fechaReclamo = new FechaReclamoPremioVO(DateTime.UtcNow);

        var reclamoPremio = new ReclamoPremio(
            Guid.NewGuid(),     // Id
            Guid.NewGuid(),     // IdUsuario
            Guid.NewGuid(),     // IdSubasta
            direccionEnvio,
            metodoEntrega,
            fechaReclamo
        );

        var id = Guid.NewGuid();

        _mockReclamoPremioPostgresRepo
            .Setup(r => r.RegistrarReclamoPremioAsync(reclamoPremio))
            .ReturnsAsync(id);

        // Act
        var result = await _service.RegistrarReclamoPremioPostgreSQLAsync(reclamoPremio);

        // Assert
        Assert.Equal(id, result);
    }


    [Fact]
    public async Task RegistrarResolucionReclamoMongoAsync_DebeRetornarHttpStatusCode()
    {
        var resolucion = new ResolucionReclamo();

        _mockResolucionMongoRepo.Setup(r => r.RegistrarResolucionReclamoMongo(resolucion))
            .ReturnsAsync(HttpStatusCode.Created);

        var result = await _service.RegistrarResolucionReclamoMongoAsync(resolucion);

        Assert.Equal(HttpStatusCode.Created, result);
    }

    [Fact]
    public async Task RegistrarResolucionReclamoPostgreSQLAsync_DebeRetornarGuid()
    {
        var resolucion = new ResolucionReclamo();
        var id = Guid.NewGuid();

        _mockResolucionPostgresRepo.Setup(r => r.RegistrarResolucionReclamo(resolucion))
            .ReturnsAsync(id);

        var result = await _service.RegistrarResolucionReclamoPostgreSQLAsync(resolucion);

        Assert.Equal(id, result);
    }
}
