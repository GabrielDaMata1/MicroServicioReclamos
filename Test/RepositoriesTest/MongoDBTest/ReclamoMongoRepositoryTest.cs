using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;
using Infrastructure.Mappers;
using Infrastructure.Models.MongoDB;
using Infrastructure.Repositories.MongoDB;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Infrastructure.Tests.Repositories.MongoDB
{
    public class ReclamoMongoRepositoryTests
    {
        private readonly Mock<IMongoClient> _mongoClientMock;
        private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
        private readonly Mock<IMongoCollection<ReclamoMongo>> _reclamoCollectionMock;
        private readonly Mock<ISubastaService> _subastaServiceMock;
        private readonly ReclamoMongoRepository _repository;

        public ReclamoMongoRepositoryTests()
        {
            _mongoClientMock = new Mock<IMongoClient>();
            _mongoDatabaseMock = new Mock<IMongoDatabase>();
            _reclamoCollectionMock = new Mock<IMongoCollection<ReclamoMongo>>();
            _subastaServiceMock = new Mock<ISubastaService>();

            _mongoClientMock.Setup(m => m.GetDatabase("MicroservicioReclamos", null))
                            .Returns(_mongoDatabaseMock.Object);

            _mongoDatabaseMock.Setup(d => d.GetCollection<ReclamoMongo>("Reclamos", null))
                              .Returns(_reclamoCollectionMock.Object);

            _repository = new ReclamoMongoRepository(_mongoClientMock.Object, _subastaServiceMock.Object);
        }

        [Fact]
        public async Task RegistrarReclamoMongo_Success_ReturnsOk()
        {
            // Arrange: crear un objeto Domain.Entities.Reclamo con VOs válidos
            var reclamoDomain = new Reclamo(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DescripcionReclamoVO("Descripcion Test"),
                new MotivoReclamoVO("Motivo Test"),
                new UrlEvidenciaReclamoVO("http://example.com/evidencia"),
                new FechaCreacionReclamoVO(DateTime.UtcNow),
                new EstadoReclamoVO("Pendiente")
            );

            _reclamoCollectionMock
                .Setup(c => c.InsertOneAsync(It.IsAny<ReclamoMongo>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _repository.RegistrarReclamoMongo(reclamoDomain);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);
            _reclamoCollectionMock.Verify(c => c.InsertOneAsync(It.IsAny<ReclamoMongo>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once());
        }



        [Fact]
        public async Task RegistrarReclamoMongo_ThrowsException_ThrowsMongoRepositoryException()
        {
            // Arrange
            var reclamoDomain = new Reclamo(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DescripcionReclamoVO("Descripción test"),
                new MotivoReclamoVO("Motivo test"),
                new UrlEvidenciaReclamoVO("http://example.com"),
                new FechaCreacionReclamoVO(DateTime.UtcNow),
                new EstadoReclamoVO("Pendiente")
            );

            var exception = new Exception("Database error");

            _reclamoCollectionMock
                .Setup(c => c.InsertOneAsync(It.IsAny<ReclamoMongo>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<MongoRepositoryException>(() => _repository.RegistrarReclamoMongo(reclamoDomain));

            Assert.Contains("Error al intentar registrar el historial de pagos en MongoDB", ex.Message);
            Assert.Equal(exception, ex.InnerException);
        }


        [Fact]
        public async Task ConsultarReclamosMongo_ReturnsListOfReclamos()
        {
            // Arrange
            var reclamosMongo = new List<ReclamoMongo>
            {
                new ReclamoMongo
                {
                    Id = Guid.NewGuid(),
                    IdUsuario = Guid.NewGuid(),
                    IdSubasta = Guid.NewGuid(),
                    Descripcion = "Test",
                    Motivo = "Motivo",
                    UrlEvidencia = "http://example.com",
                    CreatedAt = DateTime.UtcNow,
                    Estado = "Pendiente"
                }
            };

            var asyncCursorMock = new Mock<IAsyncCursor<ReclamoMongo>>();
            asyncCursorMock.SetupSequence(_ => _.MoveNextAsync(default)).ReturnsAsync(true).ReturnsAsync(false);
            asyncCursorMock.Setup(_ => _.Current).Returns(reclamosMongo);

            _reclamoCollectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<ReclamoMongo>>(), It.IsAny<FindOptions<ReclamoMongo>>(), default))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _repository.ConsultarReclamosMongo();

            // Assert
            Assert.Single(result);
            Assert.Equal(reclamosMongo[0].Id, result[0].Id);
            _reclamoCollectionMock.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<ReclamoMongo>>(), It.IsAny<FindOptions<ReclamoMongo>>(), default), Times.Once());
        }
        /*
        [Fact]
        public async Task ConsultarReclamoMongo_ReturnsReclamo()
        {
            // Arrange
            var reclamoMongo = new ReclamoMongo
            {
                Id = Guid.NewGuid(),
                IdUsuario = Guid.NewGuid(),
                IdSubasta = Guid.NewGuid(),
                Descripcion = "Test",
                Motivo = "Motivo",
                UrlEvidencia = "http://example.com",
                CreatedAt = DateTime.UtcNow,
                Estado = "Pendiente"
            };

            var asyncCursorMock = new Mock<IAsyncCursor<ReclamoMongo>>();
            asyncCursorMock
                .SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            asyncCursorMock
                .Setup(x => x.Current)
                .Returns(new List<ReclamoMongo> { reclamoMongo });

            var findFluentMock = new Mock<IFindFluent<ReclamoMongo, ReclamoMongo>>();
            findFluentMock
                .Setup(f => f.ToCursorAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursorMock.Object);

            _reclamoCollectionMock
                .Setup(c => c.Find(
                    It.IsAny<FilterDefinition<ReclamoMongo>>(),
                    It.IsAny<FindOptions>()
                ))
                .Returns(findFluentMock.Object);

            // Act
            var result = await _repository.ConsultarReclamoMongo(reclamoMongo.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reclamoMongo.Id, result.Id);
        }
        /*
        En lugar de:

        csharp
        Copy
        Edit
        var resolucionReclamoMongo = await _resolucionReclamoCollection
            .Find(r => r.IdReclamo == idReclamo)
            .FirstOrDefaultAsync();
        Usa:

        csharp
        Copy
        Edit
        var cursor = _resolucionReclamoCollection.FindSync(r => r.IdReclamo == idReclamo);
        var resolucionReclamoMongo = cursor.FirstOrDefault();
        🔍 FindSync() sí puede ser interceptado por Moq, ya que es un método de la interfaz IMongoCollection<T>.
        */


        [Fact]
        public async Task ConsultarReclamosPorSubastadorMongo_ReturnsReclamosForSubastador()
        {
            // Arrange
            var idSubastador = Guid.NewGuid();
            var reclamoMongo = new ReclamoMongo
            {
                Id = Guid.NewGuid(),
                IdUsuario = Guid.NewGuid(),
                IdSubasta = Guid.NewGuid(),
                Descripcion = "Test",
                Motivo = "Motivo",
                UrlEvidencia = "http://example.com",
                CreatedAt = DateTime.UtcNow,
                Estado = "Pendiente"
            };

            var nombreVO = new NombreSubastaVO("Subasta 1");
            var descripcionVO = new DescripcionSubastaVO("Descripción de prueba");
            var fechaInicioVO = new FechaInicioSubastaVO(DateTime.UtcNow);
            var fechaFinVO = new FechaFinSubastaVO(DateTime.UtcNow.AddDays(7));
            var incrementoMinimoVO = new IncrementoMinimoSubastaVO(100m);
            var precioReservaVO = new PrecioReservaSubastaVO(500m);
            var estadoVO = new EstadoSubastaVO("Pending");

            var subasta = new Subasta(
                reclamoMongo.IdSubasta, // Id de subasta como Guid
                nombreVO,
                descripcionVO,
                Guid.NewGuid(), // idProductoSubasta
                fechaInicioVO,
                fechaFinVO,
                incrementoMinimoVO,
                precioReservaVO,
                estadoVO,
                idSubastador
            );

            var asyncCursorMock = new Mock<IAsyncCursor<ReclamoMongo>>();
            asyncCursorMock.SetupSequence(_ => _.MoveNextAsync(default)).ReturnsAsync(true).ReturnsAsync(false);
            asyncCursorMock.Setup(_ => _.Current).Returns(new List<ReclamoMongo> { reclamoMongo });

            _reclamoCollectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<ReclamoMongo>>(), It.IsAny<FindOptions<ReclamoMongo>>(), default))
                .ReturnsAsync(asyncCursorMock.Object);
            _subastaServiceMock.Setup(s => s.ObtenerSubastaPorGuid(reclamoMongo.IdSubasta)).ReturnsAsync(subasta);

            // Act
            var result = await _repository.ConsultarReclamosPorSubastadorMongo(idSubastador);

            // Assert
            Assert.Single(result);
            Assert.Equal(reclamoMongo.Id, result[0].Id);
            _subastaServiceMock.Verify(s => s.ObtenerSubastaPorGuid(reclamoMongo.IdSubasta), Times.Once());
        }


        [Fact]
        public async Task ConsultarReclamosPorSubastadorMongo_NoReclamos_ReturnsEmptyList()
        {
            // Arrange
            var idSubastador = Guid.NewGuid();
            var asyncCursorMock = new Mock<IAsyncCursor<ReclamoMongo>>();
            asyncCursorMock.SetupSequence(_ => _.MoveNextAsync(default)).ReturnsAsync(true).ReturnsAsync(false);
            asyncCursorMock.Setup(_ => _.Current).Returns(new List<ReclamoMongo>());

            _reclamoCollectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<ReclamoMongo>>(), It.IsAny<FindOptions<ReclamoMongo>>(), default))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _repository.ConsultarReclamosPorSubastadorMongo(idSubastador);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ActualizarEstadoReclamo_Success_ReturnsOk()
        {
            // Arrange
            var idReclamo = Guid.NewGuid();
            var nuevoEstado = "Resuelto";

            _reclamoCollectionMock.Setup(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<ReclamoMongo>>(),
                It.IsAny<UpdateDefinition<ReclamoMongo>>(),
                It.IsAny<UpdateOptions>(),
                default))
                .ReturnsAsync(new UpdateResult.Acknowledged(1, 1, null));

            // Act
            var result = await _repository.ActualizarEstadoReclamo(idReclamo, nuevoEstado);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result);
            _reclamoCollectionMock.Verify(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<ReclamoMongo>>(),
                It.IsAny<UpdateDefinition<ReclamoMongo>>(),
                It.IsAny<UpdateOptions>(),
                default), Times.Once());
        }

        [Fact]
        public async Task ConsultarReclamosUsuarioMongo_ReturnsReclamosForUser()
        {
            // Arrange
            var idUsuario = Guid.NewGuid();
            var reclamoMongo = new ReclamoMongo
            {
                Id = Guid.NewGuid(),
                IdUsuario = idUsuario,
                IdSubasta = Guid.NewGuid(),
                Descripcion = "Test",
                Motivo = "Motivo",
                UrlEvidencia = "http://example.com",
                CreatedAt = DateTime.UtcNow,
                Estado = "Pendiente"
            };

            var asyncCursorMock = new Mock<IAsyncCursor<ReclamoMongo>>();
            asyncCursorMock.SetupSequence(_ => _.MoveNextAsync(default)).ReturnsAsync(true).ReturnsAsync(false);
            asyncCursorMock.Setup(_ => _.Current).Returns(new List<ReclamoMongo> { reclamoMongo });

            _reclamoCollectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<ReclamoMongo>>(), It.IsAny<FindOptions<ReclamoMongo>>(), default))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _repository.ConsultarReclamosUsuarioMongo(idUsuario);

            // Assert
            Assert.Single(result);
            Assert.Equal(reclamoMongo.Id, result[0].Id);
            Assert.Equal(idUsuario, result[0].IdUsuario);
        }
    }
}
