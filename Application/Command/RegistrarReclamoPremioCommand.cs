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
    /// Clase Command que se encarga de enviar la solicitud de reclamo de un premio de una subasta.
    /// </summary>
    public class RegistrarReclamoPremioCommand : IRequest<bool>
    {
        /// <summary>
        /// Atributo DTO que contiene la información del reclamo del premio.
        /// </summary>
        public RegistrarReclamoPremioDTO reclamoDto { get; set; }

        public RegistrarReclamoPremioCommand(RegistrarReclamoPremioDTO reclamodto)
        {
            reclamoDto = reclamodto;
        }
    }
}
