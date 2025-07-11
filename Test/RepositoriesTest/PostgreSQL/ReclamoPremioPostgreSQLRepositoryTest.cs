using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;
using Infrastructure.Persistance;
using Infrastructure.Repositories.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ReclamoPremioPostgreSQLRepositoryTests
{
    [Fact]
    public async Task RegistrarReclamoPremioAsync_ShouldAddEntityAndSave_ReturnsId()
    {
        // Arrange: Configuración del contexto InMemory
        var options = new DbContextOptionsBuilder<SubastaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Base única por prueba
            .Options;

        using var context = new SubastaDbContext(options);
        var repository = new ReclamoPremioPostgreSQLRepository(context);

        var reclamoPremioDomain = new ReclamoPremio(
            Guid.NewGuid(),                          // idUsuario
            Guid.NewGuid(),                          // idSubasta
            new DireccionEnvioPremioVO("Dirección test"),
            new MetodoEntregaPremioVO("Método test"),
            new FechaReclamoPremioVO(DateTime.UtcNow)
        );

        // Act: Ejecutar el método que se está probando
        var resultId = await repository.RegistrarReclamoPremioAsync(reclamoPremioDomain);

        // Assert: Verificar resultados
        var reclamoGuardado = await context.ReclamoPremio.FindAsync(resultId);
        Assert.NotNull(reclamoGuardado);
        Assert.Equal(resultId, reclamoGuardado.Id);
        Assert.Equal(reclamoPremioDomain.IdSubasta, reclamoGuardado.IdSubasta);
        Assert.Equal(reclamoPremioDomain.IdUsuario, reclamoGuardado.IdUsuario);
        Assert.Equal(reclamoPremioDomain.DireccionEnvio.direccionEnvio, reclamoGuardado.DireccionEnvio);
        Assert.Equal(reclamoPremioDomain.MetodoEntrega.metodoEntrega, reclamoGuardado.MetodoEntrega);
        Assert.Equal(reclamoPremioDomain.FechaReclamo.fechaReclamo, reclamoGuardado.FechaReclamo);
    }
}
