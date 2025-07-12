using System;
using Application.Command;
using Xunit;

namespace Test.CommandTests
{
    public class ConfirmarEntregaPremioCommandTests
    {
        [Fact]
        public void Constructor_DeberiaAsignarIdCorrectamente()
        {
            // Arrange
            Guid idEsperado = Guid.NewGuid();

            // Act
            var command = new ConfirmarEntregaPremioCommand(idEsperado);

            // Assert
            Assert.Equal(idEsperado, command.idReclamoPremio);
        }
    }
}
