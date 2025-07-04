using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    public class ResolucionReclamo
    {
        public Guid Id { get; set; }

        public Guid IdReclamo { get; set; }

        public DescripcionResolucionVO Descripcion { get; set; }
        public FechaResolucionVO FechaResolucion { get; set; }

        public ResolucionReclamo () { }
        public ResolucionReclamo(Guid idReclamo, DescripcionResolucionVO descripcion, FechaResolucionVO fechaResolucion) {
            Id = Guid.NewGuid();
            IdReclamo = idReclamo;
            Descripcion = descripcion;
            FechaResolucion = fechaResolucion;

        }
        [JsonConstructor]

        public ResolucionReclamo(Guid id, Guid idReclamo, DescripcionResolucionVO descripcion, FechaResolucionVO fechaResolucion)
        {
            Id = id;
            IdReclamo = idReclamo;
            Descripcion = descripcion;
            FechaResolucion = fechaResolucion;

        }

    }
}
