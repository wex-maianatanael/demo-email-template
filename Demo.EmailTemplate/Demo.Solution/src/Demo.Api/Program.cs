using Demo.Services;
using Demo.Services.Contracts;
using WEX.Edge.Core.Clients;
using WEX.Edge.Core.Interfaces;
using WEX.Edge.Notification.Clients.Interfaces;
using WEX.Edge.Notification.Clients;
using WEX.Edge.Notification.DTOs.Domain.Settings;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

[assembly: RootNamespace("Demo.Services")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddScoped<IStringLocalizer<EmailService>, StringLocalizer<EmailService>>();

builder.Services.AddSingleton(builder.Configuration.GetSection("NotificationAPI").Get<NotificationAPISettings>());
builder.Services.AddTransient<INotificationClient, NotificationClient>();

builder.Services.AddTransient<IAuthServiceClient>(provider => {

    var authURL = builder.Configuration.GetSection("Auth:Url").Value;
    return new AuthServiceClient(authURL);

});

builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

var supportedCultures = new string[] { "en-US", "pt-BR" };
var supportedCulturesList = supportedCultures.Select(c => new CultureInfo(c)).ToList();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCulturesList,
    SupportedUICultures = supportedCulturesList
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
