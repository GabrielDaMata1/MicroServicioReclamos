using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Service
{
    /// <summary>
    /// Clase service que implementa las operaciones que se pueden realizar sobre las notificaciones, en el Microservicio Notificaciones.
    /// </summary>
    public class NotificacionService: INotificacionService
    {
        /// <summary>
        /// Atributo que se encarga de procesar las solicitudes a servicios externos.
        /// </summary>
        private readonly HttpClient _httpClient;

        public NotificacionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Método que envía una notificación por correo electrónico al usuario indicando que el reclamo de una subasta ha sido respondido.
        /// </summary>
        /// <param name="destinatario">Atributo que corresponde al correo electrónico del usuario que recibió la resolución del reclamo.</param>
        /// <param name="nombreSubasta">Atributo que corresponde al nombre de la subasta involucrada en el reclamo.</param>
        /// <param name="resolucion">Atributo que corresponde a la resolución del reclamo por parte del subastador.</param>
        /// <returns>Retorna un valor booleano que indica si el envío del correo fue exitoso.</returns>
        public async Task<bool> EnviarCorreoConfirmacionSubastadorReclamoPremio(string destinatario, string nombreSubasta, string correoUsuario)
        {
            var payload = new
            {
                destinatario,
                nombreSubasta,
                correoUsuario
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5287/api/Notification/enviarCorreoSubastadorConfirmacionReclamoPremio", content);
                return response.IsSuccessStatusCode;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Método que envía una notificación por correo electrónico al subastador indicando el reclamo del premio una subasta por parte del usuario ganador.
        /// </summary>
        /// <param name="destinatario">Atributo que corresponde al correo electrónico del subastador que recibió el reclamo del premio.</param>
        /// <param name="nombreSubasta">Atributo que corresponde al nombre de la subasta involucrada en el reclamo.</param>
        /// <param name="correoUsuario">Atributo que corresponde al correo electrónico del usuario que reclamó el premio de la subasta.</param>
        /// <returns>Retorna un valor booleano que indica si el envío del correo fue exitoso.</returns>
        public async Task<bool> EnviarCorreoSubastadorReclamoPremio(string destinatario, string nombreSubasta, string correoUsuario)
        {
            var payload = new
            {
                destinatario,
                nombreSubasta,
                correoUsuario
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5287/api/Notification/enviarCorreoSubastadorReclamoPremio", content);
                return response.IsSuccessStatusCode;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Método que envía una notificación por correo electrónico al usuario indicando que el reclamo de una subasta ha sido respondido.
        /// </summary>
        /// <param name="destinatario">Atributo que corresponde al correo electrónico del usuario que recibió la resolución del reclamo.</param>
        /// <param name="nombreSubasta">Atributo que corresponde al nombre de la subasta involucrada en el reclamo.</param>
        /// <param name="resolucion">Atributo que corresponde a la resolución del reclamo por parte del subastador.</param>
        /// <returns>Retorna un valor booleano que indica si el envío del correo fue exitoso.</returns>
        public async Task<bool> EnviarCorreoUsuarioResolucionReclamo(string destinatario, string nombreSubasta, string resolución)
        {
            var payload = new
            {
                destinatario,
                nombreSubasta,
                resolución
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:5287/api/Notification/enviarCorreoUsuarioResolucionReclamo", content);
                return response.IsSuccessStatusCode;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}
