using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace plataforma_videos_api
{
    public static class StartupExtensions
    {
        private const string TOKEN_MESSAGE = "Inclua no campo abaixo a palavra 'Bearer' seguida de espaço e do token JWT";
        private const string FIREBASE_URI = "https://securetoken.google.com/{0}";
        private const string SWAGGER_ENDPOINT = "/swagger/{0}/swagger.json";

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(configuration.GetValue<string>("ApplicationConfig:Version"), new OpenApiInfo
                {
                    Title = configuration.GetValue<string>("ApplicationConfig:Name"),
                    Version = configuration.GetValue<string>("ApplicationConfig:Version")
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = TOKEN_MESSAGE,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                }
                            },
                            new string[0]
                        }
                    });
                c.DocInclusionPredicate((docName, apiDesc) => apiDesc.HttpMethod != null);
            });

            return services;
        }

        public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.Authority = String.Format(FIREBASE_URI, configuration.GetValue<string>("FirebaseConfig:projectId"));
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = String.Format(FIREBASE_URI, configuration.GetValue<string>("FirebaseConfig:projectId")),
                       ValidateAudience = true,
                       ValidAudience = configuration.GetValue<string>("FirebaseConfig:projectId"),
                       ValidateLifetime = true
                   };
               });
            return services;
        }


        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    String.Format(
                        SWAGGER_ENDPOINT,
                        configuration.GetValue<string>("ApplicationConfig:Version")
                        ),
                    String.Concat(
                        configuration.GetValue<string>("ApplicationConfig:Name"),
                        configuration.GetValue<string>("ApplicationConfig:Version")
                  ));
                c.DocExpansion(DocExpansion.None);
            });
            return app;
        }
    }
}
