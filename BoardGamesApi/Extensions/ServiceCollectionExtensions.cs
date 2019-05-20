using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;

namespace BoardGamesApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtBearerAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        var tokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = configuration["Tokens:Issuer"],
                            ValidAudience = configuration["Tokens:Issuer"],
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(configuration["Tokens:Key"]))
                        };

                        options.TokenValidationParameters = tokenValidationParameters;
                    });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerDocument(
                options =>
                {
                    options.OperationProcessors.Add(
                        new OperationSecurityScopeProcessor("jwt-token"));
                    options.DocumentProcessors.Add(
                        new SecurityDefinitionAppender(
                            "jwt-token", new[] {""}, new SwaggerSecurityScheme
                            {
                                Type = SwaggerSecuritySchemeType.ApiKey,
                                Name = "Authorization",
                                Description =
                                    "Enter \"Bearer jwt-token\" as value. " +
                                    "Use https://localhost:44393/get-token to get read-only JWT token. " +
                                    "Use https://localhost:44393/get-token?admin=true to get admin (read-write) JWT token.",
                                In = SwaggerSecurityApiKeyLocation.Header
                            }));

                    options.PostProcess = document =>
                    {
                        document.Info.Version = "v1";
                        document.Info.Title = "Board Games API v1";
                        document.Info.Description = "A sample API for presentation purpose";
                        document.Info.TermsOfService = "Do whatever you want with it :)";
                        document.Info.Contact = new SwaggerContact
                        {
                            Name = "Miroslav Popovic",
                            Email = string.Empty,
                            Url = "https://miroslavpopovic.com"
                        };
                        document.Info.License = new SwaggerLicense
                        {
                            Name = "MIT",
                            Url = "https://opensource.org/licenses/MIT"
                        };
                    };
                });
        }
    }
}
