using Demo.Services.Contracts;
using Microsoft.Extensions.Localization;
using WEX.Edge.Core.DTOs.AuthWexEdge;
using WEX.Edge.Core.Wrappers;
using WEX.Edge.Notification.Clients.Interfaces;
using WEX.Edge.Notification.DTOs.Email;

namespace Demo.Services
{
    public class EmailService : IEmailService
    {
        private readonly IStringLocalizer<EmailService> _localizer;
        private readonly INotificationClient _notificationClient;
        private readonly IAuthenticationService _authenticationService;

        public EmailService(IStringLocalizer<EmailService> localizer, INotificationClient notificationClient, IAuthenticationService authenticationService)
        {
            _localizer = localizer;
            _notificationClient = notificationClient;
            _authenticationService = authenticationService;
        }

        public async Task<Response<bool>> SendWelcomeEmail(string recipientName, string recipientEmail)
        {
            var subject = _localizer["WelcomeSubject"].Value;
            var body = string.Format(_localizer["WelcomeBody"].Value, recipientName);

            var substitutions = new List<Substitution>()
            {
                new Substitution()
                {
                    Key = "$BODY",
                    Value = body
                }
            };

            string path = GetEmailTemplateFilePath();

            var contet = new List<Content>()
            {
                new Content()
                {
                    Type = WEX.Edge.Notification.DTOs.Domain.Enums.EMailType.HTML,
                    Value = File.ReadAllText(path)
                }
            };

            var mailMessage = new EmailRequest
            {
                Subject = subject,
                From = "no-reply@wexedge.com",
                Tos = new List<string>() { recipientEmail },
                Contents = contet,
                Substitutions = substitutions
            };

            var token = GetAuthenticationToken();

            return await _notificationClient.NotificationSendEmail(mailMessage, token);
        }

        private AuthWexEdgeResponse GetAuthenticationToken()
        {
            var authResponse = _authenticationService.Authenticate();

            return new AuthWexEdgeResponse()
            {
                Login = new Login()
                {
                    Success = true,
                    Token = authResponse.AccessToken
                }
            };
        }

        private string GetEmailTemplateFilePath()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var templatesFolder = "Templates";
            var emailsFolder = "Emails";
            var file = "EmailTemplate.html";
            return Path.Combine(baseDir, templatesFolder, emailsFolder, file);
        }
    }
}
