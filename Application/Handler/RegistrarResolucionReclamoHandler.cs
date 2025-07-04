using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.Exception;
using Application.Exceptions;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    public class RegistrarResolucionReclamoHandler : IRequestHandler<RegistrarResolucionReclamoCommand, bool>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IReclamoService _reclamoService;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegistrarResolucionReclamoHandler(IUsuarioService usuarioService, IReclamoService reclamoService, IPublishEndpoint publishEndpoint)
        {
            _reclamoService = reclamoService;
            _usuarioService = usuarioService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(RegistrarResolucionReclamoCommand request, CancellationToken cancellationToken)
        {

            try
            {

                var resolucionReclamo = ResolucionReclamoFactory.CrearResolucionReclamo(request.resolucionReclamoDTO.idReclamo, request.resolucionReclamoDTO.resolucion);
                var idresolucionReclamo = await _reclamoService.RegistrarResolucionReclamoPostgreSQLAsync(resolucionReclamo);
                
                if (idresolucionReclamo == Guid.Empty)
                    throw new FalloAlRegistrarReclamoException("Fallo al registrar la resolucion del reclamo en PostgreSQL");

                var actualizacionEstado = await _reclamoService.ActualizarEstadoReclamoPostgreSQLAsync(request.resolucionReclamoDTO.idReclamo, "Resuelto");

                await _publishEndpoint.Publish(new ResolucionReclamoRegistradaEvent(resolucionReclamo, request.resolucionReclamoDTO.idReclamo));

                return true;

            }
            catch (UsuarioNoEncontradoException)
            {
                throw;
            }
            catch (FalloAlRegistrarReclamoException)
            {
                throw;
            }
            catch (PostgresRepositoryException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlRegistrarReclamoException("Ha ocurrido un error al registrar la resolucion del reclamo en la BD", ex);
            }
        }
    }
}
