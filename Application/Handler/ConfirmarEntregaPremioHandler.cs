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


        public ConfirmarEntregaPremioHandler(IReclamoService reclamoService, IPublishEndpoint publishEndpoint)
        {
            _reclamoService = reclamoService;
            _publishEndpoint = publishEndpoint;
        }


        /// <summary>
        /// Método que se encarga de procesar la confirmación de la entrega de un premio por parte de un usuario ganador de la subasta.
        /// </summary>
        /// <param name="request">Parametro que contiene el ID del reclamo del premio realizado por el usuario.</param>
        /// <returns>Retorna un valor booleano True si las operaciones fueron exitosas.</returns>
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
