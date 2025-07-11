using System;
using Application.Command;
using Application.DTOs;
using Xunit;

namespace Test.CommandTests
{
    public class RegistrarReclamoPremioCommandTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarReclamoDtoCorrectamente()
        {
            // Arrange
            var dtoEsperado = new RegistrarReclamoPremioDTO
            {
                idSubasta = Guid.NewGuid(),
                correo = "usuario@example.com",
                direccionEnvio = "Av. Siempre Viva 123",
                metodoEntrega = "Envio a domicilio"
            };

            // Act
            var command = new RegistrarReclamoPremioCommand(dtoEsperado);

            // Assert
            Assert.NotNull(command.reclamoDto);
            Assert.Equal(dtoEsperado.idSubasta, command.reclamoDto.idSubasta);
            Assert.Equal(dtoEsperado.correo, command.reclamoDto.correo);
            Assert.Equal(dtoEsperado.direccionEnvio, command.reclamoDto.direccionEnvio);
            Assert.Equal(dtoEsperado.metodoEntrega, command.reclamoDto.metodoEntrega);
        }

        [Fact]
        public void Command_DeberiaImplementarIRequestDeTipoBool()
        {
            // Arrange & Act
            var commandType = typeof(RegistrarReclamoPremioCommand);
            var interfaces = commandType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<bool>), interfaces);
        }
    }
}
