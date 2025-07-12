using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre las notificaciones, en el Microservicio Notificaciones.
    /// </summary>
    public interface INotificacionService
    {
        /// <summary>
        /// Método que envía una notificación por correo electrónico al usuario indicando que el reclamo de una subasta ha sido respondido.
        /// </summary>
        /// <param name="destinatario">Atributo que corresponde al correo electrónico del usuario que recibió la resolución del reclamo.</param>
        /// <param name="nombreSubasta">Atributo que corresponde al nombre de la subasta involucrada en el reclamo.</param>
        /// <param name="resolucion">Atributo que corresponde a la resolución del reclamo por parte del subastador.</param>
        /// <returns>Retorna un valor booleano que indica si el envío del correo fue exitoso.</returns>
        Task<bool> EnviarCorreoUsuarioResolucionReclamo(string destinatario, string nombreSubasta, string resolución);
        /// <summary>
        /// Método que envía una notificación por correo electrónico al subastador indicando el reclamo del premio una subasta por parte del usuario ganador.
        /// </summary>
        /// <param name="destinatario">Atributo que corresponde al correo electrónico del subastador que recibió el reclamo del premio.</param>
        /// <param name="nombreSubasta">Atributo que corresponde al nombre de la subasta involucrada en el reclamo.</param>
        /// <param name="correoUsuario">Atributo que corresponde al correo electrónico del usuario que reclamó el premio de la subasta.</param>
        /// <returns>Retorna un valor booleano que indica si el envío del correo fue exitoso.</returns>
        Task<bool> EnviarCorreoSubastadorReclamoPremio(string destinatario, string nombreSubasta, string correoUsuario);
        /// <summary>
        /// Método que envía una notificación por correo electrónico al subastador indicando la confirmación del reclamo del premio una subasta por parte del usuario ganador.
        /// </summary>
        /// <param name="destinatario">Atributo que corresponde al correo electrónico del subastador que recibió la confirmación reclamo del premio.</param>
        /// <param name="nombreSubasta">Atributo que corresponde al nombre de la subasta involucrada en el reclamo.</param>
        /// <param name="correoUsuario">Atributo que corresponde al correo electrónico del usuario que confirmó el reclamo del premio de la subasta.</param>
        /// <returns>Retorna un valor booleano que indica si el envío del correo fue exitoso.</returns>
        Task<bool> EnviarCorreoConfirmacionSubastadorReclamoPremio(string destinatario, string nombreSubasta, string correoUsuario);



    }
}
