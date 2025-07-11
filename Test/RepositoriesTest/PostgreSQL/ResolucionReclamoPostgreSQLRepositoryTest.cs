using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;
using Infrastructure.Persistance;
using Infrastructure.Repositories.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ResolucionReclamoPostgreSQLRepositoryTests
{
    [Fact]
    public async Task RegistrarResolucionReclamo_ShouldAddEntityAndSave_ReturnsId()
    {
        // Arrange
        var resolucionReclamoDomain = new ResolucionReclamo(
            Guid.NewGuid(),                                           // IdReclamo
            new DescripcionResolucionVO("Reclamo resuelto exitosamente"), // Descripcion
            new FechaResolucionVO(DateTime.UtcNow)                    // FechaResolucion
        );

        var options = new DbContextOptionsBuilder<SubastaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Base limpia por prueba
            .Options;

        using var context = new SubastaDbContext(options);
        var repository = new ResolucionReclamoPostgreSQLRepository(context);

        // Act
        var resultId = await repository.RegistrarResolucionReclamo(resolucionReclamoDomain);

        // Assert
        var resolucionGuardada = await context.ResolucionReclamo.FindAsync(resultId);
        Assert.NotNull(resolucionGuardada);
        Assert.Equal(resultId, resolucionGuardada.Id);
        Assert.Equal(resolucionReclamoDomain.IdReclamo, resolucionGuardada.IdReclamo);
        Assert.Equal(resolucionReclamoDomain.Descripcion.descripcionResolucion, resolucionGuardada.Descripcion);
        Assert.Equal(resolucionReclamoDomain.FechaResolucion.fechaResolucion, resolucionGuardada.CreatedAt);
    }

}
