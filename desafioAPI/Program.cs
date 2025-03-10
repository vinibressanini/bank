using desafioAPI.Bus;
using desafioAPI.Context;
using desafioAPI.DI;
using desafioAPI.Events;
using desafioAPI.Repositories;
using desafioAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace desafioAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.SetBasePath($"{Directory.GetCurrentDirectory()}/Settings/").AddJsonFile("appsettings.json");

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API com JWT", Version = "v1" });

                // Configuração para incluir o suporte ao JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please provide a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });

            builder.Services.AddRabbitMQService();

            builder.Services.AddDbContext<AppDBContext>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<WalletRepository>();
            builder.Services.AddScoped<WalletService>();
            builder.Services.AddScoped<TransactionRepository>();
            builder.Services.AddScoped<TransactionService>();
            builder.Services.AddScoped<AuthorizationService>();
            builder.Services.AddScoped<IBus<TransactionCreatedEvent>,Bus<TransactionCreatedEvent>>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = "viniciusaudience",
                        ValidIssuer = "viniciusAPI",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9gTPwPFkP3AkFM3bEhefotdJcZKQQVvMpbVyo82J9WyX4zP67DNWuDTQbduXonPH"))
                    };


                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
