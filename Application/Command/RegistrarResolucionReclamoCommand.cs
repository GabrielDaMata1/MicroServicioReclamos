using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class RegistrarResolucionReclamoCommand : IRequest<bool>
    {
        public RegistrarResolucionReclamoDTO resolucionReclamoDTO { get; set; }

        public RegistrarResolucionReclamoCommand(RegistrarResolucionReclamoDTO resolucionReclamoDTO)
        {
            this.resolucionReclamoDTO = resolucionReclamoDTO;
        }
    }
}
