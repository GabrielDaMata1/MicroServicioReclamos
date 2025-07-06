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
    /// Clase consumer que se encarga de consumir el evente ReclamoPremioRegistradoEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class ReclamoPremioRegistradoConsumer : IConsumer<ReclamoPremioRegistradoEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre el reclamo de un premio, reclamo, y resolución de un reclamo, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IReclamoService _reclamoService;

        public ReclamoPremioRegistradoConsumer(IReclamoService reclamoService)
        {
            _reclamoService = reclamoService;
        }
        /// <summary>
        /// Método que se encarga de procesar el registro de un reclamo de un premio en la base de datos de MongoBD.
        /// </summary>
        /// <param name="context">Parametro que contiene el objeto ReclamoPremio con su detalle.</param>
        /// <exception cref="FalloAlRegistrarReclamoException">
        /// Esta excepcion ocurre si ocurre un error al registrar el reclamo de premio en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task Consume(ConsumeContext<ReclamoPremioRegistradoEvent> context)
        {
            try
            {
                //Se registra el reclamo del premio en la base de datos en MongoDB
                await _reclamoService.RegistrarReclamoPremioMongoAsync(context.Message.reclamoPremio);
            }
            catch (Exception ex)
            {
                throw new FalloAlRegistrarReclamoException("Ha ocurrido un error al registrar el reclamo del premio en MongoDb");
            }

        }
    }
}
