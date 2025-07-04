using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.MongoDB;
using Infrastructure.Models.PostgreSQL;

namespace Infrastructure.Mappers
{
    public static class ResolucionReclamoMongoMapper
    {
        public static ResolucionReclamoMongo ToMongo(this ResolucionReclamo resolucionReclamo)
        {
            return new ResolucionReclamoMongo
            {
                Id = resolucionReclamo.Id,
                IdReclamo = resolucionReclamo.IdReclamo,
                Descripcion = resolucionReclamo.Descripcion.descripcionResolucion,
                CreatedAt = resolucionReclamo.FechaResolucion.fechaResolucion
            };
        }
    }
}
