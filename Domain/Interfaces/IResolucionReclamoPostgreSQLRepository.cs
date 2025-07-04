using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IResolucionReclamoPostgreSQLRepository
    {
        Task<Guid> RegistrarResolucionReclamo(ResolucionReclamo resolucionReclamo);
    }
}
