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
    /// Clase mapper que se encarga de mapear el objeto de tipo Entidad Reclamo (Dominio) a una entidad en la base de datos en MongoDB
    /// </summary>
    public static class ReclamoMongoMapper
    {
        /// <summary>
        /// Método que se encarga de mapear un Reclamo (Entidad) a un modelo en la base de datos en MongoDB.
        /// </summary>
        /// <param name="reclamo">Entidad que contiene los valores del reclamo a registrar</param>
        /// <returns>Retorna un objeto de tipo ReclamoMongo, que corresponde al modelo de reclamo en la base de datos en MongoDB.</returns>
        public static ReclamoMongo ToMongo(this Reclamo reclamo)
        {
            return new ReclamoMongo
            {
                Id = reclamo.Id,
                IdUsuario = reclamo.IdUsuario,
                IdSubasta = reclamo.IdSubasta,
                Descripcion = reclamo.Descripcion.descripcionReclamo,
                Motivo = reclamo.Motivo.motivoReclamo,
                UrlEvidencia = reclamo.UrlEvidencia.urlEvidenciaReclamo,
                Estado = reclamo.EstadoReclamo.estadoReclamo,
                CreatedAt = reclamo.FechaCreacion.fechaCreacionReclamo
            };
        }
    }
}
