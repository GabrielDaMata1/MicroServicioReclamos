using Application.Command;
using Application.DTOs;
using Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioReclamos.Controllers
{
    /// <summary>
    /// Clase controller API encargada de procesar las solicitudes de inserción, eliminación, modificación y consulta,
    /// sobre los reclamos, resolución de reclamos y reclamos de premios.
    /// </summary>
    [ApiController]
    [Route("api/Reclamos")]
    public class ReclamoController : ControllerBase
    {
        /// <summary>
        /// Atributo que se encarga de enviar solicitudes (commands/queries) mediante el patrón mediador
        /// </summary>
        private readonly IMediator _mediator;
        public ReclamoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Endpoint encargado de registrar un reclamo de una subasta.
        /// </summary>
        /// <param name="reclamoDto">Parámetro de tipo DTO con los datos del reclamo de la subasta a registrar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>
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

        /// <summary>
        /// Endpoint encargado de consultar los reclamos de un subastador.
        /// </summary>
        /// <param name="correo">Parámetro que corresponde al correo del subastador.</param>
        /// <returns>Retorna una lista de DTOs con los detalles de cada reclamo.</returns>
        [HttpGet("obtenerReclamosSubastador/{correo}")]
        public async Task<IActionResult> ObtenerReclamosPorSubastador([FromRoute] string correo)
        {
            var resultado = await _mediator.Send(new ConsultarReclamosPorSubastadorQuery(correo));
              return Ok(resultado);
        }
        /// <summary>
        /// Endpoint encargado de consultar un reclamo de un subastador.
        /// </summary>
        /// <param name="idReclamo">Parámetro que corresponde al ID del reclamo a consultar.</param>
        /// <returns>Retorna un DTO con los detalles del reclamo.</returns>
        [HttpGet("obtenerReclamo/{idReclamo}")]
        public async Task<IActionResult> ObtenerReclamoPorSubastador([FromRoute] Guid idReclamo)
        {
            var resultado = await _mediator.Send(new ConsultarReclamoQuery(idReclamo));
            return Ok(resultado);
        }

        /// <summary>
        /// Endpoint encargado de consultar los reclamos realizados por los usuarios.
        /// </summary>
        /// <returns>Retorna una lista de DTOs con los detalles de los reclamos.</returns>
        [HttpGet("obtenerReclamos")]
        public async Task<IActionResult> ObtenerReclamos()
        {
            var resultado = await _mediator.Send(new ConsultarReclamosQuery());
            return Ok(resultado);
        }
        /// <summary>
        /// Endpoint encargado de registrar una resolución del reclamo de una subasta.
        /// </summary>
        /// <param name="resolucionReclamoDTO">Parámetro de tipo DTO con los datos de la resolución del reclamo de la subasta a registrar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>
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

        /// <summary>
        /// Endpoint encargado de consultar las resoluciones de los reclamos de un usuario.
        /// </summary>
        /// <param name="correo">Parámetro que corresponde al correo del usuario.</param>
        /// <returns>Retorna una lista de DTOs con los detalles de cada reclamo y su resolución.</returns>
        [HttpGet("obtenerReclamosResolucionUsuario/{correo}")]
        public async Task<IActionResult> ObtenerReclamosResolucionPorSubastador([FromRoute] string correo)
        {
            var resultado = await _mediator.Send(new ConsultarReclamosResolucionQuery(correo));
            return Ok(resultado);
        }

        /// <summary>
        /// Endpoint encargado de registrar el reclamo del premio de una subasta.
        /// </summary>
        /// <param name="reclamoPremioDTO">Parámetro de tipo DTO con los datos del reclamo del premio de la subasta a registrar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>
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

        /// <summary>
        /// Endpoint encargado de consultar los reclamos de premios de un usuario.
        /// </summary>
        /// <param name="correo">Parámetro que corresponde al correo del usuario.</param>
        /// <returns>Retorna una lista de DTOs con los detalles de cada reclamo de premio.</returns>
        [HttpGet("obtenerReclamosPremiosUsuario/{correo}")]
        public async Task<IActionResult> ObtenerReclamosPremiosUsuario([FromRoute] string correo)
        {
            var resultado = await _mediator.Send(new ConsultarReclamosPremiosUsuarioQuery(correo));
            return Ok(resultado);
        }
        /// <summary>
        /// Endpoint encargado de confirmar la recepción del premio de un usuario.
        /// </summary>
        /// <param name="idReclamoPremio">Parámetro que corresponde al ID del reclamo del premio realizado por el usuario.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>
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
