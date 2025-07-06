using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exception;
using Application.Query;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handler
{
    /// <summary>
    /// Clase Handler que se encarga consultar el reclamo realizado por un usuario con respecto premio obtenido.
    /// </summary>
    public class ConsultarReclamoHandler : IRequestHandler<ConsultarReclamoQuery, HistorialReclamosDTO>
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

        public ConsultarReclamoHandler(IUsuarioService usuarioService, IReclamoService reclamoService, ISubastaService subastaService)
        {
            _usuarioService = usuarioService;
            _reclamoService = reclamoService;
            _subastaService = subastaService;
        }

        /// <summary>
        /// Método que se encarga de procesar la consulta de un reclamo realizador por un usuario ganador de la subasta.
        /// </summary>
        /// <param name="request">Parametro que contiene el ID del reclamo del premio realizado por el usuario</param>
        /// <returns>Retorna un DTO con la información detallada del reclamo si las operaciones fueron exitosas.</returns>
        /// <exception cref="FalloAlObtenerReclamoException">
        /// Esta excepcion ocurre si ocurre un error al obtener el reclamo en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<HistorialReclamosDTO> Handle(ConsultarReclamoQuery request, CancellationToken cancellationToken)
        {

            try
            {
                //Se obtiene la información del reclamo según el ID dado
                var reclamo = await _reclamoService.ConsultarReclamoMongoAsync(request.idReclamo);

                //En caso de que la consulta no retorne ningún valor, se retorna un objeto vacío
                if (reclamo == null)
                {
                    return new HistorialReclamosDTO();
                }

                //Se obtiene la subasta a la cual se hizo el reclamo por medio del ID de la subasta en el reclamo
                var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);

                //Se obtiene el correo del usuario el cual hizo el reclamo por medio del ID del usuario en el reclamo
                var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(reclamo.IdUsuario);
               
                var reclamoDto= new HistorialReclamosDTO
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

                    };


                return reclamoDto;

            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerReclamoException("Ha ocurrido un error al obtener el reclamo en la bd", ex);
            }
        }
    }
}
