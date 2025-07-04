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
    public class Reclamo
    {
        public Guid Id { get; set; }

        public Guid IdUsuario { get; set; }

        public Guid IdSubasta { get; set; }
        public DescripcionReclamoVO Descripcion { get; set; }
        public MotivoReclamoVO Motivo { get; set; }

        public UrlEvidenciaReclamoVO UrlEvidencia { get; set; }

        public EstadoReclamoVO EstadoReclamo { get; set; }

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
