using Cloud_storage_API.Controllers;
using Cloud_storage_API.Db;
using Cloud_storage_API.Models;
using Cloud_storage_API.Repositories.Interface;
using Cloud_storage_API.Repositories.Source;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Cloud_storage_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        AuthenticationType = "Jwt",
                        ValidIssuer = config["JwtSetting:Issuer"],
                        ValidAudience = config["JwtSetting:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(config["JwtSetting:Key"]!)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true

                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<UserContext>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFilesRepository, FilesRepository>();
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();

            app.Run();
        }
    }
}