using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Query
{
    /// <summary>
    /// Clase Query que se encarga de enviar la solicitud para consultar un reclamo en específico realizado por a un usuario .
    /// </summary>
    public class ConsultarReclamoQuery : IRequest<HistorialReclamosDTO>
    {
        /// <summary>
        /// Atributo que contiene el ID del reclamo a consultar.
        /// </summary>
        public Guid idReclamo { get; set; }

        public ConsultarReclamoQuery(Guid idReclamo)
        {
            this.idReclamo = idReclamo;
        }

    }
}
