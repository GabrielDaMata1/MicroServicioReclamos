using Application.Command;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioReclamos.Controllers
{
    [ApiController]
    [Route("api/Pagos")]
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
    }
}
