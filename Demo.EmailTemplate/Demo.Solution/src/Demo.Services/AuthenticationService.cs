using Demo.Services.Contracts;
using Demo.Services.DTOs;
using Microsoft.Extensions.Logging;
using System.Net;
using WEX.Edge.Core.Interfaces;
using WEX.Edge.Core.Models;

namespace Demo.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IAuthServiceClient _authServiceClient;

        public AuthenticationService(ILogger<AuthenticationService> logger, IAuthServiceClient authServiceClient)
        {
            _logger = logger;
            _authServiceClient = authServiceClient;
        }

        public AuthentaticationResponseObjDTO Authenticate()
        {
            try
            {
                var authResponse = _authServiceClient.Login(new LoginRequest
                {
                    // todo: these values should be added in the appsettings file
                    Username = "notificationapi",
                    Password = "57PlwdMY7rnVAZGYHGWReTqVWVFRJsvlkSC6lOmQd"
                }).Result;

                if (authResponse.Login != null && authResponse.Login.Success)
                {
                    return new AuthentaticationResponseObjDTO()
                    {
                        StatusCode = HttpStatusCode.OK,
                        AccessToken = authResponse.Login.Token,
                        TokenType = "Bearer",
                        ExpiresIn = 36000
                    };
                }

                return new AuthentaticationResponseObjDTO()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    AccessToken = string.Empty,
                    ExpiresIn = 0,
                    TokenType = string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error authenticating user in the Auth2 service: {ex.Message}", ex);
                throw;
            }
        }
    }
}
