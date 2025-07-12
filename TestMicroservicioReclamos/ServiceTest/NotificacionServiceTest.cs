using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Service;
using Microsoft.Extensions.Configuration;
using Moq.Protected;
using Moq;

namespace TestMicroservicioReclamos.ServiceTest
{
    public class NotificacionServiceTest
    {
        private readonly Mock<HttpMessageHandler> _httpHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly NotificacionService _service;

        public NotificacionServiceTest()
        {
            _httpHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpHandlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5287")
            };

            var configMock = new Mock<IConfiguration>();
            _service = new NotificacionService(_httpClient, configMock.Object);
        }

        [Fact]
        public async Task EnviarCorreoConfirmacionSubastadorReclamoPremio_DeberiaRetornarTrue_CuandoRespuestaExitosa()
        {
            SetupHttpResponse("/api/Notification/enviarCorreoSubastadorConfirmacionReclamoPremio", true);

            var result = await _service.EnviarCorreoConfirmacionSubastadorReclamoPremio("destinatario@mail.com", "Subasta 1", "usuario@mail.com");

            Assert.True(result);
        }

        [Fact]
        public async Task EnviarCorreoSubastadorReclamoPremio_DeberiaRetornarFalse_CuandoRespuestaFalla()
        {
            SetupHttpResponse("/api/Notification/enviarCorreoSubastadorReclamoPremio", false);

            var result = await _service.EnviarCorreoSubastadorReclamoPremio("subastador@mail.com", "Subasta 2", "reclamante@mail.com");

            Assert.False(result);
        }

        [Fact]
        public async Task EnviarCorreoUsuarioResolucionReclamo_DeberiaRetornarTrue_CuandoRespuestaExitosa()
        {
            SetupHttpResponse("/api/Notification/enviarCorreoUsuarioResolucionReclamo", true);

            var result = await _service.EnviarCorreoUsuarioResolucionReclamo("usuario@mail.com", "Subasta final", "Aprobado");

            Assert.True(result);
        }

        private void SetupHttpResponse(string urlPath, bool success)
        {
            _httpHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri!.AbsolutePath.Equals(urlPath, StringComparison.OrdinalIgnoreCase)
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError
                });
        }

    }
}
