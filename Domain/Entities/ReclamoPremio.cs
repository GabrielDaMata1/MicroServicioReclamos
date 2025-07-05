using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Domain.Entities
{
    public class ReclamoPremio
    {
        public Guid Id { get; set; }

        public Guid IdSubasta { get; set; }

        public Guid IdUsuario { get; set; }

        public DireccionEnvioPremioVO DireccionEnvio { get; set; }
        public MetodoEntregaPremioVO MetodoEntrega { get; set; }

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
