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
    /// Clase Query que se encarga de enviar la solicitud para consultar los reclamos de un usuario.
    /// </summary>
    public class ConsultarReclamosUsuarioQuery : IRequest<List<HistorialReclamosDTO>>
    {
        /// <summary>
        /// Atributo que contiene el correo del usuario 
        /// </summary>
        public string correo { get; set; }

        public ConsultarReclamosUsuarioQuery(string correo)
        {
            this.correo = correo;
        }
    }
}
