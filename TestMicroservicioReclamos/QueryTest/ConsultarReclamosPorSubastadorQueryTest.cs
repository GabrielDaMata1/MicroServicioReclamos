using System;
using System.Collections.Generic;
using Application.Query;
using Application.DTOs;
using Xunit;

namespace Test.QueryTests
{
    public class ConsultarReclamosPorSubastadorQueryTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarCorreoCorrectamente()
        {
            // Arrange
            var correoEsperado = "subastador@example.com";

            // Act
            var query = new ConsultarReclamosPorSubastadorQuery(correoEsperado);

            // Assert
            Assert.Equal(correoEsperado, query.correo);
        }

        [Fact]
        public void Query_DeberiaImplementarIRequestDeListaHistorialReclamosDTO()
        {
            // Arrange & Act
            var queryType = typeof(ConsultarReclamosPorSubastadorQuery);
            var interfaces = queryType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<List<HistorialReclamosDTO>>), interfaces);
        }
    }
}
