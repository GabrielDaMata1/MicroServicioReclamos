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
    public class SubastaService: ISubastaService
    {
        private readonly HttpClient _httpClient;

        public SubastaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
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
                Console.WriteLine( contenido );

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
