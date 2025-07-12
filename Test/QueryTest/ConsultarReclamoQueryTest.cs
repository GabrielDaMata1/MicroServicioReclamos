using System;
using Application.Query;
using Application.DTOs;
using Xunit;

namespace Test.QueryTests
{
    public class ConsultarReclamoQueryTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarIdReclamoCorrectamente()
        {
            // Arrange
            var idEsperado = Guid.NewGuid();

            // Act
            var query = new ConsultarReclamoQuery(idEsperado);

            // Assert
            Assert.Equal(idEsperado, query.idReclamo);
        }

        [Fact]
        public void Query_DeberiaImplementarIRequestDeHistorialReclamosDTO()
        {
            // Arrange & Act
            var queryType = typeof(ConsultarReclamoQuery);
            var interfaces = queryType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<HistorialReclamosDTO>), interfaces);
        }
    }
}
