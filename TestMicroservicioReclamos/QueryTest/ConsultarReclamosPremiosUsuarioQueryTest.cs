using System;
using System.Collections.Generic;
using Application.Query;
using Application.DTOs;
using Xunit;

namespace Test.QueryTests
{
    public class ConsultarReclamosPremiosUsuarioQueryTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarCorreoCorrectamente()
        {
            // Arrange
            var correoEsperado = "usuario@example.com";

            // Act
            var query = new ConsultarReclamosPremiosUsuarioQuery(correoEsperado);

            // Assert
            Assert.Equal(correoEsperado, query.correo);
        }

        [Fact]
        public void Query_DeberiaImplementarIRequestDeListaHistorialReclamosPremioDTO()
        {
            // Arrange & Act
            var queryType = typeof(ConsultarReclamosPremiosUsuarioQuery);
            var interfaces = queryType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<List<HistorialReclamosPremioDTO>>), interfaces);
        }
    }
}
