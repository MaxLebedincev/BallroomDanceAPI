﻿using BallroomDanceAPI.Service.Registeration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BallroomDanceAPI.Service.Registeration
{
    public static class RAuthentication 
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var appSettings = config.GetSection(nameof(AppSettings)).Get<AppSettings>() ?? new();

                options.RequireHttpsMetadata = false; // Отвечает за https
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = appSettings.AuthOptions.Issuer,
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = appSettings.AuthOptions.Audience,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    // установка ключа безопасности
                    IssuerSigningKey = appSettings.AuthOptions.SymmetricSecurityKey,
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddAuthorization();
        }
    }
}
