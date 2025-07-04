using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using MediatR;

namespace Application.Query
{
    public class ConsultarReclamosResolucionQuery : IRequest<List<ReclamoResolucionDTO>>
    {
        public string correo { get; set; }

        public ConsultarReclamosResolucionQuery(string correo)
        {
            this.correo = correo;
        }
    }
}
