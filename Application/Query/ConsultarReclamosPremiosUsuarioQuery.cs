using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Query
{
    public class ConsultarReclamosPremiosUsuarioQuery : IRequest<List<HistorialReclamosPremioDTO>>
    {
        public string correo { get; set; }

        public ConsultarReclamosPremiosUsuarioQuery(string correo)
        {
            this.correo = correo;
        }
    }
}
