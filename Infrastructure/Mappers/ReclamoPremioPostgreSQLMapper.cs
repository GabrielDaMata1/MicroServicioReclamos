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
    /// Clase mapper que se encarga de mapear el objeto de tipo Entidad ReclamoPremio (Dominio) a una entidad en la base de datos en PostgreSQL
    /// </summary>
    public static class ReclamoPremioPostgreSQLMapper
    {
        /// <summary>
        /// Método que se encarga de mapear un ReclamoPremio (Entidad) a un modelo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="reclamoPremio">Entidad que contiene los valores del reclamo del premio a registrar</param>
        /// <returns>Retorna un objeto de tipo ReclamoPremioPostgreSQL, que corresponde al modelo de reclamo en la base de datos en PostgreSQL.</returns>
        public static ReclamoPremioPostgreSQL ToPostgres(this ReclamoPremio reclamoPremio)
        {
            return new ReclamoPremioPostgreSQL
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
