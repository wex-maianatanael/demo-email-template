using WEX.Edge.Core.Wrappers;

namespace Demo.Services.Contracts
{
    public interface IEmailService
    {
        Task<Response<bool>> SendWelcomeEmail(string recipientName, string recipientEmail);
    }
}
