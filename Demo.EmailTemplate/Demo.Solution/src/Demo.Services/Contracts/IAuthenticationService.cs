using Demo.Services.DTOs;

namespace Demo.Services.Contracts
{
    public interface IAuthenticationService
    {
        AuthentaticationResponseObjDTO Authenticate();
    }
}
