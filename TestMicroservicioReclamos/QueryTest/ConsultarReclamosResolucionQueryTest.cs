using System;
using System.Collections.Generic;
using Application.Query;
using Application.DTOs;
using Xunit;

namespace Test.QueryTests
{
    public class ConsultarReclamosResolucionQueryTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarCorreoCorrectamente()
        {
            // Arrange
            var correoEsperado = "subastador@example.com";

            // Act
            var query = new ConsultarReclamosResolucionQuery(correoEsperado);

            // Assert
            Assert.Equal(correoEsperado, query.correo);
        }

        [Fact]
        public void Query_DeberiaImplementarIRequestDeListaReclamoResolucionDTO()
        {
            // Arrange & Act
            var queryType = typeof(ConsultarReclamosResolucionQuery);
            var interfaces = queryType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<List<ReclamoResolucionDTO>>), interfaces);
        }
    }
}
