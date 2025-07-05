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
    public class ConsultarReclamoPremioUsuarioHandler : IRequestHandler<ConsultarReclamosPremiosUsuarioQuery, List<HistorialReclamosPremioDTO>>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IReclamoService _reclamoService;
        private readonly ISubastaService _subastaService;

        public ConsultarReclamoPremioUsuarioHandler(IUsuarioService usuarioService, IReclamoService reclamoService, ISubastaService subastaService)
        {
            _usuarioService = usuarioService;
            _reclamoService = reclamoService;
            _subastaService = subastaService;
        }

        public async Task<List<HistorialReclamosPremioDTO>> Handle(ConsultarReclamosPremiosUsuarioQuery request, CancellationToken cancellationToken)
        {

            try
            {

                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                if (idUsuario == Guid.Empty || idUsuario == null)
                    throw new UsuarioNoEncontradoException();

                var reclamosPremios = await _reclamoService.ConsultarReclamosPremiosUsuarioMongoAsync(idUsuario);

                if (reclamosPremios == null || !reclamosPremios.Any())
                {
                    return new List<HistorialReclamosPremioDTO>();
                }
                var listaReclamosPremios = new List<HistorialReclamosPremioDTO>();

                foreach (var reclamo in reclamosPremios)
                {
                    var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);

                    var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(reclamo.IdUsuario);
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
