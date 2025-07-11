using System;
using Application.Command;
using Application.DTOs;
using Xunit;

namespace Test.CommandTests
{
    public class RegistrarReclamoCommandTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarReclamoDtoCorrectamente()
        {
            // Arrange
            var dtoEsperado = new RegistrarReclamoDTO
            {
                correo = "usuario@example.com",
                idSubasta = Guid.NewGuid(),
                descripcion = "El producto llegó dañado.",
                motivo = "Producto defectuoso",
                urlEvidencia = "https://firebase.com/evidencias/123"
            };

            // Act
            var command = new RegistrarReclamoCommand(dtoEsperado);

            // Assert
            Assert.NotNull(command.reclamoDto);
            Assert.Equal(dtoEsperado.correo, command.reclamoDto.correo);
            Assert.Equal(dtoEsperado.idSubasta, command.reclamoDto.idSubasta);
            Assert.Equal(dtoEsperado.descripcion, command.reclamoDto.descripcion);
            Assert.Equal(dtoEsperado.motivo, command.reclamoDto.motivo);
            Assert.Equal(dtoEsperado.urlEvidencia, command.reclamoDto.urlEvidencia);
        }

        [Fact]
        public void Command_DeberiaImplementarIRequestDeTipoBool()
        {
            // Arrange & Act
            var commandType = typeof(RegistrarReclamoCommand);
            var interfaces = commandType.GetInterfaces();

            // Assert
            Assert.Contains(typeof(MediatR.IRequest<bool>), interfaces);
        }
    }
}
