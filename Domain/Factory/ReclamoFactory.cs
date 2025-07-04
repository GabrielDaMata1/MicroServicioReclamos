using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;

namespace Domain.Factory
{
    public static class ReclamoFactory
    {
        public static Reclamo CrearReclamo(Guid idusuario, Guid idSubasta, string descripcion, string motivo, string urlEvidencia)
        {
            var descripcionVO = new DescripcionReclamoVO(descripcion);
            var motivoVO = new MotivoReclamoVO(motivo);
            var urlEvidenciaVO = new UrlEvidenciaReclamoVO(urlEvidencia);
            var fechaCreacionVO = new FechaCreacionReclamoVO(DateTime.UtcNow);
            var estadoVO = new EstadoReclamoVO("Pendiente");
            return new Reclamo(idusuario, idSubasta, descripcionVO, motivoVO, urlEvidenciaVO, fechaCreacionVO, estadoVO);
        }
    }
}
