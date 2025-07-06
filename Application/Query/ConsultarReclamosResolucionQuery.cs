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
    /// <summary>
    /// Clase Query que se encarga de enviar la solicitud para consultar los reclamos y su resolución de un subastador .
    /// </summary>
    public class ConsultarReclamosResolucionQuery : IRequest<List<ReclamoResolucionDTO>>
    {
        /// <summary>
        /// Atributo que contiene el correo del subastador 
        /// </summary>
        public string correo { get; set; }

        public ConsultarReclamosResolucionQuery(string correo)
        {
            this.correo = correo;
        }
    }
}
