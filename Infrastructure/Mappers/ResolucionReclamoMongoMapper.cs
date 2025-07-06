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
    /// <summary>
    /// Clase mapper que se encarga de mapear el objeto de tipo Entidad ResolucionReclamo (Dominio) a una entidad en la base de datos en MongoDB
    /// </summary>
    public static class ResolucionReclamoMongoMapper
    {
        /// <summary>
        /// Método que se encarga de mapear un ResolucionReclamo (Entidad) a un modelo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="resolucionReclamo">Entidad que contiene los valores de la resolucion del reclamo del premio a registrar</param>
        /// <returns>Retorna un objeto de tipo ResolucionReclamoMongo, que corresponde al modelo de reclamo en la base de datos en MongoDB.</returns>
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
