using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exception;
using Application.Query;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    public class ConfirmarEntregaPremioHandler : IRequestHandler<ConfirmarEntregaPremioCommand, bool>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IReclamoService _reclamoService;
        private readonly ISubastaService _subastaService;
        private readonly IPublishEndpoint _publishEndpoint;


        public ConfirmarEntregaPremioHandler(IUsuarioService usuarioService, IReclamoService reclamoService, ISubastaService subastaService, IPublishEndpoint publishEndpoint)
        {
            _usuarioService = usuarioService;
            _reclamoService = reclamoService;
            _subastaService = subastaService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(ConfirmarEntregaPremioCommand request, CancellationToken cancellationToken)
        {

            try
            {

                var reclamoPremio = await _reclamoService.ConsultarReclamoPremioMongoAsync(request.idReclamoPremio);

                if (reclamoPremio == null)
                {
                    throw new FalloAlObtenerReclamoException();
                }

                // notificar al subastador y al usuario

                await _publishEndpoint.Publish(new EntregaPremioConfirmadaEvent(reclamoPremio.IdSubasta));


                return true;

            }
            catch (FalloAlObtenerReclamoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerReclamoException("Ha ocurrido un error al obtener el reclamo de premio del usuario en la bd", ex);
            }
        }
    }
}
