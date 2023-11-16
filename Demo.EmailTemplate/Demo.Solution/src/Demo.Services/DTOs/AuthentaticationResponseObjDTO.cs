using System.Net;

namespace Demo.Services.DTOs
{
    public class AuthentaticationResponseObjDTO
    {
        public HttpStatusCode StatusCode { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
}
