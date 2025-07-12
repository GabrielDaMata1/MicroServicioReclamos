using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;
using Infrastructure.Mappers;
using Infrastructure.Models.MongoDB;
using Infrastructure.Repositories.MongoDB;
using MongoDB.Driver;
using Moq;
using Xunit;

public class ReclamoPremioMongoRepositoryTest
{
    private readonly Mock<IMongoCollection<ReclamoPremioMongo>> _reclamoPremioCollectionMock;
    private readonly ReclamoPremioMongoRepository _repository;

    public ReclamoPremioMongoRepositoryTest()
    {
        var mongoClientMock = new Mock<IMongoClient>();
        var databaseMock = new Mock<IMongoDatabase>();

        _reclamoPremioCollectionMock = new Mock<IMongoCollection<ReclamoPremioMongo>>();

        // Configurar que GetCollection devuelva el mock
        databaseMock.Setup(db => db.GetCollection<ReclamoPremioMongo>("ReclamosPremios", null))
            .Returns(_reclamoPremioCollectionMock.Object);

        mongoClientMock.Setup(m => m.GetDatabase("MicroservicioReclamos", null))
            .Returns(databaseMock.Object);

        _repository = new ReclamoPremioMongoRepository(mongoClientMock.Object);
    }

    [Fact]
    public async Task RegistrarReclamoPremioAsync_Should_Insert_And_Return_OK()
    {
        // Arrange
        var reclamoPremio = new ReclamoPremio(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DireccionEnvioPremioVO("Dirección de prueba"),
            new MetodoEntregaPremioVO("Courier"),
            new FechaReclamoPremioVO(DateTime.UtcNow)
        );

        _reclamoPremioCollectionMock
            .Setup(c => c.InsertOneAsync(It.IsAny<ReclamoPremioMongo>(), null, default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _repository.RegistrarReclamoPremioAsync(reclamoPremio);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result);
        _reclamoPremioCollectionMock.Verify(c => c.InsertOneAsync(It.IsAny<ReclamoPremioMongo>(), null, default), Times.Once);
    }
}
