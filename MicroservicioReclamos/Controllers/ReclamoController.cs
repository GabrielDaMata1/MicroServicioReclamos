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
    }
}
