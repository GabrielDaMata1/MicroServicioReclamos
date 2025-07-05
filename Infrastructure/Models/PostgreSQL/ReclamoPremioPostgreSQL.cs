using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models.PostgreSQL
{
    public class ReclamoPremioPostgreSQL
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid IdUsuario { get; set; }

        [Required]
        public Guid IdSubasta { get; set; }
        [Required]
        public string DireccionEnvio { get; set; }
        [Required]
        public string MetodoEntrega { get; set; }
        [Required]
        public DateTime FechaReclamo { get; set; }
    }
}
