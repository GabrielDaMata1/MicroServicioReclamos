using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.Exception;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    public class RegistrarReclamoPremioHandler : IRequestHandler<RegistrarReclamoPremioCommand, bool>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IReclamoService _reclamoService;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegistrarReclamoPremioHandler(IUsuarioService usuarioService, IReclamoService reclamoService, IPublishEndpoint publishEndpoint)
        {
            _reclamoService = reclamoService;
            _usuarioService = usuarioService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(RegistrarReclamoPremioCommand request, CancellationToken cancellationToken)
        {

            try
            {

                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo);

                if (idUsuario == Guid.Empty || idUsuario == null)
                    throw new UsuarioNoEncontradoException();

                var reclamoPremio = ReclamoPremioFactory.CrearReclamoPremio(idUsuario, request.reclamoDto.idSubasta, request.reclamoDto.direccionEnvio, request.reclamoDto.metodoEntrega);

                var idReclamoPremio = await _reclamoService.RegistrarReclamoPremioPostgreSQLAsync(reclamoPremio);

                if (idReclamoPremio == Guid.Empty)
                    throw new FalloAlRegistrarReclamoException();

                await _publishEndpoint.Publish(new ReclamoPremioRegistradoEvent(reclamoPremio));

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
            catch (System.Exception ex)
            {
                throw new FalloAlRegistrarReclamoException("Ha ocurrido un error al registrar el reclamo del premio en la BD", ex);
            }
        }
    }
}
