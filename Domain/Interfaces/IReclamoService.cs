using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IReclamoService
    {
        Task<HttpStatusCode> RegistrarReclamoMongoAsync(Reclamo reclamo);

        Task<Guid> RegistrarReclamoPostgreSQLAsync(Reclamo reclamo);

    }
}
