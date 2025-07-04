using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;

namespace Domain.Factory
{
    public static class ResolucionReclamoFactory
    {
        public static ResolucionReclamo CrearResolucionReclamo(Guid idReclamo, string descripcion)
        {
            var descripcionVO = new DescripcionResolucionVO(descripcion);
            var fechaResolucionVO = new FechaResolucionVO(DateTime.UtcNow);
            return new ResolucionReclamo(idReclamo, descripcionVO, fechaResolucionVO);
        }

        public static ResolucionReclamo CrearResolucionReclamoConId(Guid id, Guid idReclamo, string descripcion, DateTime fecha)
        {
            var descripcionVO = new DescripcionResolucionVO(descripcion);
            var fechaResolucionVO = new FechaResolucionVO(fecha);
            return new ResolucionReclamo(id,idReclamo, descripcionVO, fechaResolucionVO);
        }
    }
}
