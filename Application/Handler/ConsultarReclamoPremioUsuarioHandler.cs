using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exception;
using Application.Query;
using Domain.Interfaces;
using MediatR;

namespace Application.Handler
{
    /// <summary>
    /// Clase Handler que se encarga consultar los reclamos de premios que han sido realizados a por un usuario.
    /// </summary>
    public class ConsultarReclamoPremioUsuarioHandler : IRequestHandler<ConsultarReclamosPremiosUsuarioQuery, List<HistorialReclamosPremioDTO>>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un reclamo, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IReclamoService _reclamoService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta en el Microservicio Subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;

        public ConsultarReclamoPremioUsuarioHandler(IUsuarioService usuarioService, IReclamoService reclamoService, ISubastaService subastaService)
        {
            _usuarioService = usuarioService;
            _reclamoService = reclamoService;
            _subastaService = subastaService;
        }

        /// <summary>
        /// Método que se encarga de procesar la consulta de los reclamos de premios realizador por un usuario ganador de las subasta.
        /// </summary>
        /// <param name="request">Parametro que contiene el correo del usuario quien consulta sus reclamos del premio</param>
        /// <returns>Retorna una lista de  DTOs con la información detallada de los reclamos de premios si las operaciones fueron exitosas.</returns>
        /// <exception cref="UsuarioNoEncontradoException">
        /// Esta excepcion ocurre si no se pudo obtener el ID del usuario en el Microservicio Usuarios.
        /// </exception>
        /// <exception cref="FalloAlObtenerReclamoException">
        /// Esta excepcion ocurre si ocurre un error al obtener la lista de reclamo de premios en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<List<HistorialReclamosPremioDTO>> Handle(ConsultarReclamosPremiosUsuarioQuery request, CancellationToken cancellationToken)
        {

            try
            {
                // Se obtiene el ID del usuario al que le pertenecen los reclamos de premio.
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                //En caso de que el ID del usuario retornado por la consulta sea vacío, se lanza la excepción
                if (idUsuario == Guid.Empty || idUsuario == null)
                    throw new UsuarioNoEncontradoException();

                //Se obtiene la información de los reclamos de premio según el ID del usuario
                var reclamosPremios = await _reclamoService.ConsultarReclamosPremiosUsuarioMongoAsync(idUsuario);

                //En caso de que la consulta no retorne ningún valor, se retorna una lista vacía
                if (reclamosPremios == null || !reclamosPremios.Any())
                {
                    return new List<HistorialReclamosPremioDTO>();
                }
                var listaReclamosPremios = new List<HistorialReclamosPremioDTO>();

                foreach (var reclamo in reclamosPremios)
                {
                    //Se obtiene la información de la subasta a la cuál se reclamó el premio
                    var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);

                    listaReclamosPremios.Add(new HistorialReclamosPremioDTO
                    {
                        id = subasta.Id,
                        nombreSubasta = subasta.nombreSubasta.Nombre,
                        descripcionSubasta = subasta.descripcionSubasta.descripcion,
                        fechaInicioSubasta = subasta.fechaInicioSubasta.fechaInicio,
                        fechaFinSubasta = subasta.fechaFinSubasta.fechaFin,
                        incrementoMinimoSubasta = subasta.incrementoMinimoSubasta.incrementoMinimo,
                        precioReservaSubasta = subasta.precioReservaSubasta.precioReserva,
                        estadoSubasta = subasta.estadoSubasta.estado,
                        idReclamoPremio = reclamo.Id,
                        direccionEnvio = reclamo.DireccionEnvio.direccionEnvio,
                        metodoEnvio = reclamo.MetodoEntrega.metodoEntrega,
                        fechaReclamo = reclamo.FechaReclamo.fechaReclamo

                    });
                }


                return listaReclamosPremios;

            }
            catch (UsuarioNoEncontradoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerReclamoException("Ha ocurrido un error al obtener los reclamos de premios del usuario en la bd", ex);
            }
        }
    }
}
