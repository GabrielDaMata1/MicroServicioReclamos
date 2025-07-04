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
    public class ReclamoRegistradoConsumer : IConsumer<ReclamoRegistradoEvent>
    {
        private readonly IReclamoService _reclamoService;

        public ReclamoRegistradoConsumer(IReclamoService reclamoService)
        {
            _reclamoService = reclamoService;
        }
        public async Task Consume(ConsumeContext<ReclamoRegistradoEvent> context)
        {
            try
            {
                await _reclamoService.RegistrarReclamoMongoAsync(context.Message.reclamo);
            }
            catch (Exception ex) {
                throw new FalloAlRegistrarReclamoException("Ha ocurrido un error al registrar el reclamo en MongoDb");
            }

        }
}
}
