using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Models.MongoDB
{
    public class ReclamoMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("IdUsuario")]
        public Guid IdUsuario { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("IdSubasta")]

        public Guid IdSubasta { get; set; }
        [BsonElement("Descripcion")]
        public string Descripcion { get; set; }
        [BsonElement("Motivo")]
        public string Motivo { get; set; }

        [BsonElement("UrlEvidencia")]
        public string UrlEvidencia { get; set; }
        [BsonElement("Estado")]
        public string Estado { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
