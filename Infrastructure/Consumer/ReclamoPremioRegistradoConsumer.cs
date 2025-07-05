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
    public class ReclamoPremioRegistradoConsumer : IConsumer<ReclamoPremioRegistradoEvent>
    {
        private readonly IReclamoService _reclamoService;

        public ReclamoPremioRegistradoConsumer(IReclamoService reclamoService)
        {
            _reclamoService = reclamoService;
        }
        public async Task Consume(ConsumeContext<ReclamoPremioRegistradoEvent> context)
        {
            try
            {
                await _reclamoService.RegistrarReclamoPremioMongoAsync(context.Message.reclamoPremio);
            }
            catch (Exception ex)
            {
                throw new FalloAlRegistrarReclamoException("Ha ocurrido un error al registrar el reclamo del premio en MongoDb");
            }

        }
    }
}
