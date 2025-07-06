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
    /// Clase Query que se encarga de enviar la solicitud para consultar los reclamo de premios realizados por un usuario .
    /// </summary>
    public class ConsultarReclamosPremiosUsuarioQuery : IRequest<List<HistorialReclamosPremioDTO>>
    {
        /// <summary>
        /// Atributo que contiene el correo del usuario 
        /// </summary>
        public string correo { get; set; }

        public ConsultarReclamosPremiosUsuarioQuery(string correo)
        {
            this.correo = correo;
        }
    }
}
