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
    /// Clase Query que se encarga de enviar la solicitud para consultar los reclamo realizados por los usuarios .
    /// </summary>
    public class ConsultarReclamosQuery : IRequest<List<HistorialReclamosDTO>>
    {
    }
}
