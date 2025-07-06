using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Application.Services
{
    /// <summary>
    /// Clase Service que se encarga de procesar todas las operaciones sobre una subasta, realizando peticiones HTTP al Microservicio Subasta.
    /// </summary>
    public class SubastaService: ISubastaService
    {
        /// <summary>
        /// Atributo que se encarga de procesar las solicitudes a servicios externos.
        /// </summary>
        private readonly HttpClient _httpClient;

        public SubastaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /// <summary>
        /// Método que se encarga de obtener una subasta por su ID en el Microservicio Subasta.
        /// </summary>
        /// <param name="idSubasta">Parametro que corresponde al ID de la subasta a consultar</param>
        /// <returns>Retorna un objeto Subasta que contiene la informacion de la subasta dada.
        /// Si no lo consigue, retorna null</returns>
        public async Task<Subasta> ObtenerSubastaPorGuid(Guid idSubasta)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5003/api/Subastas/obtenerSubasta/{idSubasta}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var contenido = await response.Content.ReadAsStringAsync();

                var dto = JsonSerializer.Deserialize<SubastaDTO>(contenido, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dto == null)
                {
                    return null;
                }

                var subasta = new Subasta(
                    dto.Id,
                    new NombreSubastaVO(dto.nombreSubasta),
                    new DescripcionSubastaVO(dto.descripcionSubasta),
                    dto.idProductoSubasta,
                    new FechaInicioSubastaVO(dto.fechaInicioSubasta),
                    new FechaFinSubastaVO(dto.fechaFinSubasta),
                    new IncrementoMinimoSubastaVO(dto.incrementoMinimoSubasta),
                    new PrecioReservaSubastaVO(dto.precioReservaSubasta),
                    new EstadoSubastaVO(dto.estadoSubasta),
                    dto.idUsuario

                );

                return subasta;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
    }
}
