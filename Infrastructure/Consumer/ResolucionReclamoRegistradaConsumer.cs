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
    public class ResolucionReclamoRegistradaConsumer : IConsumer<ResolucionReclamoRegistradaEvent>
    {
        private readonly IReclamoService _reclamoService;

        public ResolucionReclamoRegistradaConsumer(IReclamoService reclamoService)
        {
            _reclamoService = reclamoService;
        }
        public async Task Consume(ConsumeContext<ResolucionReclamoRegistradaEvent> context)
        {
            try
            {
                await _reclamoService.RegistrarResolucionReclamoMongoAsync(context.Message.resolucionReclamo);

                await _reclamoService.ActualizarEstadoReclamoMongoAsync(context.Message.idReclamo, "Resuelto");
            }
            catch (Exception ex)
            {
                throw new FalloAlRegistrarReclamoException("Ha ocurrido un error al registrar la resolucion del reclamo en MongoDb", ex);
            }

        }
    }
}
