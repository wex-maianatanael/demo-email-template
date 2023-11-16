using Demo.Api.ViewModels;
using Demo.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("api/emails")]
    public class EmailsController : ControllerBase
    {
        private readonly ILogger<EmailsController> _logger;
        private readonly IEmailService _emailService;

        public EmailsController(ILogger<EmailsController> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Post([FromBody] EmailRequestViewModel model)
        {
            try
            {
                var response = await _emailService.SendWelcomeEmail(model.RecipientName, model.RecipientEmail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when sending the email: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}