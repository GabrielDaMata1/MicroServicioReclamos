using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    /// <summary>
    /// Clase Command que se encarga de enviar la solicitud de resolución de un reclamo de una subasta.
    /// </summary>
    public class RegistrarResolucionReclamoCommand : IRequest<bool>
    {
        /// <summary>
        /// Atributo DTO que contiene la información de la resolución del reclamo de la subasta.
        /// </summary>
        public RegistrarResolucionReclamoDTO resolucionReclamoDTO { get; set; }

        public RegistrarResolucionReclamoCommand(RegistrarResolucionReclamoDTO resolucionReclamoDTO)
        {
            this.resolucionReclamoDTO = resolucionReclamoDTO;
        }
    }
}
