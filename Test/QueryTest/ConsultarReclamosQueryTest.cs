using System;
using System.Collections.Generic;
using Application.Query;
using Application.DTOs;
using Xunit;

namespace Test.QueryTests
{
    public class ConsultarReclamosQueryTests
    {
        [Fact]
        public void Query_DeberiaImplementarIRequestDeListaHistorialReclamosDTO()
        {
            // Arrange & Act
            var queryType = typeof(ConsultarReclamosQuery);
            var interfaces = queryType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<List<HistorialReclamosDTO>>), interfaces);
        }

        [Fact]
        public void Constructor_SinParametros_DeberiaCrearInstancia()
        {
            // Act
            var query = new ConsultarReclamosQuery();

            // Assert
            Assert.NotNull(query);
        }
    }
}
