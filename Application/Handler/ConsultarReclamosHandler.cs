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
    /// Clase Handler que se encarga consultar los reclamos que han sido realizados por los usuarios.
    /// </summary>
    public class ConsultarReclamosHandler : IRequestHandler<ConsultarReclamosQuery, List<HistorialReclamosDTO>>
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

        public ConsultarReclamosHandler(IUsuarioService usuarioService, IReclamoService reclamoService, ISubastaService subastaService)
        {
            _usuarioService = usuarioService;
            _reclamoService = reclamoService;
            _subastaService = subastaService;
        }

        /// <summary>
        /// Método que se encarga de procesar la consulta de los reclamos realizados.
        /// </summary>
        /// <returns>Retorna una lista de  DTOs con la información detallada de los reclamos si las operaciones fueron exitosas.</returns>
        /// <exception cref="FalloAlObtenerReclamoException">
        /// Esta excepcion ocurre si ocurre un error al obtener la lista de reclamos en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<List<HistorialReclamosDTO>> Handle(ConsultarReclamosQuery request, CancellationToken cancellationToken)
        {

            try
            {
                // Se obtienen los reclamos realizados por los usuarios en la base de datos en MongoDB.
                var reclamos = await _reclamoService.ConsultarReclamosMongoAsync();

                //En caso de que la consulta no retorne ningún valor, se retorna una lista vacía
                if (reclamos == null || !reclamos.Any())
                {
                    return new List<HistorialReclamosDTO>();
                }
                var listaReclamos = new List<HistorialReclamosDTO>();

                foreach (var reclamo in reclamos)
                {
                    //Se obtiene la información de la subasta a la cuál se realizó el reclamo
                    var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);

                    //Se obtiene el correo del usuario que realizó el reclamo
                    var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(reclamo.IdUsuario);
                    listaReclamos.Add(new HistorialReclamosDTO
                    {
                        id = reclamo.Id,
                        descripcion = reclamo.Descripcion.descripcionReclamo,
                        motivo = reclamo.Motivo.motivoReclamo,
                        urlEvidencia = reclamo.UrlEvidencia.urlEvidenciaReclamo,
                        fecha = reclamo.FechaCreacion.fechaCreacionReclamo,
                        correo = correoUsuario,
                        estado = reclamo.EstadoReclamo.estadoReclamo,
                        idSubasta = reclamo.IdSubasta,
                        nombreSubasta = subasta.nombreSubasta.Nombre

                    });
                }


                return listaReclamos;

            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerReclamoException("Ha ocurrido un error al obtener el reclamo en la bd", ex);
            }
        }
    }
}
