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
    public static class ReclamoPremioPostgreSQLMapper
    {
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
