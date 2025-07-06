using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    /// <summary>
    /// Clase Entity que representa a la entidad Reclamo en el dominio del sistema.
    /// </summary>
    public class Reclamo
    {
        /// <summary>
        /// Atributo que corresponde al ID del reclamo .
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del usuario que hace el reclamo .
        /// </summary>
        public Guid IdUsuario { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta que se le hace un reclamo .
        /// </summary>
        public Guid IdSubasta { get; set; }

        /// <summary>
        /// Atributo que corresponde a la descripción del reclamo .
        /// </summary>
        public DescripcionReclamoVO Descripcion { get; set; }

        /// <summary>
        /// Atributo que corresponde al motivo del reclamo .
        /// </summary>
        public MotivoReclamoVO Motivo { get; set; }

        /// <summary>
        /// Atributo que corresponde a la dirección URL del reclamo en Firebase .
        /// </summary>
        public UrlEvidenciaReclamoVO UrlEvidencia { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado del reclamo .
        /// </summary>
        public EstadoReclamoVO EstadoReclamo { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha de creación del reclamo .
        /// </summary>
        public FechaCreacionReclamoVO FechaCreacion { get; set; }

        public Reclamo() { }
        public Reclamo( Guid idUsuario, Guid idSubasta, DescripcionReclamoVO descripcion, MotivoReclamoVO motivo, UrlEvidenciaReclamoVO urlEvidencia, FechaCreacionReclamoVO fechaCreacion, EstadoReclamoVO estadoReclamo)
        {
            Id = Guid.NewGuid();
            IdUsuario = idUsuario;
            IdSubasta = idSubasta;
            Descripcion = descripcion;
            Motivo = motivo;
            UrlEvidencia = urlEvidencia;
            FechaCreacion = fechaCreacion;
            EstadoReclamo = estadoReclamo;
        }
        [JsonConstructor]
        public Reclamo(Guid id, Guid idUsuario, Guid idSubasta, DescripcionReclamoVO descripcion, MotivoReclamoVO motivo, UrlEvidenciaReclamoVO urlEvidencia, FechaCreacionReclamoVO fechaCreacion, EstadoReclamoVO estadoReclamo)
        {
            Id = id;
            IdUsuario = idUsuario;
            IdSubasta = idSubasta;
            Descripcion = descripcion;
            Motivo = motivo;
            UrlEvidencia = urlEvidencia;
            FechaCreacion = fechaCreacion;
            EstadoReclamo = estadoReclamo;

        }
    }
}
