using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;
using Infrastructure.Models.MongoDB;
using Infrastructure.Repositories.MongoDB;
using MongoDB.Driver;
using Moq;
using Xunit;

public class ResolucionReclamoMongoRepositoryTest
{
    private readonly Mock<IMongoCollection<ResolucionReclamoMongo>> _resolucionReclamoCollectionMock;
    private readonly ResolucionReclamoMongoRepository _repository;

    public ResolucionReclamoMongoRepositoryTest()
    {
        var mongoClientMock = new Mock<IMongoClient>();
        var databaseMock = new Mock<IMongoDatabase>();
        var subastaServiceMock = new Mock<ISubastaService>(); // aunque no se use, es necesario para el constructor

        _resolucionReclamoCollectionMock = new Mock<IMongoCollection<ResolucionReclamoMongo>>();

        databaseMock.Setup(db => db.GetCollection<ResolucionReclamoMongo>("ResolucionReclamo", null))
            .Returns(_resolucionReclamoCollectionMock.Object);

        mongoClientMock.Setup(m => m.GetDatabase("MicroservicioReclamos", null))
            .Returns(databaseMock.Object);

        _repository = new ResolucionReclamoMongoRepository(mongoClientMock.Object, subastaServiceMock.Object);
    }

    [Fact]
    public async Task RegistrarResolucionReclamoMongo_Should_Insert_And_Return_OK()
    {
        // Arrange
        var resolucionReclamo = new ResolucionReclamo(
            Guid.NewGuid(),
            new DescripcionResolucionVO("Descripción de prueba"),
            new FechaResolucionVO(DateTime.UtcNow)
        );

        _resolucionReclamoCollectionMock
            .Setup(c => c.InsertOneAsync(It.IsAny<ResolucionReclamoMongo>(), null, default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _repository.RegistrarResolucionReclamoMongo(resolucionReclamo);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result);
        _resolucionReclamoCollectionMock.Verify(c => c.InsertOneAsync(It.IsAny<ResolucionReclamoMongo>(), null, default), Times.Once);
    }
}
