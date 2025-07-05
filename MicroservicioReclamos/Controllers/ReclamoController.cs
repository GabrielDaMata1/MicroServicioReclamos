using Application.Command;
using Application.DTOs;
using Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioReclamos.Controllers
{
    [ApiController]
    [Route("api/Reclamos")]
    public class ReclamoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReclamoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("registroReclamo")]
        public async Task<IActionResult> RegistrarReclamo([FromBody] RegistrarReclamoDTO reclamoDto)
        {
            var resultado = await _mediator.Send(new RegistrarReclamoCommand(reclamoDto));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "El reclamo se registró exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "El reclamo no pudo ser registrada.", Exito = false });
        }

        [HttpGet("obtenerReclamosSubastador/{correo}")]
        public async Task<IActionResult> ObtenerReclamosPorSubastador([FromRoute] string correo)
        {
            var resultado = await _mediator.Send(new ConsultarReclamosPorSubastadorQuery(correo));
              return Ok(resultado);
        }

        [HttpGet("obtenerReclamo/{idReclamo}")]
        public async Task<IActionResult> ObtenerReclamoPorSubastador([FromRoute] Guid idReclamo)
        {
            var resultado = await _mediator.Send(new ConsultarReclamoQuery(idReclamo));
            return Ok(resultado);
        }

        [HttpGet("obtenerReclamos")]
        public async Task<IActionResult> ObtenerReclamos()
        {
            var resultado = await _mediator.Send(new ConsultarReclamosQuery());
            return Ok(resultado);
        }

        [HttpPost("registroResolucionReclamo")]
        public async Task<IActionResult> RegistrarResolucionReclamo([FromBody] RegistrarResolucionReclamoDTO resolucionReclamoDTO)
        {
            var resultado = await _mediator.Send(new RegistrarResolucionReclamoCommand(resolucionReclamoDTO));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "La resolución del reclamo se registró exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "La resolución del reclamo  no pudo ser registrada.", Exito = false });
        }


        [HttpGet("obtenerReclamosResolucionSubastador/{correo}")]
        public async Task<IActionResult> ObtenerReclamosResolucionPorSubastador([FromRoute] string correo)
        {
            var resultado = await _mediator.Send(new ConsultarReclamosResolucionQuery(correo));
            return Ok(resultado);
        }

        [HttpPost("registroReclamoPremio")]
        public async Task<IActionResult> RegistrarReclamoPremio([FromBody] RegistrarReclamoPremioDTO reclamoPremioDTO)
        {
            var resultado = await _mediator.Send(new RegistrarReclamoPremioCommand(reclamoPremioDTO));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "El reclamo del premio se registró exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "El reclamo del premio  no pudo ser registrada.", Exito = false });
        }


        [HttpGet("obtenerReclamosPremiosUsuario/{correo}")]
        public async Task<IActionResult> ObtenerReclamosPremiosUsuario([FromRoute] string correo)
        {
            var resultado = await _mediator.Send(new ConsultarReclamosPremiosUsuarioQuery(correo));
            return Ok(resultado);
        }

        [HttpPost("confirmarEntregaPremio/{idReclamoPremio}")]
        public async Task<IActionResult> ConfirmarEntregaPremio([FromRoute] Guid idReclamoPremio)
        {
            var resultado = await _mediator.Send(new ConfirmarEntregaPremioCommand(idReclamoPremio));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "El reclamo del premio se ha confirmado exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "El reclamo del premio  no pudo ser confirmado.", Exito = false });
        }
    }
}
