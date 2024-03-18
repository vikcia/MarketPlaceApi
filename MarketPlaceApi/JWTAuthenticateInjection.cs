using MarketPlaceApi.Interfaces;
using MarketPlaceApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.Text;

namespace MarketPlaceApi;

public static class JWTAuthenticateInjection
{
    public static void AddJWTAuthenticate(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJWTService, JWTService>();

        string secretKey = config.GetSection("Jwt:Key").Value ?? throw new InvalidDataException("JWT SecretKey");
        string issuer = config.GetSection("Jwt:Issuer").Value ?? throw new InvalidDataException("JWT Issuer");
        string audience = config.GetSection("Jwt:Audience").Value ?? throw new InvalidDataException("JWT audience");

        var swaggerGenServiceDescriptor = services.SingleOrDefault
        (
            d => d.ServiceType == typeof(ISwaggerProvider)
        );

        if (swaggerGenServiceDescriptor != null)
        {
            services.Remove(swaggerGenServiceDescriptor);
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Market Place API",
                    Version = "v1",
                    Description = "Market with some endpoint for creating items and creating orders"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
            
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
    }
}
