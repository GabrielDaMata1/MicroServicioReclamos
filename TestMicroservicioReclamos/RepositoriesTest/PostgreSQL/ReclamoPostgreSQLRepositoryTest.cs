using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;
using Infrastructure.Models.PostgreSQL;
using Infrastructure.Persistance;
using Infrastructure.Repositories.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ReclamoPostgreSQLRepositoryTests
{
    [Fact]
    public async Task RegistrarReclamoAsync_Should_Add_And_Save()
    {
        // Arrange
        var reclamoDomain = new Reclamo(
            Guid.NewGuid(), // IdUsuario
            Guid.NewGuid(), // IdSubasta
            new DescripcionReclamoVO("Descripción de prueba"),
            new MotivoReclamoVO("Motivo de prueba"),
            new UrlEvidenciaReclamoVO("http://example.com/evidencia"),
            new FechaCreacionReclamoVO(DateTime.UtcNow),
            new EstadoReclamoVO("Pendiente")
        );

        var options = new DbContextOptionsBuilder<SubastaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new SubastaDbContext(options);
        var repository = new ReclamoPostgreSQLRepository(context);

        // Act
        var idRetornado = await repository.RegistrarReclamoAsync(reclamoDomain);

        // Assert
        var reclamoGuardado = await context.Reclamo.FindAsync(idRetornado);
        Assert.NotNull(reclamoGuardado);
        Assert.Equal(idRetornado, reclamoGuardado.Id);
        Assert.Equal(reclamoDomain.Descripcion.descripcionReclamo, reclamoGuardado.Descripcion);
        Assert.Equal(reclamoDomain.Motivo.motivoReclamo, reclamoGuardado.Motivo);
        Assert.Equal(reclamoDomain.UrlEvidencia.urlEvidenciaReclamo, reclamoGuardado.UrlEvidencia);
        Assert.Equal(reclamoDomain.EstadoReclamo.estadoReclamo, reclamoGuardado.Estado);
    }

    [Fact]
    public async Task ActualizarEstadoReclamo_Should_Find_Update_And_Save()
    {
        // Arrange
        var reclamoId = Guid.NewGuid();
        var nuevoEstado = "Resuelto";

        var options = new DbContextOptionsBuilder<SubastaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new SubastaDbContext(options);

        // Insertar reclamo inicial
        var reclamoEntity = new ReclamoPostgreSQL
        {
            Id = reclamoId,
            IdUsuario = Guid.NewGuid(),
            IdSubasta = Guid.NewGuid(),
            Descripcion = "Descripción inicial",
            Motivo = "Motivo inicial",
            UrlEvidencia = "http://example.com",
            CreatedAt = DateTime.UtcNow,
            Estado = "Pendiente"
        };
        context.Reclamo.Add(reclamoEntity);
        await context.SaveChangesAsync();

        var repository = new ReclamoPostgreSQLRepository(context);

        // Act
        var result = await repository.ActualizarEstadoReclamo(reclamoId, nuevoEstado);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result);

        var reclamoActualizado = await context.Reclamo.FindAsync(reclamoId);
        Assert.NotNull(reclamoActualizado);
        Assert.Equal(nuevoEstado, reclamoActualizado.Estado);
    }
}
