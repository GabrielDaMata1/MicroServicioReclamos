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
    /// Clase Command que se encarga de enviar la solicitud de reclamo de un producto de una subasta.
    /// </summary>
    public class RegistrarReclamoCommand : IRequest<bool>   
    {
        /// <summary>
        /// Atributo DTO que contiene la información del reclamo del producto.
        /// </summary>
        public RegistrarReclamoDTO reclamoDto { get; set; }

        public RegistrarReclamoCommand(RegistrarReclamoDTO reclamodto)
        {
            reclamoDto = reclamodto;
        }
    }
}
