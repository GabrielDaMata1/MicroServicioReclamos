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
    /// Clase Entity que representa a la entidad ReclamoPremio en el dominio del sistema.
    /// </summary>
    public class ReclamoPremio
    {
        /// <summary>
        /// Atributo que corresponde al ID del reclamo del premio .
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la subasta que se le hace un reclamo del premio .
        /// </summary>
        public Guid IdSubasta { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID del usuario que hace el reclamo del premio.
        /// </summary>
        public Guid IdUsuario { get; set; }
        /// <summary>
        /// Atributo que corresponde a la direccion de envio del reclamo del premio .
        /// </summary>
        public DireccionEnvioPremioVO DireccionEnvio { get; set; }
        /// <summary>
        /// Atributo que corresponde al metodo de entrega del reclamo del premio .
        /// </summary>
        public MetodoEntregaPremioVO MetodoEntrega { get; set; }
        /// <summary>
        /// Atributo que corresponde a la fecha del reclamo del reclamo del premio .
        /// </summary>
        public FechaReclamoPremioVO FechaReclamo { get; set; }

        public ReclamoPremio(Guid idUsuario, Guid idSubasta, DireccionEnvioPremioVO direccionEnvio, MetodoEntregaPremioVO metodoEntrega, FechaReclamoPremioVO fechaReclamo)
        {
            Id = Guid.NewGuid();
            IdSubasta = idSubasta;
            IdUsuario = idUsuario;
            DireccionEnvio = direccionEnvio;
            MetodoEntrega = metodoEntrega;
            FechaReclamo = fechaReclamo;
        }
        [JsonConstructor]
        public ReclamoPremio(Guid id, Guid idUsuario, Guid idSubasta, DireccionEnvioPremioVO direccionEnvio, MetodoEntregaPremioVO metodoEntrega, FechaReclamoPremioVO fechaReclamo)
        {
            Id = id;
            IdSubasta = idSubasta;
            IdUsuario = idUsuario;
            DireccionEnvio = direccionEnvio;
            MetodoEntrega = metodoEntrega;
            FechaReclamo = fechaReclamo;
        }
    }
}
