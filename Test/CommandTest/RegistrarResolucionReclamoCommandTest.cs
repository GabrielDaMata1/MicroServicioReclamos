using System;
using Application.Command;
using Application.DTOs;
using Xunit;

namespace Test.CommandTests
{
    public class RegistrarResolucionReclamoCommandTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarResolucionReclamoDtoCorrectamente()
        {
            // Arrange
            var dtoEsperado = new RegistrarResolucionReclamoDTO
            {
                resolucion = "Se devuelve el dinero al usuario.",
                idReclamo = Guid.NewGuid()
            };

            // Act
            var command = new RegistrarResolucionReclamoCommand(dtoEsperado);

            // Assert
            Assert.NotNull(command.resolucionReclamoDTO);
            Assert.Equal(dtoEsperado.resolucion, command.resolucionReclamoDTO.resolucion);
            Assert.Equal(dtoEsperado.idReclamo, command.resolucionReclamoDTO.idReclamo);
        }

        [Fact]
        public void Command_DeberiaImplementarIRequestDeTipoBool()
        {
            // Arrange & Act
            var commandType = typeof(RegistrarResolucionReclamoCommand);
            var interfaces = commandType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<bool>), interfaces);
        }
    }
}
