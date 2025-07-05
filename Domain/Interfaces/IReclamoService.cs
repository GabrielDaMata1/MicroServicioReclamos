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
        Task<List<Reclamo>> ConsultarReclamosMongoAsync();

        Task<Reclamo> ConsultarReclamoMongoAsync(Guid idReclamo);

        Task<List<Reclamo>> ConsultarReclamosPorSubastadorMongoAsync(Guid idSubastador);

        Task<HttpStatusCode> RegistrarResolucionReclamoMongoAsync(ResolucionReclamo resolucionReclamo);

        Task<Guid> RegistrarResolucionReclamoPostgreSQLAsync(ResolucionReclamo resolucionReclamo);

        Task<HttpStatusCode> ActualizarEstadoReclamoMongoAsync(Guid idReclamo, string nuevoEstado);
        Task<HttpStatusCode> ActualizarEstadoReclamoPostgreSQLAsync(Guid idReclamo, string nuevoEstado);

        Task<ResolucionReclamo> ConsultarResolucionReclamoMongoAsync(Guid idReclamo);

        Task<HttpStatusCode> RegistrarReclamoPremioMongoAsync(ReclamoPremio reclamoPremio);

        Task<Guid> RegistrarReclamoPremioPostgreSQLAsync(ReclamoPremio reclamoPremio);

        Task<List<ReclamoPremio>> ConsultarReclamosPremiosUsuarioMongoAsync(Guid idUsuario);

        Task<ReclamoPremio> ConsultarReclamoPremioMongoAsync(Guid idReclamoPremio);

    }
}
