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
    public class ConsultarReclamoHandler : IRequestHandler<ConsultarReclamoQuery, HistorialReclamosDTO>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IReclamoService _reclamoService;
        private readonly ISubastaService _subastaService;

        public ConsultarReclamoHandler(IUsuarioService usuarioService, IReclamoService reclamoService, ISubastaService subastaService)
        {
            _usuarioService = usuarioService;
            _reclamoService = reclamoService;
            _subastaService = subastaService;
        }

        public async Task<HistorialReclamosDTO> Handle(ConsultarReclamoQuery request, CancellationToken cancellationToken)
        {

            try
            {

                var reclamo = await _reclamoService.ConsultarReclamoMongoAsync(request.idReclamo);

                if (reclamo == null)
                {
                    return new HistorialReclamosDTO();
                }
                var listaReclamos = new List<HistorialReclamosDTO>();

                var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);

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
