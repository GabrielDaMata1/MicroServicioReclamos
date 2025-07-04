using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.PostgreSQL;

namespace Infrastructure.Mappers
{
    public static class ReclamoPostgreSQLMapper
    {
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
