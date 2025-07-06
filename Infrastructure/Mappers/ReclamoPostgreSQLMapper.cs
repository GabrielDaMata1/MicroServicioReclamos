using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.PostgreSQL;

namespace Infrastructure.Mappers
{
    /// <summary>
    /// Clase mapper que se encarga de mapear el objeto de tipo Entidad Reclamo (Dominio) a una entidad en la base de datos en PostgreSQL
    /// </summary>
    public static class ReclamoPostgreSQLMapper
    {
        /// <summary>
        /// Método que se encarga de mapear un Reclamo (Entidad) a un modelo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamo">Entidad que contiene los valores del reclamo a registrar</param>
        /// <returns>Retorna un objeto de tipo ReclamoPostgreSQL, que corresponde al modelo de reclamo en la base de datos en PostgreSQL.</returns>
        public static ReclamoPostgreSQL ToPostgres(this Reclamo reclamo)
        {
            return new ReclamoPostgreSQL
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
