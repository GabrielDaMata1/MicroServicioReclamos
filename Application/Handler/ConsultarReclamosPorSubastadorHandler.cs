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
    public class ConsultarReclamosPorSubastadorHandler : IRequestHandler<ConsultarReclamosPorSubastadorQuery, List<HistorialReclamosDTO>>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IReclamoService _reclamoService;
        private readonly ISubastaService _subastaService;

        public ConsultarReclamosPorSubastadorHandler(IUsuarioService usuarioService, IReclamoService reclamoService, ISubastaService subastaService)
        {
            _usuarioService = usuarioService;
            _reclamoService = reclamoService;
            _subastaService = subastaService;
        }

        public async Task<List<HistorialReclamosDTO>> Handle(ConsultarReclamosPorSubastadorQuery request, CancellationToken cancellationToken)
        {

            try
            {

                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                if (idUsuario == Guid.Empty || idUsuario == null)
                    throw new UsuarioNoEncontradoException();

                var reclamos = await _reclamoService.ConsultarReclamosPorSubastadorMongoAsync(idUsuario);

                if (reclamos == null || !reclamos.Any())
                {
                    return new List<HistorialReclamosDTO>();
                }
                var listaReclamos = new List<HistorialReclamosDTO>();

                foreach (var reclamo in reclamos)
                {
                    var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);

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
            catch (UsuarioNoEncontradoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerReclamoException("Ha ocurrido un error al obtener el reclamo en la bd", ex);
            }
        }
    }
}
