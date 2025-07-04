using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Query
{
    public class ConsultarReclamosPorSubastadorQuery : IRequest<List<HistorialReclamosDTO>>
    {
        public string correo { get; set; }

        public ConsultarReclamosPorSubastadorQuery(string correo) 
        {
            this.correo = correo;
        }
    }
}
