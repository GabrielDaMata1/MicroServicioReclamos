using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Query
{
    public class ConsultarReclamoQuery : IRequest<HistorialReclamosDTO>
    {
        public Guid idReclamo { get; set; }

        public ConsultarReclamoQuery(Guid idReclamo)
        {
            this.idReclamo = idReclamo;
        }

    }
}
