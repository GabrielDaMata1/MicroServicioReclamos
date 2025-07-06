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
    /// Clase Query que se encarga de enviar la solicitud para consultar los reclamo en realizado a un subastador .
    /// </summary>
    public class ConsultarReclamosPorSubastadorQuery : IRequest<List<HistorialReclamosDTO>>
    {
        /// <summary>
        /// Atributo que contiene el correo del subastador 
        /// </summary>
        public string correo { get; set; }

        public ConsultarReclamosPorSubastadorQuery(string correo) 
        {
            this.correo = correo;
        }
    }
}
