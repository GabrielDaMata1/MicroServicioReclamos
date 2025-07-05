using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class RegistrarReclamoPremioCommand : IRequest<bool>
    {
        public RegistrarReclamoPremioDTO reclamoDto { get; set; }

        public RegistrarReclamoPremioCommand(RegistrarReclamoPremioDTO reclamodto)
        {
            reclamoDto = reclamodto;
        }
    }
}
