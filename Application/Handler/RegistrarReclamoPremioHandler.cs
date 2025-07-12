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
    /// <summary>
    /// Clase Handler que se encarga registrar un reclamo de premio realizado por un usuario ganador de la subasta.
    /// </summary>
    public class RegistrarReclamoPremioHandler : IRequestHandler<RegistrarReclamoPremioCommand, bool>
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
        /// Atributo que corresponde a las publicación de mensajes a la cola de RabbitMQ.
        /// </summary>
        private readonly IPublishEndpoint _publishEndpoint;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre las notificaciones en el Microservicio Notificaciones, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly INotificacionService _notificacionService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre una subasta en el Microservicio Subasta, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly ISubastaService _subastaService;

        public RegistrarReclamoPremioHandler(IUsuarioService usuarioService, IReclamoService reclamoService, IPublishEndpoint publishEndpoint, INotificacionService notificacionService, ISubastaService subastaService)
        {
            _reclamoService = reclamoService;
            _usuarioService = usuarioService;
            _publishEndpoint = publishEndpoint;
            _notificacionService = notificacionService;
            _subastaService = subastaService;
        }

        /// <summary>
        /// Método que se encarga de procesar el registro de un reclamos de premio por parte de un usuario.
        /// </summary>
        /// <param name="request">Parametro que contiene un DTO con el correo del usuario que realiza el reclamo del premio, el ID de la subasta
        /// y el detalle del reclamo.</param>
        /// <returns>Retorna un valor booleano True si las operaciones fueron exitosas.</returns>
        /// <exception cref="UsuarioNoEncontradoException">
        /// Esta excepcion ocurre si no se pudo obtener el ID del usuario en el Microservicio Usuarios.
        /// </exception>
        /// <exception cref="FalloAlEnviarCorreoException">
        /// Esta excepcion ocurre si no se pudo enviar el correo al subastador desde el Microservicio Notificaciones.
        /// </exception>
        /// <exception cref="FalloAlRegistrarReclamoException">
        /// Esta excepcion ocurre si ocurre un error al registrar el reclamo del premio en la base de datos o si ocurre un error inesperado.
        /// </exception>

        public async Task<bool> Handle(RegistrarReclamoPremioCommand request, CancellationToken cancellationToken)
        {

            try
            {

                // Se obtiene el ID del usuario al que realiza el reclamo de premio.
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo);

                //En caso de que el ID del usuario retornado por la consulta sea vacío, se lanza la excepción
                if (idUsuario == Guid.Empty || idUsuario == null)
                    throw new UsuarioNoEncontradoException();

                //Se crea la instancia del objeto ReclamoPremio
                var reclamoPremio = ReclamoPremioFactory.CrearReclamoPremio(idUsuario, request.reclamoDto.idSubasta, request.reclamoDto.direccionEnvio, request.reclamoDto.metodoEntrega);

                //Se registra el reclamo del premio en la base de datos de PostgreSQL
                var idReclamoPremio = await _reclamoService.RegistrarReclamoPremioPostgreSQLAsync(reclamoPremio);

                // En caso de que la operación para registrar el reclamo del premio en PostgreSQL no sea exitosa, se lanza una excepción.
                if (idReclamoPremio == Guid.Empty)
                    throw new FalloAlRegistrarReclamoException();

                //Se publica el mensaje en la cola de RabbitMQ para sincronizar la base de datos de MongoDB con PostgreSQL
                await _publishEndpoint.Publish(new ReclamoPremioRegistradoEvent(reclamoPremio));

                //Se obtiene la subasta donde se reclama el premio desde el Microservicio Subasta.
                var subasta = await _subastaService.ObtenerSubastaPorGuid(request.reclamoDto.idSubasta);

                //Se obtiene el correo del subastador que organizó la subasta desde el Microservicio Usuarios.
                var correoSubastador = await _usuarioService.ObtenerCorreoPorIdAsync(subasta.idUsuario);

                //Se envia la notificacion al subastador del reclamo del premio de su subasta desde el Microservicio Notificaciones.
                var notificacionSubastador = await _notificacionService.EnviarCorreoSubastadorReclamoPremio(correoSubastador, subasta.nombreSubasta.Nombre, request.reclamoDto.correo);

                //En caso de que falle el envio del correo en el Microservicio Notificaciones, se lanza la excepción
                if (!notificacionSubastador)
                    throw new FalloAlEnviarCorreoException();

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
            catch (FalloAlEnviarCorreoException)
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
