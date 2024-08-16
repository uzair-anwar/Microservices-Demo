using Okta.AspNetCore;
using Microsoft.OpenApi.Models;
using Users.Data.Models;
using Users.Services;
using Users.Data;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHealthChecks();

builder.Services.AddHttpClient("OktaClient", client =>
{
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Okta Authorization header using Bearer scheme. Example: \"Authorization: Bearer <token>\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            },
            new string[] {}
        }
    });
});

builder.Services.Configure<OktaTokenSettings>(builder.Configuration.GetSection("Okta"));
var oktaSettings = builder.Configuration.GetSection("Okta").Get<OktaTokenSettings>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    option.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    option.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
}).AddOktaWebApi(new OktaWebApiOptions
{
    OktaDomain = oktaSettings.OktaDomain,
    AuthorizationServerId = oktaSettings.AuthorizationServerId,
    Audience = oktaSettings.Audience,
});


builder.Services.AddAuthorization();

builder.Services.AddSingleton<IOktaService, OktaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();