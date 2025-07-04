using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class RegistrarReclamoCommand : IRequest<bool>
    {
        public RegistrarReclamoDTO reclamoDto { get; set; }

        public RegistrarReclamoCommand(RegistrarReclamoDTO reclamodto)
        {
            reclamoDto = reclamodto;
        }
    }
}
