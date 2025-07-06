using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.Exception;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    /// <summary>
    /// Clase Handler que se encarga registrar un reclamo realizado por un usuario ganador de la subasta.
    /// </summary>
    public class RegistrarReclamoHandler : IRequestHandler<RegistrarReclamoCommand, bool>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un reclamo, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IReclamoService _reclamoService;
        /// <summary>
        /// Atributo que corresponde a las publicación de mensajes a la cola de RabbitMQ.
        /// </summary>
        private readonly IPublishEndpoint _publishEndpoint;

        public RegistrarReclamoHandler(IUsuarioService usuarioService, IReclamoService reclamoService, IPublishEndpoint publishEndpoint)
        {
            _reclamoService = reclamoService;
            _usuarioService = usuarioService;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Método que se encarga de procesar el registro de un reclamo realizado a un subastador.
        /// </summary>
        /// <param name="request">Parametro que contiene un DTO con el correo del usuario que realiza el reclamo, el ID de la subasta
        /// y el detalle del reclamo.</param>
        /// <returns>Retorna un valor booleano True si las operaciones fueron exitosas.</returns>
        /// <exception cref="UsuarioNoEncontradoException">
        /// Esta excepcion ocurre si no se pudo obtener el ID del usuario en el Microservicio Usuarios.
        /// </exception>
        /// <exception cref="FalloAlRegistrarReclamoException">
        /// Esta excepcion ocurre si ocurre un error al registrar el reclamo en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<bool> Handle(RegistrarReclamoCommand request, CancellationToken cancellationToken)
        {

            try
            {

                // Se obtiene el ID del usuario que realiza el reclamo de la subasta.
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.reclamoDto.correo);

                //En caso de que el ID del usuario retornado por la consulta sea vacío, se lanza la excepción
                if (idUsuario == Guid.Empty || idUsuario == null)
                    throw new UsuarioNoEncontradoException();

                //Se crea la instancia del objeto Reclamo
                var reclamo = ReclamoFactory.CrearReclamo(idUsuario, request.reclamoDto.idSubasta, request.reclamoDto.descripcion, request.reclamoDto.motivo, request.reclamoDto.urlEvidencia);

                //Se registra el reclamo en la base de datos de PostgreSQL
                var idReclamo = await _reclamoService.RegistrarReclamoPostgreSQLAsync(reclamo);

                // En caso de que la operación para registrar el reclamo en PostgreSQL no sea exitosa, se lanza una excepción.
                if (idReclamo == Guid.Empty)
                    throw new FalloAlRegistrarReclamoException();

                //Se publica el mensaje en la cola de RabbitMQ para sincronizar la base de datos de MongoDB con PostgreSQL
                await _publishEndpoint.Publish(new ReclamoRegistradoEvent(reclamo));

                return true;

            }
            catch (UsuarioNoEncontradoException)
            {
                throw;
            }
            catch (FalloAlRegistrarReclamoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlRegistrarReclamoException("Ha ocurrido un error al registrar el reclamo en la BD", ex);
            }
        }
    }

}
