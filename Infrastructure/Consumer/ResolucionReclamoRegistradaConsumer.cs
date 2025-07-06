using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumer
{
    /// <summary>
    /// Clase consumer que se encarga de consumir el evente ResolucionReclamoRegistradaEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class ResolucionReclamoRegistradaConsumer : IConsumer<ResolucionReclamoRegistradaEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre el reclamo de un premio, reclamo, y resolución de un reclamo, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IReclamoService _reclamoService;

        public ResolucionReclamoRegistradaConsumer(IReclamoService reclamoService)
        {
            _reclamoService = reclamoService;
        }
        /// <summary>
        /// Método que se encarga de procesar el registro de una resolución de un reclamo  en la base de datos de MongoBD.
        /// </summary>
        /// <param name="context">Parametro que contiene el objeto ResolucionReclamo con su detalle.</param>
        /// <exception cref="FalloAlRegistrarReclamoException">
        /// Esta excepcion ocurre si ocurre un error al registrar la resolución de un reclamo en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<ResolucionReclamoRegistradaEvent> context)
        {
            try
            {
                //Se registra la resolución del reclamo en la bases de datos en MongoDB
                await _reclamoService.RegistrarResolucionReclamoMongoAsync(context.Message.resolucionReclamo);

                //Se actualiza el estado del reclamo correspondiente a la resolución en la bases de datos en MongoDB
                await _reclamoService.ActualizarEstadoReclamoMongoAsync(context.Message.idReclamo, "Resuelto");
            }
            catch (Exception ex)
            {
                throw new FalloAlRegistrarReclamoException("Ha ocurrido un error al registrar la resolucion del reclamo en MongoDb", ex);
            }

        }
    }
}
