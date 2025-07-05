using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;

namespace Domain.Factory
{
    public static class ReclamoPremioFactory
    {
        public static ReclamoPremio CrearReclamoPremio(Guid idusuario, Guid idSubasta, string direccion, string metodoEntrega)
        {
            var direccionVO = new DireccionEnvioPremioVO(direccion);
            var metodoEntregaVO = new MetodoEntregaPremioVO(metodoEntrega);
            var fechaReclamoVO = new FechaReclamoPremioVO(DateTime.UtcNow);
            return new ReclamoPremio(idusuario, idSubasta, direccionVO, metodoEntregaVO, fechaReclamoVO);
        }

        public static ReclamoPremio CrearReclamoPremioConId(Guid id, Guid idusuario, Guid idSubasta, string direccion, string metodoEntrega, DateTime fecha)
        {
            var direccionVO = new DireccionEnvioPremioVO(direccion);
            var metodoEntregaVO = new MetodoEntregaPremioVO(metodoEntrega);
            var fechaReclamoVO = new FechaReclamoPremioVO(fecha);
            return new ReclamoPremio(id, idusuario, idSubasta, direccionVO, metodoEntregaVO, fechaReclamoVO);
        }
    }
}
