using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    /// <summary>
    /// Clase Entity que representa a la entidad ResolucionReclamo en el dominio del sistema.
    /// </summary>
    public class ResolucionReclamo
    {
        /// <summary>
        /// Atributo que corresponde al ID de la resolucion del reclamo .
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del reclamo .
        /// </summary>
        public Guid IdReclamo { get; set; }
        /// <summary>
        /// Atributo que corresponde la descripcion de la resolucón del reclamo .
        /// </summary>
        public DescripcionResolucionVO Descripcion { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de resolución del reclamo .
        /// </summary>
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
