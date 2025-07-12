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
    /// Clase Handler que se encarga registrar la resolucion de un reclamo de premio realizado por un usuario ganador de la subasta.
    /// </summary>
    public class RegistrarResolucionReclamoHandler : IRequestHandler<RegistrarResolucionReclamoCommand, bool>
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

        public RegistrarResolucionReclamoHandler(IUsuarioService usuarioService, IReclamoService reclamoService, IPublishEndpoint publishEndpoint, ISubastaService subastaService, INotificacionService notificacionService)
        {
            _reclamoService = reclamoService;
            _usuarioService = usuarioService;
            _publishEndpoint = publishEndpoint;
            _subastaService = subastaService;
            _notificacionService= notificacionService;
        }
        /// <summary>
        /// Método que se encarga de procesar el registro de la resolucion de un reclamo realizado por parte de un usuario.
        /// </summary>
        /// <param name="request">Parametro que contiene un DTO con la información de la resolución del reclamo y el ID del reclamo</param>
        /// <returns>Retorna un valor booleano True si las operaciones fueron exitosas.</returns>
        /// <exception cref="FalloAlEnviarCorreoException">
        /// Esta excepcion ocurre si no se pudo enviar el correo al subastador desde el Microservicio Notificaciones.
        /// </exception>
        /// <exception cref="FalloAlRegistrarReclamoException">
        /// Esta excepcion ocurre si ocurre un error al registrar la resolución del reclamo del premio en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<bool> Handle(RegistrarResolucionReclamoCommand request, CancellationToken cancellationToken)
        {

            try
            {

                //Se crea la instancia del objeto ResolucionReclamo
                var resolucionReclamo = ResolucionReclamoFactory.CrearResolucionReclamo(request.resolucionReclamoDTO.idReclamo, request.resolucionReclamoDTO.resolucion);

                //Se registra la resolución del reclamo en la base de datos de PostgreSQL
                var idresolucionReclamo = await _reclamoService.RegistrarResolucionReclamoPostgreSQLAsync(resolucionReclamo);

                // En caso de que la operación para registrar la resolución del reclamo en PostgreSQL no sea exitosa, se lanza una excepción.
                if (idresolucionReclamo == Guid.Empty)
                    throw new FalloAlRegistrarReclamoException("Fallo al registrar la resolucion del reclamo en PostgreSQL");

                //Se actualiza el estado del reclamo en la base de datos de PostgreSQL
                var actualizacionEstado = await _reclamoService.ActualizarEstadoReclamoPostgreSQLAsync(request.resolucionReclamoDTO.idReclamo, "Resuelto");

                //Se publica el mensaje en la cola de RabbitMQ para sincronizar la base de datos de MongoDB con PostgreSQL
                await _publishEndpoint.Publish(new ResolucionReclamoRegistradaEvent(resolucionReclamo, request.resolucionReclamoDTO.idReclamo));
                Console.WriteLine(request.resolucionReclamoDTO.idReclamo);
                //Se obtiene el reclamo al cual se le está ofreciendo su resolución
                var reclamo = await _reclamoService.ConsultarReclamoMongoAsync(request.resolucionReclamoDTO.idReclamo);

                Console.WriteLine(reclamo.IdSubasta);

                //Se obtiene el correo del usuario quien confirma recibe la resolución de su reclamo de la subasta desde el Microservicio Usuarios.
                var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(reclamo.IdUsuario);

                //Se obtiene la subasta donde se hace la resolución del reclamo desde el Microservicio Subasta.
                var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamo.IdSubasta);

                //Se envia la notificacion al usuario de la resolución del reclamo de su subasta ganada desde el Microservicio Notificaciones.
                var notificacionUsuario = await _notificacionService.EnviarCorreoUsuarioResolucionReclamo(correoUsuario,
                    subasta.nombreSubasta.Nombre, request.resolucionReclamoDTO.resolucion);

                if (!notificacionUsuario)
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
            catch (PostgresRepositoryException)
            {
                throw;
            }
            catch (FalloAlEnviarCorreoException)
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
