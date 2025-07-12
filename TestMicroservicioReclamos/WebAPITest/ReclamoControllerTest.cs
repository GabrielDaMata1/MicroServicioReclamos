using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Query;
using MediatR;
using MicroservicioReclamos.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestMicroservicioReclamos.WebAPITest
{
    public class ReclamoControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ReclamoController _controller;

        public ReclamoControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ReclamoController(_mediatorMock.Object);
        }

        [Fact]
        public async Task RegistrarReclamo_DebeRetornarOk_SiResultadoEsTrue()
        {
            var dto = new RegistrarReclamoDTO();
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarReclamoCommand>(), default)).ReturnsAsync(true);

            var result = await _controller.RegistrarReclamo(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var payload = Assert.IsType<ResultadoDTO>(okResult.Value);
            Assert.True(payload.Exito);
        }

        [Fact]
        public async Task RegistrarReclamo_DebeRetornarBadRequest_SiResultadoEsFalse()
        {
            var dto = new RegistrarReclamoDTO();
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarReclamoCommand>(), default)).ReturnsAsync(false);

            var result = await _controller.RegistrarReclamo(dto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var payload = Assert.IsType<ResultadoDTO>(badResult.Value);
            Assert.False(payload.Exito);
        }

        [Fact]
        public async Task ObtenerReclamosPorSubastador_DebeRetornarOk()
        {
            var correo = "subastador@email.com";
            var lista = new List<HistorialReclamosDTO>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ConsultarReclamosPorSubastadorQuery>(), default)).ReturnsAsync(lista);

            var result = await _controller.ObtenerReclamosPorSubastador(correo);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(lista, okResult.Value);
        }

        [Fact]
        public async Task ObtenerReclamoPorSubastador_DebeRetornarOk()
        {
            var id = Guid.NewGuid();
            var dto = new HistorialReclamosDTO();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ConsultarReclamoQuery>(), default)).ReturnsAsync(dto);

            var result = await _controller.ObtenerReclamoPorSubastador(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task ObtenerReclamos_DebeRetornarOk()
        {
            var lista = new List<HistorialReclamosDTO>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ConsultarReclamosQuery>(), default)).ReturnsAsync(lista);

            var result = await _controller.ObtenerReclamos();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(lista, okResult.Value);
        }

        [Fact]
        public async Task RegistrarResolucionReclamo_DebeRetornarOk_SiResultadoEsTrue()
        {
            var dto = new RegistrarResolucionReclamoDTO();
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarResolucionReclamoCommand>(), default)).ReturnsAsync(true);

            var result = await _controller.RegistrarResolucionReclamo(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var payload = Assert.IsType<ResultadoDTO>(okResult.Value);
            Assert.True(payload.Exito);
        }

        [Fact]
        public async Task RegistrarResolucionReclamo_DebeRetornarBadRequest_SiResultadoEsFalse()
        {
            var dto = new RegistrarResolucionReclamoDTO();
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarResolucionReclamoCommand>(), default)).ReturnsAsync(false);

            var result = await _controller.RegistrarResolucionReclamo(dto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var payload = Assert.IsType<ResultadoDTO>(badResult.Value);
            Assert.False(payload.Exito);
        }

        [Fact]
        public async Task ObtenerReclamosResolucionPorSubastador_DebeRetornarOk()
        {
            var correo = "usuario@test.com";
            var lista = new List<ReclamoResolucionDTO>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ConsultarReclamosResolucionQuery>(), default)).ReturnsAsync(lista);

            var result = await _controller.ObtenerReclamosResolucionPorSubastador(correo);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(lista, okResult.Value);
        }

        [Fact]
        public async Task RegistrarReclamoPremio_DebeRetornarOk_SiResultadoEsTrue()
        {
            var dto = new RegistrarReclamoPremioDTO();
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarReclamoPremioCommand>(), default)).ReturnsAsync(true);

            var result = await _controller.RegistrarReclamoPremio(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var payload = Assert.IsType<ResultadoDTO>(okResult.Value);
            Assert.True(payload.Exito);
        }

        [Fact]
        public async Task RegistrarReclamoPremio_DebeRetornarBadRequest_SiResultadoEsFalse()
        {
            var dto = new RegistrarReclamoPremioDTO();
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegistrarReclamoPremioCommand>(), default)).ReturnsAsync(false);

            var result = await _controller.RegistrarReclamoPremio(dto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var payload = Assert.IsType<ResultadoDTO>(badResult.Value);
            Assert.False(payload.Exito);
        }

        [Fact]
        public async Task ObtenerReclamosPremiosUsuario_DebeRetornarOk()
        {
            var correo = "usuario@test.com";
            var lista = new List<HistorialReclamosPremioDTO>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ConsultarReclamosPremiosUsuarioQuery>(), default)).ReturnsAsync(lista);

            var result = await _controller.ObtenerReclamosPremiosUsuario(correo);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(lista, okResult.Value);
        }

        [Fact]
        public async Task ConfirmarEntregaPremio_DebeRetornarOk_SiResultadoEsTrue()
        {
            var id = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ConfirmarEntregaPremioCommand>(), default)).ReturnsAsync(true);

            var result = await _controller.ConfirmarEntregaPremio(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var payload = Assert.IsType<ResultadoDTO>(okResult.Value);
            Assert.True(payload.Exito);
        }

        [Fact]
        public async Task ConfirmarEntregaPremio_DebeRetornarBadRequest_SiResultadoEsFalse()
        {
            var id = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ConfirmarEntregaPremioCommand>(), default)).ReturnsAsync(false);

            var result = await _controller.ConfirmarEntregaPremio(id);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var payload = Assert.IsType<ResultadoDTO>(badResult.Value);
            Assert.False(payload.Exito);
        }

    }
}
