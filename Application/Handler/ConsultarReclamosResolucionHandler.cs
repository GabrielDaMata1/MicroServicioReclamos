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
    /// Clase Handler que se encarga consultar las resoluciones de los reclamos que han sido realizados por un usuario.
    /// </summary>
    public class ConsultarReclamosResolucionHandler : IRequestHandler<ConsultarReclamosResolucionQuery, List<ReclamoResolucionDTO>>
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

        public ConsultarReclamosResolucionHandler(IUsuarioService usuarioService, IReclamoService reclamoService, ISubastaService subastaService)
        {
            _usuarioService = usuarioService;
            _reclamoService = reclamoService;
            _subastaService = subastaService;
        }

        /// <summary>
        /// Método que se encarga de procesar la consulta de las resoluciones de los reclamos realizados por un usuario.
        /// </summary>
        /// <param name="request">Parametro que contiene el correo del usuario quien consulta sus reclamos</param>
        /// <returns>Retorna una lista de  DTOs con la información detallada de los reclamos y su resolución si las operaciones fueron exitosas.</returns>
        /// <exception cref="UsuarioNoEncontradoException">
        /// Esta excepcion ocurre si no se pudo obtener el ID del usuario en el Microservicio Usuarios.
        /// </exception>
        /// <exception cref="FalloAlObtenerReclamoException">
        /// Esta excepcion ocurre si ocurre un error al obtener la lista de reclamos en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<List<ReclamoResolucionDTO>> Handle(ConsultarReclamosResolucionQuery request, CancellationToken cancellationToken)
        {

            try
            {

                // Se obtiene el ID del usuario al que le pertenecen los reclamos.
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                //En caso de que el ID del usuario retornado por la consulta sea vacío, se lanza la excepción
                if (idUsuario == Guid.Empty || idUsuario == null)
                    throw new UsuarioNoEncontradoException();

                //Se obtienen los reclamos realizados por el usuario
                var reclamos = await _reclamoService.ConsultarReclamosUsuarioMongoAsync(idUsuario);

                //En caso de que la consulta no retorne ningún valor, se retorna una lista vacía
                if (reclamos == null || !reclamos.Any())
                {
                    return new List<ReclamoResolucionDTO>();
                }
                var listaReclamos = new List<ReclamoResolucionDTO>();

                foreach (var reclamo in reclamos)
                {
                    //Se obtiene la información de la subasta a la cuál se realizó el reclamo
                    var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);


                    //Se obtiene la resolución de cada reclamo
                    var resolucion = await _reclamoService.ConsultarResolucionReclamoMongoAsync(reclamo.Id);
                    listaReclamos.Add(new ReclamoResolucionDTO
                    {
                        idReclamo = reclamo.Id,
                        descripcion = reclamo.Descripcion.descripcionReclamo,
                        motivo = reclamo.Motivo.motivoReclamo,
                        urlEvidencia = reclamo.UrlEvidencia.urlEvidenciaReclamo,
                        fecha = reclamo.FechaCreacion.fechaCreacionReclamo,
                        correo = request.correo,
                        estado = reclamo.EstadoReclamo.estadoReclamo,
                        idSubasta = reclamo.IdSubasta,
                        nombreSubasta = subasta.nombreSubasta.Nombre,
                        idResolucion = resolucion?.IdReclamo ?? Guid.Empty,
                        descripcionResolucion = resolucion?.Descripcion.descripcionResolucion ?? "Por determinar",
                        fechaResolucion = resolucion?.FechaResolucion.fechaResolucion ?? DateTime.MinValue



                    });
                }


                return listaReclamos;

            }
            catch (UsuarioNoEncontradoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerReclamoException("Ha ocurrido un error al obtener el reclamo y su resolucion en la bd", ex);
            }
        }
    }
}
