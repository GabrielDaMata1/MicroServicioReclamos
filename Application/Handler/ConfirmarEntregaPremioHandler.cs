using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exception;
using Application.Exceptions;
using Application.Query;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    /// <summary>
    /// Clase Handler que se encarga confirmar al subastador la recepción del premio por parte del usuario ganador de una subasta.
    /// </summary>
    public class ConfirmarEntregaPremioHandler : IRequestHandler<ConfirmarEntregaPremioCommand, bool>
    {
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
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;
        public ConfirmarEntregaPremioHandler(IReclamoService reclamoService, IPublishEndpoint publishEndpoint, INotificacionService notificacionService, ISubastaService subastaService, IUsuarioService usuarioService)
        {
            _reclamoService = reclamoService;
            _publishEndpoint = publishEndpoint;
            _subastaService=subastaService;
            _notificacionService=notificacionService;
            _usuarioService= usuarioService;
        }


        /// <summary>
        /// Método que se encarga de procesar la confirmación de la entrega de un premio por parte de un usuario ganador de la subasta.
        /// </summary>
        /// <param name="request">Parametro que contiene el ID del reclamo del premio realizado por el usuario.</param>
        /// <returns>Retorna un valor booleano True si las operaciones fueron exitosas.</returns>
        /// <exception cref="FalloAlEnviarCorreoException">
        /// Esta excepcion ocurre si no se pudo enviar el correo al subastador desde el Microservicio Notificaciones.
        /// </exception>
        /// <exception cref="FalloAlObtenerReclamoException">
        /// Esta excepcion ocurre si ocurre un error al obtener el reclamo en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<bool> Handle(ConfirmarEntregaPremioCommand request, CancellationToken cancellationToken)
        {

            try
            {
                // Se obtiene el reclamo del premio mediante el ID dado.
                var reclamoPremio = await _reclamoService.ConsultarReclamoPremioMongoAsync(request.idReclamoPremio);

                //En caso de que la consulta no retorne el reclamo deseado, se lanza una excepción
                if (reclamoPremio == null)
                {
                    throw new FalloAlObtenerReclamoException();
                }

                // notificar al subastador y al usuario

                //Se publica el mensaje en la cola de RabbitMQ para que el Microservicio Subasta procese el nuevo estado de la subasta a "Delivered"
                await _publishEndpoint.Publish(new EntregaPremioConfirmadaEvent(reclamoPremio.IdSubasta));

                //Se obtiene la subasta donde se confirma el reclamo del premio desde el Microservicio Subasta.
                var subasta = await _subastaService.ObtenerSubastaPorGuid(reclamoPremio.IdSubasta);

                //Se obtiene el correo del usuario quien confirma el reclamo del premio de la subasta desde el Microservicio Usuarios.
                var correoUsuario = await _usuarioService.ObtenerCorreoPorIdAsync(reclamoPremio.IdUsuario);

                //Se obtiene el correo del subastador que organizó la subasta desde el Microservicio Usuarios.
                var correoSubastador = await _usuarioService.ObtenerCorreoPorIdAsync(subasta.idUsuario);

                //Se envia la notificacion al subastador de la confirmación del reclamo del premio de su subasta desde el Microservicio Notificaciones.
                var notificacionSubastador = await _notificacionService.EnviarCorreoConfirmacionSubastadorReclamoPremio(correoSubastador, subasta.nombreSubasta.Nombre, correoUsuario);

                //En caso de que falle el envio del correo en el Microservicio Notificaciones, se lanza la excepción
                if (!notificacionSubastador)
                    throw new FalloAlEnviarCorreoException();

                return true;

            }
            catch (FalloAlObtenerReclamoException)
            {
                throw;
            }
            catch (FalloAlEnviarCorreoException)
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
