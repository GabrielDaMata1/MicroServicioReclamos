using System;
using System.Collections.Generic;
using Application.Query;
using Application.DTOs;
using Xunit;

namespace Test.QueryTests
{
    public class ConsultarReclamosUsuarioQueryTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarCorreoCorrectamente()
        {
            // Arrange
            var correoEsperado = "usuario@example.com";

            // Act
            var query = new ConsultarReclamosUsuarioQuery(correoEsperado);

            // Assert
            Assert.Equal(correoEsperado, query.correo);
        }

        [Fact]
        public void Query_DeberiaImplementarIRequestDeListaHistorialReclamosDTO()
        {
            // Arrange & Act
            var queryType = typeof(ConsultarReclamosUsuarioQuery);
            var interfaces = queryType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<List<HistorialReclamosDTO>>), interfaces);
        }
    }
}
