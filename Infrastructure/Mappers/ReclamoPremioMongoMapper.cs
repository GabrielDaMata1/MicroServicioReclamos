using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.MongoDB;

namespace Infrastructure.Mappers
{
    public static class ReclamoPremioMongoMapper
    {
        public static ReclamoPremioMongo ToMongo(this ReclamoPremio reclamoPremio)
        {
            return new ReclamoPremioMongo
            {
                Id = reclamoPremio.Id,
                IdUsuario = reclamoPremio.IdUsuario,
                IdSubasta = reclamoPremio.IdSubasta,
                DireccionEnvio = reclamoPremio.DireccionEnvio.direccionEnvio,
                MetodoEntrega = reclamoPremio.MetodoEntrega.metodoEntrega,
                FechaReclamo = reclamoPremio.FechaReclamo.fechaReclamo,
            };
        }
    }
}
